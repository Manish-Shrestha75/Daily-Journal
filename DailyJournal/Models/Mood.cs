using System;
using System.Collections.Generic;

namespace DailyJournal.Models
{
    public static class Mood
    {
        // Static list of predefined moods
        public static List<string> All = new List<string>
        {
            "Neutral", "Happy", "Sad", "Excited",
            "Stressed", "Grateful", "Tired",
            "Energetic", "Calm", "Anxious", "Focused",
            "Bored", "Confused", "Sick"
        };

        // Helper method to get an emoji for a mood 
        public static string GetEmoji(string moodName)
        {
            try
            {
                if (string.IsNullOrEmpty(moodName))
                {
                    return "❓";
                }
                else if (moodName == "Happy")
                {
                    return "😊";
                }
                else if (moodName == "Sad")
                {
                    return "😢";
                }
                else if (moodName == "Excited")
                {
                    return "🤩";
                }
                else if (moodName == "Stressed")
                {
                    return "😫";
                }
                else if (moodName == "Neutral")
                {
                    return "😐";
                }
                else
                {
                    return "📝"; // Default
                }
            }
            catch (Exception)
            {
                return "⚠️"; // Fallback if something fails
            }
        }
    }
}