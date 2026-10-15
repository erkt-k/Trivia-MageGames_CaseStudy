using UnityEngine;
using DG.Tweening;

namespace TriviaGame.Settings
{
    public class SettingsUIBuilder : MonoBehaviour
    {
        [SerializeField] private Canvas m_settingsCanvas;

        public void Initialize()
        {
            RectTransform canvasRect = m_settingsCanvas.GetComponent<RectTransform>();
            RectTransform parentRect = m_settingsCanvas.rootCanvas.GetComponent<RectTransform>();

            float height = parentRect.rect.height;
            float margin = 50f;

            Vector2 onScreenPos = canvasRect.anchoredPosition;
            Vector2 offScreenPos = parentRect.anchoredPosition + Vector2.up * (height + margin);

            canvasRect.anchoredPosition = offScreenPos;
            m_settingsCanvas.enabled = true;
            
            canvasRect.DOAnchorPos(onScreenPos, 0.5f)
                    .SetEase(Ease.OutCubic);
        }

        public void ResetUI()
        {
            RectTransform canvasRect = m_settingsCanvas.GetComponent<RectTransform>();
            RectTransform parentRect = m_settingsCanvas.rootCanvas.GetComponent<RectTransform>();

            float height = parentRect.rect.height;
            float margin = 50f;

            Vector2 onScreenPos = canvasRect.anchoredPosition;
            Vector2 offScreenPos = parentRect.anchoredPosition + Vector2.up * (height + margin);

            canvasRect.DOAnchorPos(offScreenPos, 0.5f)
                    .SetEase(Ease.OutCubic)
                    .OnComplete(() =>
                    {
                        m_settingsCanvas.enabled = false;
                        canvasRect.anchoredPosition = onScreenPos;
                    });
        }
    }
}