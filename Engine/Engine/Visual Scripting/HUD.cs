using System.Collections;
using UnityEngine;
using CCDKEngine;
using CCDKGame;
using CCDKObjects;

namespace CCDKVisualScripting
{
    public class HUD : MonoBehaviour
    {
        /**<summary>Create and return a new HUD Game Object created from a HUD Scriptable Object.</summary>**/
        public static CCDKGame.HUD AddHUD(CCDKObjects.HUD hud)
        {
            LevelManager.ManageEngine();
            GameObject newHUDObj = new GameObject();
            CCDKGame.HUD newHUD = newHUDObj.AddComponent<CCDKGame.HUD>();
            newHUDObj.name = "HUD_"+hud.name;
            newHUD.data = hud;
            return newHUD;
        }

        /**<summary>Tell the game to Display a Conversation using the Conversation Scriptable Object </summary> **/
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