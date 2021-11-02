using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace CCDKObjects
{
    [CreateAssetMenu(menuName = "Game Info/Level Info")]
    public class Level : ScriptableObject
    {
        public string levelName;

        [Header(" - Game Info - ")]
        [Tooltip("A List of all Game Types that can be played on this level.")]
        public List<GameTypeInfo> compatibleGameTypes;
    }
}