using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using TriviaGame.FSM;
using UnityEngine;
using UnityEngine.UI;

namespace TriviaGame.Leaderboard
{
    public class LeaderboardUIBuilder : MonoBehaviour
    {
        [Header("UI Builder Fields")]
        [SerializeField] private List<LeaderboardEntry> m_entries;
        [SerializeField] private TextMeshProUGUI m_pageText; 
        [SerializeField] private GameObject m_failedText;
        [SerializeField] private LeaderboardThrobber m_throbber;
        [SerializeField] private Button m_previousButton;
        [SerializeField] private Button m_nextButton;
        [SerializeField] private Canvas m_lbCanvas;

        public Button PreviousButton => m_previousButton;
        public Button NextButton => m_nextButton;

        public void DrawUI(LeaderboardList list)
        {
            ResetUI();

            for (int i = 0; i < m_entries.Count; i++)
            {
                // If more entry than data, hide the entry
                if (i < list.data.Count - 1)
                {
                    m_entries[i].Initialize(list.data[i]);
                    m_entries[i].gameObject.SetActive(true);
                } else
                {
                    m_entries[i].gameObject.SetActive(false);
                }
            }

            // When you move forward, enable going back
            if (list.page > 0)
            {
                // if this is the last page close going forward
                m_nextButton.interactable = !list.is_last;
                m_previousButton.interactable = true; 
            } else // If the first page close going backwards
            {
                // There is the possibility of only one page
                m_nextButton.interactable = !list.is_last;
                m_previousButton.interactable = false;
            }

            m_pageText.text = list.page.ToString();
        }

        private void ResetUI()
        {
            m_throbber.gameObject.SetActive(false);
            // Activate each entry so we can make sure each entry is open
            foreach (LeaderboardEntry entry in m_entries)
            {
                entry.gameObject.SetActive(false);
            }

            m_failedText.SetActive(false);
        }

        public void OpenLoadingScreen()
        {
            ResetUI();
            m_throbber.gameObject.SetActive(true);

            m_nextButton.interactable = false;
            m_previousButton.interactable = false;
        }

        public void CloseLoadingScreen(bool success, LeaderboardList list)
        {
            ResetUI();
            if (success)
            {
                DrawUI(list);
            } else
            {
                m_failedText.SetActive(true);
            }
        }

        public void OpenUI()
        {
            RectTransform canvasRect = m_lbCanvas.GetComponent<RectTransform>();
            RectTransform parentRect = m_lbCanvas.rootCanvas.GetComponent<RectTransform>();

            float height = parentRect.rect.height;
            float margin = 50f;

            Vector2 onScreenPos = canvasRect.anchoredPosition;
            Vector2 offScreenPos = parentRect.anchoredPosition + Vector2.up * (height + margin);

            canvasRect.anchoredPosition = offScreenPos;
            m_lbCanvas.enabled = true;
            
            canvasRect.DOAnchorPos(onScreenPos, 0.5f)
                    .SetEase(Ease.OutCubic);
        }

        public void CloseUI()
        {
            RectTransform canvasRect = m_lbCanvas.GetComponent<RectTransform>();
            RectTransform parentRect = m_lbCanvas.rootCanvas.GetComponent<RectTransform>();

            float height = parentRect.rect.height;
            float margin = 50f;

            Vector2 onScreenPos = canvasRect.anchoredPosition;
            Vector2 offScreenPos = parentRect.anchoredPosition + Vector2.up * (height + margin);

            canvasRect.DOAnchorPos(offScreenPos, 0.5f)
                    .SetEase(Ease.OutCubic)
                    .OnComplete(() =>
                    {
                        m_lbCanvas.enabled = false;
                        canvasRect.anchoredPosition = onScreenPos;
                    });
        }
    }
}