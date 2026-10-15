using System.Collections;
using UnityEditorInternal.Profiling.Memory.Experimental.FileFormat;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;

namespace TriviaGame.Leaderboard
{
    public class LeaderboardGetter : MonoBehaviour
    {
        private LeaderboardList m_lbList;
        [SerializeField] private const string baseUrl = "https://localhost:8080/leaderboard?page=";
        [SerializeField] private const string fallbackUrl = "https://magegamessite.web.app/case1/leaderboard_page_";
        [SerializeField, Min(1)] private int m_timeOut;
        private Coroutine m_webrequestRoutine;

        public UnityEvent OnRequestStart = new UnityEvent();
        public UnityEvent<bool, LeaderboardList> OnRequestEnd = new UnityEvent<bool, LeaderboardList>();

        public void GetLeaderboardEntries(int page)
        {
            // If routine is not null, there is already a call.
            if(m_webrequestRoutine == null ) m_webrequestRoutine = StartCoroutine(WebRequestCoroutine(baseUrl, page));
        }

        private string GetFallbackUrl(int page)
        {
            return fallbackUrl + page + ".json";
        }

        IEnumerator WebRequestCoroutine(string uri, int page)
        {
            OnRequestStart.Invoke();
            Debug.Log(uri + page);
            using (UnityWebRequest webRequest = UnityWebRequest.Get(uri + page))
            {
                webRequest.timeout = m_timeOut;

                // Request and wait for the desired page
                yield return webRequest.SendWebRequest();

                switch(webRequest.result)
                {
                    case UnityWebRequest.Result.ConnectionError:
                        Debug.LogError($"Base URL has thrown an Error: {webRequest.error}");
                        m_webrequestRoutine = StartCoroutine(FallbackWebRequestCoroutine(page));
                    break;
                    case UnityWebRequest.Result.DataProcessingError:
                        Debug.LogError($"Base URL has thrown an Error: {webRequest.error}");
                        m_webrequestRoutine = StartCoroutine(FallbackWebRequestCoroutine(page));
                    break;
                    case UnityWebRequest.Result.ProtocolError:
                        Debug.LogError($"Base URL has thrown an Error: {webRequest.error}");
                        m_webrequestRoutine = StartCoroutine(FallbackWebRequestCoroutine(page));
                    break;
                    case UnityWebRequest.Result.Success:
                        m_lbList = FileIOHelper.GetData<LeaderboardList>(webRequest.downloadHandler.text);
                        m_webrequestRoutine = null;
                        OnRequestEnd.Invoke(true, m_lbList);
                    break;
                }
            }
        }

        IEnumerator FallbackWebRequestCoroutine(int page)
        {
            using (UnityWebRequest webRequest = UnityWebRequest.Get(GetFallbackUrl(page)))
            {
                webRequest.timeout = m_timeOut;

                // Request and wait for the desired page
                yield return webRequest.SendWebRequest();

                switch(webRequest.result)
                {
                    case UnityWebRequest.Result.ConnectionError:
                        Debug.LogError($"Fallback URL has thrown an Error: {webRequest.error}");
                        m_webrequestRoutine = null;
                        OnRequestEnd.Invoke(false, null);
                    break;
                    case UnityWebRequest.Result.DataProcessingError:
                        Debug.LogError($"Fallback URL has thrown an Error: {webRequest.error}");
                        m_webrequestRoutine = null;
                        OnRequestEnd.Invoke(false, null);
                    break;
                    case UnityWebRequest.Result.ProtocolError:
                        Debug.LogError($"Fallback URL has thrown an Error: {webRequest.error}");
                        m_webrequestRoutine = null;
                        OnRequestEnd.Invoke(false, null);
                    break;
                    case UnityWebRequest.Result.Success:
                        Debug.Log($"The text from webRequest:\n{webRequest.downloadHandler.text}");
                        m_lbList = FileIOHelper.GetData<LeaderboardList>(webRequest.downloadHandler.text);
                        foreach (LeaderboardEntryData entry in m_lbList.data) Debug.Log(entry.ToString());
                        m_webrequestRoutine = null;
                        OnRequestEnd.Invoke(true, m_lbList);
                    break;
                }
            }
        }
    }
}