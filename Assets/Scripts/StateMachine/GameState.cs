using TriviaGame.Questions;
using TriviaGame.ScoreSystem;
using TriviaGame.Sounds;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace TriviaGame.FSM
{
    public class GameState : MonoBehaviour, IState
    {
        [Header("Game Settings")]
        [SerializeField] private int m_correctChoicePoints = 10;
        [SerializeField] private int m_wrongChoicePoints = - 5;
        [SerializeField] private int m_timeOutPoints = -3;
        [SerializeField] private QuestionGetter m_questionGetter;
        [SerializeField] private GameTimer m_timer;
        [SerializeField] private ScoreManager m_scoreManager;
        [SerializeField] private SoundManager m_soundManager;

        private Question m_currentQuestion;
        private bool m_waitingForGameScene;

        public UnityEvent OnGameEnd = new UnityEvent();

        public void Enter()
        {
            if (SceneManager.GetActiveScene().buildIndex != 1)
            {
                m_waitingForGameScene = true;
                SceneManager.sceneLoaded -= OnGameSceneLoaded;
                SceneManager.sceneLoaded += OnGameSceneLoaded;
                SceneManager.LoadScene(1);
                return;
            }

            InitializeGameScene();
        }

        private void OnGameSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            if (!m_waitingForGameScene || scene.buildIndex != 1) return;

            m_waitingForGameScene = false;
            SceneManager.sceneLoaded -= OnGameSceneLoaded;
            InitializeGameScene();
        }

        private void InitializeGameScene()
        {
            m_scoreManager = FindAnyObjectByType<ScoreManager>();
            m_scoreManager.Initialize();
            GameObject timerObject = GameObject.FindGameObjectWithTag("Timer");
            if (timerObject == null)
            {
                Debug.LogError("GameState couldn't find Timer.");
                return;
            }

            m_timer = timerObject.GetComponent<GameTimer>();
            if (m_timer == null)
            {
                Debug.LogError("Timer doesn't have Timer Component");
                return;
            }

            m_questionGetter = FindAnyObjectByType<QuestionGetter>();
            if (m_questionGetter == null)
            {
                Debug.LogError("GameState couldn't find Question Getter.");
                return;
            }
            
            m_soundManager = FindAnyObjectByType<SoundManager>();
            if (m_soundManager == null)
            {
                Debug.LogError("GameState couldn't find Sound Manager.");
                return;
            }

            m_timer.OnTimerRunOutEvent.AddListener(OnQuestionResult);
            GetNewQuestion();
        }

        public void Exit()
        {
            m_waitingForGameScene = false;
            SceneManager.sceneLoaded -= OnGameSceneLoaded;
            if (m_timer != null) m_timer.OnTimerRunOutEvent.RemoveAllListeners();
            if (m_currentQuestion != null)
            {
                m_currentQuestion.Unload();
                m_currentQuestion.OnQuestionAnswered.RemoveListener(OnQuestionResult);
            }

            Debug.Log("<color=orange><b>Question Run Out</b></color>");

            
        }

        private void GetNewQuestion()
        {
            if (m_currentQuestion != null)
            {
                m_currentQuestion.OnQuestionAnswered.RemoveAllListeners();
                m_currentQuestion = null;
            }

            m_currentQuestion = m_questionGetter.GetNewQuestion();
            if (m_currentQuestion != null) m_currentQuestion.OnQuestionAnswered.AddListener(OnQuestionResult);
            else OnGameEnd.Invoke();

            m_timer.ResetTimer();
            m_timer.StartTimer();
        }

        public void OnQuestionResult(QuestionResult questionResult)
        {
            switch (questionResult)
            {
                case QuestionResult.Success:
                UpdateScore(m_correctChoicePoints);
                m_soundManager.PlaySuccessSound();
                GetNewQuestion();
                break;
                case QuestionResult.Fail:
                UpdateScore(m_wrongChoicePoints);
                m_soundManager.PlayFailSound();
                GetNewQuestion();
                break;
                case QuestionResult.TimeOut:
                UpdateScore(m_timeOutPoints);
                m_soundManager.PlayFailSound();
                GetNewQuestion();
                break;
            }
        }

        public void UpdateScore(int scoreChange)
        {
            m_scoreManager.UpdateScore(scoreChange);
        }
    }
}