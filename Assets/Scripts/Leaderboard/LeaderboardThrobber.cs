using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

namespace TriviaGame.Leaderboard
{
    // A simple throbber that works while the web request continues
    public class LeaderboardThrobber : MonoBehaviour
    {
        [Header("Throbber Fields")]
        [SerializeField] private Image m_throbber;

        [Header("Animation Settings")]
        [SerializeField, Tooltip("Angle to rotate at per second.")] private float m_angle;

        public void OnEnable()
        {
            // On enable start rotating.
            m_throbber.rectTransform.DORotate(new Vector3(0f, 0f, m_angle), 1)
                                    .SetLoops(-1, LoopType.Incremental);
        }

        public void OnDisable()
        {
            // Kill the infinite loop
            m_throbber.rectTransform.DOKill();
        }
    }
}