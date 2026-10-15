using Unity.VisualScripting;
using UnityEngine;

namespace TriviaGame.FSM
{
    public class StateMachine : MonoBehaviour
    {
        private static StateMachine s_instance;

        [SerializeField] private GameState m_gameState;
        [SerializeField] private LeaderboardState m_leaderboardState;
        [SerializeField] private MainMenuState m_mainMenuState;
        [SerializeField] private SettingsState m_settingsState;

        private IState m_currentState;

        public void Awake()
        {
            if (s_instance != null && s_instance != this)
            {
                Destroy(gameObject);
                return;
            }

            s_instance = this;
            DontDestroyOnLoad(gameObject);

            ChangeState(m_mainMenuState);
        }

        private void ChangeState(IState newState)
        {
            if (m_currentState != null) m_currentState.Exit();
            m_currentState = newState;
            Debug.Log($"New State: {m_currentState}");
            m_currentState.Enter();
        }

        public void ChangeToGameState()
        {
            if (m_currentState is GameState) return;
            if (m_gameState == null)
            {
                m_gameState = FindAnyObjectByType<GameState>();
                if (m_gameState != null)
                {
                    ChangeState(m_gameState);
                    m_gameState.OnGameEnd.AddListener(ChangeToMainMenuState);
                } else
                {
                    Debug.LogError("State Machine CAN'T FIND Game State");
                    return;
                }
            } else
            {
                ChangeState(m_gameState);
                m_gameState.OnGameEnd.AddListener(ChangeToMainMenuState);
            }
        }

        public void ChangeToLeaderboardState()
        {
            if (m_currentState is LeaderboardState) return;
            if (m_leaderboardState == null)
            {
                m_leaderboardState = FindAnyObjectByType<LeaderboardState>();
                if (m_leaderboardState != null) ChangeState(m_leaderboardState);
                else Debug.LogError("State Machine CAN'T FIND Leaderboard State");
            } else
            ChangeState(m_leaderboardState);
        }

        public void ChangeToMainMenuState()
        {
            if (m_currentState is MainMenuState) return;
            if (m_mainMenuState == null)
            {
                m_mainMenuState = FindAnyObjectByType<MainMenuState>();
                if (m_mainMenuState != null)
                {
                    ChangeState(m_mainMenuState);
                    m_gameState.OnGameEnd.RemoveListener(ChangeToMainMenuState);
                } else
                {
                    Debug.LogError("State Machine CAN'T FIND Main Menu State");
                    return;
                }
            } else
            {
                ChangeState(m_mainMenuState);
                m_gameState.OnGameEnd.RemoveListener(ChangeToMainMenuState);
            }
        }

        public void ChangeToSettingsState()
        {
            if (m_currentState is SettingsState) return;
            if (m_settingsState == null)
            {
                m_settingsState = FindAnyObjectByType<SettingsState>();
                if (m_settingsState != null) ChangeState(m_settingsState);
                else Debug.LogError("State Machine CAN'T FIND Settings State");
            } else
            ChangeState(m_settingsState);
        }
    }
}