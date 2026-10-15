using UnityEngine;

namespace TriviaGame.Sounds
{
    public class SoundManager : MonoBehaviour
    {
        private static SoundManager s_instance;

        [Header("Audio Settings")]
        [SerializeField] private float m_soundLevel;
        public float SoundLevel
        { 
            get => m_soundLevel;
            set
            {
                m_soundLevel = Mathf.Clamp(value, 0, 1);
                m_buttonClickSound.volume = SoundLevel - 0.3f;
                m_bgSound.volume = SoundLevel;
                m_successSFX.volume = SoundLevel;
                m_failedSFX.volume = SoundLevel;
            }
        }
        [SerializeField] private float m_pitch;

        [Header("Audio Sources")]
        [SerializeField] private AudioSource m_buttonClickSound;
        [SerializeField] private AudioSource m_bgSound;
        [SerializeField] private AudioSource m_successSFX;
        [SerializeField] private AudioSource m_failedSFX;
        
        void Awake()
        {
            if (s_instance != null && s_instance != this)
            {
                Destroy(gameObject);
                return;
            }

            s_instance = this;
            DontDestroyOnLoad(gameObject);
        }

        public void PlayButtonClickSound()
        {
            PlaySound(m_buttonClickSound);
        }

        public void PlaySuccessSound()
        {
            PlaySound(m_successSFX);
        }

        public void PlayFailSound()
        {
            PlaySound(m_failedSFX);
        }

        private void PlaySound(AudioSource source)
        {
            // Pitch delta allows us to add a bit of variety to the sound every time it is played.
            float m_pitchDelta = Random.Range(-0.5f, 0.5f);
            float pitch = m_pitch + m_pitchDelta;

            // Reset the SFX
            source.Stop();
            source.time = 0;

            source.pitch = pitch;

            // Play the SFX
            source.Play();
        }
    }
}