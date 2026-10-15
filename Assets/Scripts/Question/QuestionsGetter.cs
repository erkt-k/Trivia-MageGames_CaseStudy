using System.Collections.Generic;
using TriviaGame.Questions;
using UnityEngine;
using UnityEngine.Events;

public class QuestionGetter : MonoBehaviour
{
    private QuestionsData m_questionsdata;
    [SerializeField] private List<Question> m_questions;
    [SerializeField] private TextAsset m_questionsJsonAsset;
    [SerializeField] private QuestionUIBuilder m_questionUIBuilder;

    public UnityEvent OnQuestionsRunOutEvent = new UnityEvent();

    private int m_questionCounter;
    private Question m_currentQuestion;

    void Awake()
    {
        // Get the data from the json
        m_questionsdata = FileIOHelper.GetData<QuestionsData>(m_questionsJsonAsset.text);
        m_questions = m_questionsdata.questions;

        // Reset the counter
        m_questionCounter = 0;
        m_currentQuestion = null;
    }

    public Question GetNewQuestion()
    {
        // If used all the question, do not get a question.
        // Announce the questions are done.
        if (m_questionCounter >= m_questions.Count)
        {
            OnQuestionsRunOutEvent.Invoke();
            return null;
        }

        if (m_currentQuestion != null)
        {
            m_currentQuestion.Unload();
        }

        // Get the next question

        Question newQuestion = m_questions[m_questionCounter];
        if (newQuestion != null) m_currentQuestion = newQuestion;
        m_currentQuestion.Initialize(m_questionUIBuilder);

        // Move the counter.
        m_questionCounter++;
        return m_currentQuestion;
    }
}