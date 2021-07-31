using CCDKGame;
using System.Collections;
using UnityEngine;

namespace TemplateGame
{
    public class TMPPlayerInput : PlayerInput
    {
        /** The Script of actions we're using **/
        //TemplatePCControls inputActions;
        
        /** Whether we've translated Pawn methods to input methods yet **/
        bool hasCommandedPawn;

        public override void Start()
        {
            //inputActions = new TemplatePCControls();
            controller.Possessed += OnPossess;
            base.Start();
        }

        /** When a Pawn has been possessed, loop through all the Items in the Controller's
         * IO Dictionary to find what methods should be called for each Input, 
         * and assign the Inputs accordingly within the Pawn's Input Handler **/
        public void OnPossess()
        {
            int index = 0;
            //foreach(DictionaryItem<string> item in controller.data.inputInfo.InputOutput.dictionary)
            //{
            //    if(item.key == inputActions.Newactionmap.Get().actions[index].name)
            //    {
            //        inputActions.Newactionmap.Get().actions[index].performed += ctx => controller.possessedPawn.GetComponent<PawnInputHandler>().GetType().GetMethod(item.value);
            //    }
            //    index++;
            //}
       }
    }
}