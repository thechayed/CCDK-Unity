﻿/** The Sound Cue class provided information for how an Audio Node should play sound clips **/
using System.Collections;
using UnityEngine;

namespace CCDKEngine
{
    public class SoundCue : MonoBehaviour
    {
        /** All the Audio Clips that this Sound Cue can play **/
        public AudioClipDictionary AudioClipDictionary;

        public SoundCue(AudioClipDictionary clips)
        {
            AudioClipDictionary = clips;
        }
    }
}