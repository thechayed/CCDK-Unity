using System.Collections;
using UnityEngine;
using System.Collections.Generic;
using CCDKEngine;

namespace CCDKObjects
{
    [CreateAssetMenu(menuName = "HUD/HeadsUpDisplay")]
    public class HUD : ScriptableObject
    {
        public string name;
        public Script.HUDClass HUDClass;
    }
}