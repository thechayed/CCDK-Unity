using System.Collections;
using UnityEngine;
using CCDKGame;
using UnityEngine.Audio;
using System.Collections.Generic;

namespace CCDKEngine
{
    public class AudioManager
    {
        public static CCDKObjects.Audio data;
        public static AudioNode sources;
        public static AudioMixer mixer;
        public static AudioMixerGroup[] groups;
        public static List<AudioMaster> audioMasters = new List<AudioMaster>();

        /**Create Audio Masters for every Mixer Group by getting the Game's Mixer from the Engine**/
        public static void Init(CCDKObjects.Audio data,AudioMixer mixer)
        {
            AudioManager.data = data;

            groups = mixer.FindMatchingGroups("");
            
            foreach (AudioMixerGroup mixerGroup in groups)
            {
                LevelManager.ManageEngine();

                GameObject newMaster = new GameObject();
                newMaster.name = "AudioMaster_" + mixerGroup.name;
                audioMasters.Add(newMaster.AddComponent<AudioMaster>());
                audioMasters[audioMasters.Count-1].group = mixerGroup.name;
                audioMasters[audioMasters.Count - 1].Init(mixerGroup);

                LevelManager.ReturnToGameplay();
            }
        }

        public static bool IsPlaying(string name)
        {
            return GetMaster(name).audioSource.isPlaying;
        }

        /**<summary>Play a sound once.</summary>**/
        public static void PlayOneShot(string name, AudioClip clip)
        {
            AudioMaster audioMaster = GetMaster(name);
            audioMaster.audioSource.PlayOneShot(clip);
        }

        /**<summary>Play and loop a sound.</summary>**/
        public static void PlayLoop(string name, AudioClip clip)
        {
            AudioMaster audioMaster = GetMaster(name);
            audioMaster.audioSource.Stop();
            audioMaster.audioSource.loop = true;
            audioMaster.audioSource.clip = clip;
            audioMaster.audioSource.Play();
        }

        /**<summary>Get an Audio Master by it's group name.</summary>**/
        public static AudioMaster GetMaster(string name)
        {
            foreach(AudioMaster master in audioMasters)
            {
                if(master.group == name)
                {
                    return master;
                }
            }
            return null;
        }

        /**<summary>Set the volume of an Audio Master.</summary>**/
        public static void SetVolume(string name, float volume)
        {
            AudioMaster audioMaster = GetMaster(name);
            audioMaster.audioSource.volume = volume;
        }

        /**<summary>Fade the volume of an Audio Master.</summary>**/
        public static void FadeVolume(string name, float volume, float fadeSpeed = 0.1f)
        {
            AudioMaster audioMaster = GetMaster(name);
            audioMaster.toVol = volume;
            audioMaster.fade = true;
            audioMaster.fadeSpeed = fadeSpeed;
        }
    }
}