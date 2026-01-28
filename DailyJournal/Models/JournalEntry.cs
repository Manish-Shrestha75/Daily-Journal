using SQLite;
using System;

namespace DailyJournal.Models
{
    public class JournalEntry
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        [Indexed(Unique = true)]
        public DateTime EntryDate { get; set; }

        public string Content { get; set; } = "";
        public string PrimaryMood { get; set; } = "Neutral";
        public string? SecondaryMoods { get; set; }
        public string? Tags { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime LastUpdated { get; set; }
    }
}