using System.Collections;
using UnityEngine;
using CCDKGame;
using UnityEngine.Audio;

namespace CCDKEngine
{
    public class AudioMaster : MonoBehaviour
    {
        public string group;
        public AudioMasterData data;
        public AudioSource audioSource;

        public float toVol = 0f;
        public bool fade = false;
        public float fadeSpeed = 0.1f;

        public void Init(AudioMixerGroup group)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.outputAudioMixerGroup = group;
        }

        private void Update()
        {
            if (fade)
            {
                audioSource.volume = Mathf.Lerp(audioSource.volume, toVol, fadeSpeed);
            }
        }
    }
}