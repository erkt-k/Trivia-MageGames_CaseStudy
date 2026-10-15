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

            // height + margin is how much higher we need to be to be offscreen.
            float height = parentRect.rect.height;
            float margin = 50f;

            Vector2 onScreenPos = canvasRect.anchoredPosition;
            Vector2 offScreenPos = parentRect.anchoredPosition + Vector2.up * (height + margin);

            // Put the canvas offscreen
            canvasRect.anchoredPosition = offScreenPos;
            m_settingsCanvas.enabled = true;

            // Animate coming into screen
            canvasRect.DOAnchorPos(onScreenPos, 0.5f)
                    .SetEase(Ease.OutCubic);
        }

        public void ResetUI()
        {
            RectTransform canvasRect = m_settingsCanvas.GetComponent<RectTransform>();
            RectTransform parentRect = m_settingsCanvas.rootCanvas.GetComponent<RectTransform>();

            // Calculate off screen position.
            float height = parentRect.rect.height;
            float margin = 50f;

            Vector2 onScreenPos = canvasRect.anchoredPosition;
            Vector2 offScreenPos = parentRect.anchoredPosition + Vector2.up * (height + margin);

            // Animate leaving screen.
            canvasRect.DOAnchorPos(offScreenPos, 0.5f)
                    .SetEase(Ease.OutCubic)
                    .OnComplete(() =>
                    {
                        // Disable and reset the position.
                        m_settingsCanvas.enabled = false;
                        canvasRect.anchoredPosition = onScreenPos;
                    });
        }
    }
}