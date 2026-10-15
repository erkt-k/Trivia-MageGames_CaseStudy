using TriviaGame.ScoreSystem;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms.Impl;

namespace TriviaGame.FSM
{
    public class MainMenuState : MonoBehaviour, IState
    {
        [SerializeField] private Canvas m_mainMenuCanvas;
        private ScoreManager m_scoreManager; 
        private bool m_waitingForMainScene;
        public void Enter()
        {
            // If the scene is changing wait for it.
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
            // If done waiting and in wrong scene do nothing. 
            if (!m_waitingForMainScene ||scene.buildIndex != 0) return;

            m_waitingForMainScene = false;
            SceneManager.sceneLoaded -= OnMainSceneLoaded;

            InitializeMainScene();
        } 

        private void InitializeMainScene()
        {
            // Get the fields and initialize them.
            m_scoreManager = FindAnyObjectByType<ScoreManager>();
            m_scoreManager.Initialize();

            m_mainMenuCanvas = GameObject.FindGameObjectWithTag("HomeBody").GetComponent<Canvas>();
            m_mainMenuCanvas.enabled = true;
        }

        public void Exit()
        {
            m_mainMenuCanvas.enabled = false;
        }
    }
}