using UnityEngine;
using UnityEngine.Events;

namespace TriviaGame.ScoreSystem
{
    [RequireComponent(typeof(ScoreUIManager))]
    public class ScoreManager : MonoBehaviour
    {
        private static ScoreManager s_instance;

        [Header("Score Manager Fields")]
        [SerializeField] private ScoreUIManager m_scoreUIManager;

        private int m_score;
        private const string m_scoreSaveKey = "PlayerScore"; 

        public int Score => m_score;
        public UnityEvent OnScoreChange => new UnityEvent();

        void Awake()
        {
            // there is only one score system manager so it is a Singleton.
            if (s_instance != null && s_instance != this)
            {
                Destroy(gameObject);
                return;
            }

            s_instance = this;
            DontDestroyOnLoad(gameObject);

            Initialize();
        }

        public void Initialize()
        {
            m_score = GetScore();

            if (m_scoreUIManager == null) m_scoreUIManager.GetComponent<ScoreUIManager>();
            m_scoreUIManager.Initialize(m_score);
        }

        /// <summary>
        /// Updates and saves the score.
        /// </summary>
        /// <param name="change"></param>
        public void UpdateScore(int change)
        {
            // Update the score
            m_score += change;
            m_score = Mathf.Max(0, m_score); // score can't be lower then 0.

            // Save the score
            SaveScore(m_score);

            // Animate the score change
            m_scoreUIManager.ShowScoreChange(change, m_score);

            OnScoreChange.Invoke();
        }

        private void SaveScore(int score)
        {
            PlayerPrefs.SetInt(m_scoreSaveKey, score);
            PlayerPrefs.Save();
        }

        private int GetScore()
        {
            return PlayerPrefs.GetInt(m_scoreSaveKey);
        }
    }
}
