using System.Collections;
using UnityEngine;
using CCDKEngine;
using CCDKGame;
using CCDKObjects;

namespace CCDKVisualScripting
{
    public class HUD : MonoBehaviour
    {
        /** Tell the game to Display a Conversation using the Conversation Scriptable Object **/
        public static void StartConversation(Conversation conversation)
        {
            conversation.conversation.StartConversation();
        }

        public static void EndConversation()
        {
            DialogueManager.EndDialogue();
        }
    }
}