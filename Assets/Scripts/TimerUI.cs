using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

[RequireComponent(typeof(GameTimer))]
public class TimerUI : MonoBehaviour
{
    [Header("Timer UI Fields")]
    [SerializeField] private GameTimer m_timer;
    [Tooltip("This will give the timer it's actual color."), 
    SerializeField] private Image m_timerBG;
    [Tooltip("This shadow will start to fill to represent the timer running out."), 
    SerializeField] private Image m_timerShadow;
    [SerializeField] private TextMeshProUGUI m_timerText;

    [Header("Timer UI Customization")]
    [SerializeField] private Color m_safeZoneColor = Color.limeGreen;
    [SerializeField] private Color m_midDangerZoneColor = Color.yellowNice;
    [SerializeField] private Color m_DangerZoneColor = Color.indianRed;

    [Header("Animation Settings")]
    [SerializeField] private Vector3 m_puncStrength = Vector3.one;
    [SerializeField, Range(0.1f, 1f)] private float m_duration = 0.2f;
    [SerializeField] private int m_vibrato = 5;
    [SerializeField] private int m_elasticity = 1;
    

    private RectTransform m_rectTransform;
    private Coroutine m_timerCoroutine;

    void Start()
    {
        if (m_timer == null)
        {
            m_timer = GetComponent<GameTimer>();
        }

        m_timer.OnTimerStartedEvent.AddListener(ResetTimerUI);
        m_timer.OnTimerUpdateEvent.AddListener(UpdateTimer);
        m_timer.OnTimerReset.AddListener(ResetTimerUI);

        m_rectTransform = GetComponent<RectTransform>();
    } 

    public void UpdateTimer(int timePassed)
    {
        // Calculate how much the shadow newFillAmount should be  
        float newFillAmount = timePassed / (float) m_timer.MaxTime;
        m_timerShadow.fillAmount  = newFillAmount;
        
        if(newFillAmount <= 0.5) // if less then half time has passed we are in the safe zone
            m_timerBG.color = m_safeZoneColor;
        else if (newFillAmount <= 0.75) // if less then 3/4 time has passed we are in the mid danger zone
        {
            m_timerBG.DOColor(m_midDangerZoneColor, 0.1f);
        }
        else // else we are in the danger zone
        {
            m_timerBG.DOColor(m_DangerZoneColor, 0.1f);
            m_timerText.color = Color.white;
        }

        // calculate how much time is left
        int timeLeft = m_timer.MaxTime - timePassed;
        m_timerText.text = timeLeft.ToString();

        // Play a little punch animation
        m_rectTransform.DOPunchScale(m_puncStrength, m_duration, m_vibrato, m_elasticity).SetEase(Ease.InOutElastic);
    }

    public void ResetTimerUI()
    {
        m_timerShadow.fillAmount = 0f;
        m_timerBG.color = m_safeZoneColor;
        m_timerText.color = Color.black;
    }
}
