using System.Collections;
using UnityEngine;

namespace CCDKEngine
{
    [System.Serializable]
    public class Dialogue 
    {
        public Sprite portrait;
        public string name;
        [TextArea(3,10)]
        public string[] sentences;
    }

    [System.Serializable]
    public class DialogueList
    {
        public Dialogue[] dialogues;

        public void StartConversation()
        {
            DialogueManager.StartConversation(dialogues);
        }
    }
}