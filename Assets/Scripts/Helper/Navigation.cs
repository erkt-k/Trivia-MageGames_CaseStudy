using TriviaGame.FSM;
using UnityEngine;

public class Navigation : MonoBehaviour
{

    [SerializeField] private StateMachine m_FSM;
    [SerializeField] private NavigationDestination m_destination;

    void Awake()
    {
        if (m_FSM == null) m_FSM = FindAnyObjectByType<StateMachine>();
    }

    public void Navigate()
    {
        switch (m_destination)
        {
            case NavigationDestination.GameState:
                m_FSM.ChangeToGameState();
            break;
            case NavigationDestination.LeaderboardState:
                m_FSM.ChangeToLeaderboardState();
            break;
            case NavigationDestination.MainMenuState:
                m_FSM.ChangeToMainMenuState();
            break;
            case NavigationDestination.SettingsState:
                m_FSM.ChangeToSettingsState();
            break;
        }
    }
}