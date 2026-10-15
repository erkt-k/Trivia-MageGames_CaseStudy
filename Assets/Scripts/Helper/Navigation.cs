using TriviaGame.FSM;
using UnityEngine;

/// <summary>
/// A Simple Navigation class that allows me to handle navigation even after changing scenes.
/// If I don't handle getting the State Machine in the navigator, it would lose it after scene changes.
/// </summary>
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