using DailyJournal.Models;
using DailyJournal.Services.Interfaces;
using SQLite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace DailyJournal.Services
{
    public class JournalService : IJournalService
    {
        private SQLiteAsyncConnection? _db;

        private async Task InitAsync()
        {
            if (_db != null) return;
            var dbPath = Path.Combine(FileSystem.AppDataDirectory, "journal_final_v5.db");
            _db = new SQLiteAsyncConnection(dbPath);
            await _db.CreateTableAsync<JournalEntry>();
        }

        public async Task<List<JournalEntry>> GetHistoryAsync()
        {
            await InitAsync();
            return await _db!.Table<JournalEntry>().OrderByDescending(x => x.EntryDate).ToListAsync();
        }

        public async Task<JournalEntry> FindEntryAsync(DateTime date)
        {
            await InitAsync();
            var all = await _db!.Table<JournalEntry>().ToListAsync();
            return all.FirstOrDefault(x => x.EntryDate.Date == date.Date);
        }

        public async Task UpsertEntryAsync(JournalEntry entry)
        {
            await InitAsync();
            if (entry.Id != 0) await _db!.UpdateAsync(entry);
            else await _db!.InsertAsync(entry);
        }

        public async Task DeleteEntryAsync(JournalEntry entry)
        {
            await InitAsync();
            await _db!.DeleteAsync(entry);
        }

        public async Task<List<JournalEntry>> SearchJournalAsync(string searchText)
        {
            await InitAsync();
            if (string.IsNullOrWhiteSpace(searchText))
                return await GetHistoryAsync();

            var all = await _db!.Table<JournalEntry>().ToListAsync();

            return all.Where(x =>
                    x.Content.Contains(searchText, StringComparison.OrdinalIgnoreCase) ||
                    x.PrimaryMood.Contains(searchText, StringComparison.OrdinalIgnoreCase) ||
                    (x.Tags != null &&
                     x.Tags.Contains(searchText, StringComparison.OrdinalIgnoreCase))
                )
                .OrderByDescending(x => x.EntryDate)
                .ToList();
        }





        public async Task<DashboardStats> GetDashboardStatsAsync()
        {
            await InitAsync();
            var all = await _db!.Table<JournalEntry>().ToListAsync();

            return new DashboardStats
            {
                TotalEntries = all.Count,
                CurrentStreak = CalculateStreak(all),
                MoodCounts = all.GroupBy(x => x.PrimaryMood).ToDictionary(g => g.Key, g => g.Count())
            };
        }

        private int CalculateStreak(List<JournalEntry> entries)
        {
            if (entries == null || !entries.Any()) return 0;
            var dates = entries.Select(e => e.EntryDate.Date).Distinct().OrderByDescending(d => d).ToList();

            // Streak is valid only if you wrote today OR yesterday
            if (!dates.Contains(DateTime.Today) && !dates.Contains(DateTime.Today.AddDays(-1))) return 0;

            int streak = 0;
            DateTime check = dates.First();
            foreach (var d in dates)
            {
                if (d == check) { streak++; check = check.AddDays(-1); }
                else break;
            }
            return streak;
        }
    }
}