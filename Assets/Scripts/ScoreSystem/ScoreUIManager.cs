using DG.Tweening;
using TMPro;
using UnityEngine;

namespace TriviaGame.ScoreSystem
{
    public class ScoreUIManager : MonoBehaviour
    {
        [Header("Score UI Fields")]
        [SerializeField] private TextMeshProUGUI m_scoreText;
        [SerializeField] private TextMeshProUGUI m_scoreChangePopupText;

        [Header("PopUp Animation Settings")]
        [SerializeField, Min(0)] private float m_jumpPower;
        [SerializeField, Min(1)] private int m_numJumps; 
        [SerializeField, Min(0.01f)] private float m_popupDuration; 

        [Header("Score Text Animation Settings")]
        [SerializeField, Min(0.001f)] private float m_scoreDuration;
        [SerializeField] private Vector3 m_strength;
        [SerializeField, Min(0f)] private int m_vibrato; 
        [SerializeField, Range(0f, 180f)] private float m_randomness;
        

        public void Initialize(int score)
        {
            m_scoreText = GameObject.FindGameObjectWithTag("Score").GetComponent<TextMeshProUGUI>();
            string scoreText = score.ToString();
            m_scoreText.text = scoreText;

            Transform scoreParent = m_scoreText.transform.parent;
            m_scoreChangePopupText = scoreParent.GetChild(scoreParent.childCount-1).GetComponent<TextMeshProUGUI>(); // This is a bit hard coded so there must be a better way.
        }

        // Jumps to score text
        public void ShowScoreChange(int change, int newScore)
        {
            // Get info
            RectTransform popupTransform = m_scoreChangePopupText.rectTransform;
            Vector3 initialPos = popupTransform.anchoredPosition;

            RectTransform scoreTextTransform = m_scoreText.rectTransform;

            // Set popUp text
            m_scoreChangePopupText.text =  change >= 0 
                                        ? $"+{change}"
                                        : change.ToString();

            // Animate the text to score
            popupTransform.DOKill();

            popupTransform.gameObject.SetActive(true);
            popupTransform.DOJumpAnchorPos(scoreTextTransform.anchoredPosition,
                                            m_jumpPower, m_numJumps, m_popupDuration)
                            .SetEase(Ease.InOutCubic)
                            .OnKill(() =>
                            {
                                // Reset the popUp
                                m_scoreChangePopupText.gameObject.SetActive(false);
                                popupTransform.anchoredPosition = initialPos;

                                // Animate the score
                                AnimateScoreText(newScore);
                            });
        }

        public void AnimateScoreText(int score)
        {
            RectTransform scoreTextTransform = m_scoreText.rectTransform;

            m_scoreText.text = score.ToString();
            scoreTextTransform.DOShakeScale(m_scoreDuration, m_strength, m_vibrato,
                                            m_randomness);
        }
    }
}