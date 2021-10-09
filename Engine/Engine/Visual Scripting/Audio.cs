using CCDKEngine;
using System.Collections;
using UnityEngine;

namespace CCDKVisualScripting
{
    public class Audio : MonoBehaviour
    {
        public static void PlayLoop(string mixerGroup,AudioClip clip)
        {
            AudioManager.PlayLoop(mixerGroup, clip);
        }
    }
}