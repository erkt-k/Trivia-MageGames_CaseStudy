using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class GameTimer : MonoBehaviour
{
    [Header("Timer Customization")]
    [SerializeField] private bool m_isTesting = false;
    [SerializeField, Min(1)] private int m_maxTime = 20;
    public int MaxTime => m_maxTime;
    private Coroutine m_timerCoroutine;

    public UnityEvent OnTimerStartedEvent = new UnityEvent();
    public UnityEvent<int> OnTimerUpdateEvent = new UnityEvent<int>();
    public UnityEvent<TriviaGame.Questions.QuestionResult> OnTimerRunOutEvent = new UnityEvent<TriviaGame.Questions.QuestionResult>();
    public UnityEvent OnTimerReset = new UnityEvent();

    void Start()
    {
        if (m_isTesting)
        {
            StartTimer();
        }
    } 

    public void StartTimer()
    {
        m_timerCoroutine = StartCoroutine(CountDownRoutine());
        OnTimerStartedEvent.Invoke();
    }

    public void ResetTimer()
    {
        if (m_timerCoroutine != null)
        {
            StopCoroutine(m_timerCoroutine);
            m_timerCoroutine = null;
        }

        OnTimerReset.Invoke();
    }

    IEnumerator CountDownRoutine()
    {
        int timePassed = 0;
        while(timePassed < m_maxTime)
        {
            yield return new WaitForSeconds(1);
            timePassed++;
            OnTimerUpdateEvent.Invoke(timePassed);
        }
        OnTimerRunOutEvent.Invoke(TriviaGame.Questions.QuestionResult.TimeOut);
    }
}
