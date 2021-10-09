using CCDKEngine;
using System.Collections;
using UnityEngine;

namespace CCDKEngine
{
    public class Scriptment : FSM.Component
    {
        /** All the Dialogues in the Scriptment **/
        public Dictionary<Dialogue> dialogues = new Dictionary<Dialogue>();
        /** All the variables in the Scriptment **/
        public Dictionary<object> variables = new Dictionary<object>();

        public bool switchState = false;
        public string stateToGoTo = "";
        
        public void AddDialogueList(Sprite newPortrait, string newName, string[] newSentences)
        {
            dialogues.Set(""+dialogues.length,new Dialogue() { portrait = newPortrait, name = newName, sentences = newSentences });
        }

        public override void FixedUpdate()
        {
            if (switchState)
            {
                GoToState(stateToGoTo);
                switchState = true;
            }

            base.FixedUpdate();
        }
    }
}