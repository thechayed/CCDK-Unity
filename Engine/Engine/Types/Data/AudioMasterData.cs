using System.Collections;
using UnityEngine;

namespace CCDKEngine
{
    [System.Serializable]
    public class AudioMasterData 
    {
        public bool spatial = false;
        public bool Loop = false;
        public float Volume = 0.0f;

        public bool fadeOnClipChange = true;
        public float fadeSpeed = 0.1f;

        public AudioSource audioSource;
    }
}