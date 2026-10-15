using TriviaGame.Leaderboard;
using UnityEngine;

namespace TriviaGame.FSM
{
    public class LeaderboardState : MonoBehaviour, IState
    {
        [SerializeField] private LeaderboardGetter m_lbGetter;
        [SerializeField] private LeaderboardUIBuilder m_lbUIBuilder;
        private int m_pageCounter;

        public void Enter()
        {
            // Initialize the fields
            m_pageCounter = 0;
            if (m_lbGetter == null) m_lbGetter = FindAnyObjectByType<LeaderboardGetter>();
            if (m_lbUIBuilder == null) m_lbUIBuilder = FindAnyObjectByType<LeaderboardUIBuilder>();

            // Set the buttons and open UI
            m_lbUIBuilder.PreviousButton.onClick.AddListener(PreviousPage);
            m_lbUIBuilder.NextButton.onClick.AddListener(NextPage);
            m_lbUIBuilder.OpenUI();

            // Start listening for requests
            m_lbGetter.OnRequestStart.AddListener(m_lbUIBuilder.OpenLoadingScreen); 
            m_lbGetter.OnRequestEnd.AddListener(m_lbUIBuilder.CloseLoadingScreen);

            // Get the next entry
            m_lbGetter.GetLeaderboardEntries(m_pageCounter);
        }

        /// <summary>
        /// Moves the counter forward and gets the next entries
        /// (Possible error if this is called at the final page. 
        /// Shouldn't happen but I'll have to look into it if I have time)
        /// </summary>
        public void NextPage()
        {
            m_pageCounter++;

            m_lbGetter.GetLeaderboardEntries(m_pageCounter);
        }

        /// <summary>
        /// Moves the counter backwards and gets the previous entries.
        /// Does nothing if this is the first page.
        /// </summary>
        public void PreviousPage()
        {
            m_pageCounter = Mathf.Max(0, --m_pageCounter);

            m_lbGetter.GetLeaderboardEntries(m_pageCounter);
        }

        public void Exit()
        {
            m_lbUIBuilder.CloseUI();
            m_pageCounter = 0;
            m_lbGetter.OnRequestStart.RemoveListener(m_lbUIBuilder.OpenLoadingScreen);
            m_lbGetter.OnRequestEnd.RemoveListener(m_lbUIBuilder.CloseLoadingScreen);
        }
    }
}