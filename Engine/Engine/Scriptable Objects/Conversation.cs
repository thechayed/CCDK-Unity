using System.Collections;
using UnityEngine;
using System.Collections.Generic;
using CCDKEngine;

namespace CCDKObjects
{
    [CreateAssetMenu(menuName = "HUD/Conversation")]
    public class Conversation : ScriptableObject
    {
        public DialogueList conversation;
    }
}