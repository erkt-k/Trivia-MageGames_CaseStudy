using System;

namespace TriviaGame.Leaderboard
{
    [Serializable]
    public class LeaderboardEntryData
    {
        public int rank;
        public string nickname;
        public int score;

        public override string ToString()
        {
            return $"Rank: {rank} | Nickname: {nickname} | Score: {score}";
        }
    }
}