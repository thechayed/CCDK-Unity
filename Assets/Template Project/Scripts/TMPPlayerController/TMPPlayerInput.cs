using CCDKGame;
using System.Collections;
using UnityEngine;

namespace TemplateGame
{
    public class TMPPlayerInput : PlayerInput
    {
        /** The Script of actions we're using **/
        TemplateControls inputActions;
        
        /** Whether we've translated Pawn methods to input methods yet **/
        bool hasCommandedPawn;

        public override void Start()
        {
            inputActions = new TemplateControls();
            controller.Possessed += OnPossess;
            base.Start();
        }

        /** When a Pawn has been possessed, loop through all the Items in the Controller's
         * IO Dictionary to find what methods should be called for each Input, 
         * and assign the Inputs accordingly within the Pawn's Input Handler **/
        public void OnPossess()
        {
            int index = 0;
            foreach(DictionaryItem<string> item in controller.data.inputInfo.InputOutput.dictionary)
            {
                if(item.key == inputActions.Newactionmap.Get().actions[index].name)
                {
                    if (pawn.GetComponent<PawnInputHandler>().GetType().GetMethod(item.value) != null)
                    {
                        inputActions.Newactionmap.Get().actions[index].performed += ctx => pawn.GetComponent<PawnInputHandler>().GetType().GetMethod(item.value).Invoke(pawn.GetComponent<PawnInputHandler>(), null);

                        if (pawn.GetComponent<PawnInputHandler>().GetType().GetMethod(item.value+"_Cancel") != null)
                            inputActions.Newactionmap.Get().actions[index].canceled += ctx => pawn.GetComponent<PawnInputHandler>().GetType().GetMethod(item.value+"_Cancel").Invoke(pawn.GetComponent<PawnInputHandler>(), null);
                    }
                    else
                    {
                        Debug.LogWarning("The requested Method: " + item.value + " does not exist in the class this Controller Possesses.");
                    }
                }
                index++;
            }
       }

        public class Default : FSM.State
        {
            TMPPlayerInput self;

            public override void Enter()
            {
                self = (TMPPlayerInput)selfObj;
                self.inputActions.Enable();
            }

            public override void Update()
            {

            }
        }
    }
}