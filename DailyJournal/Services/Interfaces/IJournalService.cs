using DailyJournal.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DailyJournal.Services.Interfaces
{
    public interface IJournalService
    {
        // CRUD
        Task<List<JournalEntry>> GetHistoryAsync();
        Task<JournalEntry> FindEntryAsync(DateTime date);
        Task UpsertEntryAsync(JournalEntry entry);
        Task DeleteEntryAsync(JournalEntry entry);

        // Search & Stats 
        Task<List<JournalEntry>> SearchJournalAsync(string searchText);
        Task<DashboardStats> GetDashboardStatsAsync();
    }
}