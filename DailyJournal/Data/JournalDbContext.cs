using SQLite;
using DailyJournal.Models;

namespace DailyJournal.Data
{
    public class JournalDatabase
    {
        private readonly SQLiteAsyncConnection _database;

        public JournalDatabase(string dbPath)
        {
            _database = new SQLiteAsyncConnection(dbPath);
            _database.CreateTableAsync<JournalEntry>().Wait();
        }

        public Task<List<JournalEntry>> GetEntriesAsync()
            => _database.Table<JournalEntry>().ToListAsync();

        public Task<int> SaveEntryAsync(JournalEntry entry)
        {
            entry.LastUpdated = DateTime.Now;

            if (entry.Id != 0)
                return _database.UpdateAsync(entry);

            entry.CreatedAt = DateTime.Now;
            return _database.InsertAsync(entry);
        }

        public Task<int> DeleteEntryAsync(JournalEntry entry)
            => _database.DeleteAsync(entry);
    }
}
