using System;
using System.Collections.Generic;

namespace TriviaGame.Leaderboard
{
    [Serializable]
    public class LeaderboardList
    {
        public int page;
        public bool is_last;
        public List<LeaderboardEntryData> data;
    }
}