using System;
using UnityEngine;

namespace TriviaGame.Settings
{
    [Serializable]
    public class SettingsSaveHandler
    {
        private string m_soundLevelSaveKey = "SoundLevelSaveKey";

        public SettingsSaveHandler(string newSoundLevelKey)
        {

        }

        public SettingsSaveHandler()
        {

        }

        public void SaveSoundLevel (float value)
        {
            PlayerPrefs.SetFloat(m_soundLevelSaveKey, value);
        }
        
        public float GetSoundLevel()
        {
            if (!PlayerPrefs.HasKey(m_soundLevelSaveKey)) PlayerPrefs.SetFloat(m_soundLevelSaveKey, 1);

            return PlayerPrefs.GetFloat(m_soundLevelSaveKey);
        }
    }
}