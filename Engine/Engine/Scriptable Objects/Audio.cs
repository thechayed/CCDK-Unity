using UnityEditor;
using UnityEngine;

namespace CCDKObjects
{
    [CreateAssetMenu(menuName = "Editor/Audio")]
    public class Audio : ScriptableObject
    {
        public AudioClip beep;

        public Dictionary<AudioClip> Music = new Dictionary<AudioClip>();
    }
}