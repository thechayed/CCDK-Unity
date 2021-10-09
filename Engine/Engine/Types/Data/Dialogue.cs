using System.Collections;
using UnityEngine;

namespace CCDKEngine
{
    [System.Serializable]
    public class Dialogue
    {
        [Tooltip("The portrait of the speaker of this Dialogue.")]
        public Sprite portrait;
        [Tooltip("The name of the speaker of this Dialogue.")]
        public string name;
        [Tooltip("All the sentences this speaker says.")]
        [TextArea(3,10)]
        public string[] sentences;
    }

    [System.Serializable]
    public class DialogueList
    {
        [Tooltip("Dialogues, groups of sentences usually pertaining to one speaker.")]
        public Dialogue[] dialogues;

        public void StartConversation()
        {
            DialogueManager.StartConversation(dialogues);
        }
    }
}