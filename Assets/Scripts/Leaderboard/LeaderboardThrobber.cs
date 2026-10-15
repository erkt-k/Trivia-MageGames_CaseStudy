using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

namespace TriviaGame.Leaderboard
{
    public class LeaderboardThrobber : MonoBehaviour
    {
        [Header("Throbber Fields")]
        [SerializeField] private Image m_throbber;

        [Header("Animation Settings")]
        [SerializeField] private float m_angle;

        public void OnEnable()
        {
            m_throbber.rectTransform.DORotate(new Vector3(0f, 0f, m_angle), 1)
                                    .SetLoops(-1);
        }

        public void OnDisable()
        {
            m_throbber.rectTransform.DOKill();
        }
    }
}