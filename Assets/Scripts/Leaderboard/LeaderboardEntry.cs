using TMPro;
using UnityEngine;

namespace TriviaGame.Leaderboard
{
    public class LeaderboardEntry : MonoBehaviour
    {
        [Header("Entry Fields")]
        [SerializeField] private TextMeshProUGUI m_rankText;
        [SerializeField] private TextMeshProUGUI m_nicknameText;
        [SerializeField] private TextMeshProUGUI m_scoreText;
        
        public void Initialize(LeaderboardEntryData data)
        {
            m_rankText.text = data.rank.ToString();
            m_nicknameText.text = data.nickname;
            m_scoreText.text = data.score.ToString();
        }
    }
}