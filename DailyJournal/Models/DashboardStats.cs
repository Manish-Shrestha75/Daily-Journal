using System.Collections.Generic;

namespace DailyJournal.Models
{
    public class DashboardStats
    {
        public int TotalEntries { get; set; }
        public int CurrentStreak { get; set; }
        public Dictionary<string, int> MoodCounts { get; set; } = new Dictionary<string, int>();
    }
}