using TriviaGame.ScoreSystem;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace TriviaGame.FSM
{
    public class MainMenuState : MonoBehaviour, IState
    {
        [SerializeField] private Canvas m_mainMenuCanvas;
        private bool m_waitingForMainScene;
        public void Enter()
        {
            if (SceneManager.GetActiveScene().buildIndex != 0)
            {
                m_waitingForMainScene = true;
                SceneManager.sceneLoaded -= OnMainSceneLoaded;
                SceneManager.sceneLoaded += OnMainSceneLoaded;
                SceneManager.LoadScene(0);
                return;
            }

            InitializeMainScene();
        }

        private void OnMainSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            if (!m_waitingForMainScene ||scene.buildIndex != 0) return;

            m_waitingForMainScene = false;
            SceneManager.sceneLoaded -= OnMainSceneLoaded;
            InitializeMainScene();
        } 

        private void InitializeMainScene()
        {
            m_mainMenuCanvas = GameObject.FindGameObjectWithTag("HomeBody").GetComponent<Canvas>();
            m_mainMenuCanvas.enabled = true;
        }

        public void Exit()
        {
            m_mainMenuCanvas.enabled = false;
        }
    }
}