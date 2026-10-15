using TriviaGame.Settings;
using TriviaGame.Sounds;
using UnityEngine;
using UnityEngine.UI;

namespace TriviaGame.FSM
{
    public class SettingsState : MonoBehaviour, IState
    {
        [SerializeField, Range(0,1)] private float m_soundLevel;
        [SerializeField] private SettingsUIBuilder m_settingsUIBuilder;
        [SerializeField] private SettingsSaveHandler m_settingsSaveHandler;
        [SerializeField] private SoundManager m_soundManager;
        public void Enter()
        {
            if(m_settingsUIBuilder == null)
                m_settingsUIBuilder = FindAnyObjectByType<SettingsUIBuilder>();
            if (m_settingsSaveHandler == null) 
                m_settingsSaveHandler = new SettingsSaveHandler();
            if (m_soundManager == null) 
                m_soundManager = FindAnyObjectByType<SoundManager>();

            m_settingsUIBuilder.Initialize();
            m_soundLevel = m_settingsSaveHandler.GetSoundLevel();
            m_soundManager.SoundLevel = m_soundLevel;
        }

        public void SetSoundLevel(Slider soundSlider)
        {
            m_soundLevel = soundSlider.value;
            m_settingsSaveHandler.SaveSoundLevel(m_soundLevel);
            m_soundManager.SoundLevel = m_soundLevel;
        }

        public void Exit()
        {
            m_settingsUIBuilder.ResetUI();
            m_settingsSaveHandler.SaveSoundLevel(m_soundLevel);
        }
    }
}