using UnityEditor;
using UnityEngine;

namespace CCDKObjects
{
    [CreateAssetMenu(menuName = "Editor/Audio")]
    public class Audio : ScriptableObject
    {
        public AudioClip beep;
    }
}