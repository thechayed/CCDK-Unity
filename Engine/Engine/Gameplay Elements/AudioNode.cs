/* An Audio Node is given a Sound Cue and uses it's defined behavior to play sounds through the Audio Source */

using System.Collections;
using System;
using UnityEngine;
using UnityEditor;
using CCDKEngine;

namespace CCDKGame
{
    public class AudioNode : CCDKEngine.Object
    {
        public AudioSource audioSource;
        public Dictionary<AudioClip> audioClips;
        [SerializeField]
        public SoundCue soundCue;


        // Use this for initialization
        public void Start()
        {
            audioSource = GetComponent<AudioSource>();
        }
    }
}