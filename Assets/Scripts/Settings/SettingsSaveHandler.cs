using System;
using UnityEngine;

namespace TriviaGame.Settings
{
    [Serializable]
    public class SettingsSaveHandler
    {
        private string m_soundLevelSaveKey = "SoundLevelSaveKey";

        public SettingsSaveHandler()
        {}

        /// <summary>
        /// Saves the given sound level value.
        /// </summary>
        /// <param name="value"></param>
        public void SaveSoundLevel (float value)
        {
            PlayerPrefs.SetFloat(m_soundLevelSaveKey, value);
            PlayerPrefs.Save();
        }

        /// <summary>
        /// Returns the saved sound level.
        /// If sound level hasn't been saved yet, it is set to 1.
        /// </summary>
        /// <returns></returns>
        public float GetSoundLevel()
        {
            if (!PlayerPrefs.HasKey(m_soundLevelSaveKey)) PlayerPrefs.SetFloat(m_soundLevelSaveKey, 1);

            return PlayerPrefs.GetFloat(m_soundLevelSaveKey);
        }
    }
}