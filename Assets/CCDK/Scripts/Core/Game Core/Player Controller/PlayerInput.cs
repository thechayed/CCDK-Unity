using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CCDKEngine;
using UnityEngine.InputSystem;
using TemplateGame;
using System;
using System.Reflection;
using UnityEngine.InputSystem.Utilities;

namespace CCDKGame
{
    public class PlayerInput : ControllerInput
    {

        /** Input Action Collection script. Input actions are used in the Player Controller to get control from the Player **/
        public object inputActions;
        public Type inputActionsType;

        public InputAction[] inputActionValues;

        /** Whether we've translated Pawn methods to input methods yet **/
        bool hasCommandedPawn;

        public override void Start()
        {
            if (Type.GetType(controller.data.inputInfo.inputActions).BaseType.Name == "IInputActionCollection")
            {
                inputActionsType = Type.GetType(controller.data.inputInfo.inputActions);
                inputActions = Activator.CreateInstance(inputActionsType);
            }

            controller.Possessed += OnPossess;
            base.Start();
        }

        /** When a Pawn has been possessed, loop through all the Items in the Controller's
         * IO Dictionary to find what methods should be called for each Input, 
         * and assign the Inputs accordingly within the Pawn's Input Handler **/
        public void OnPossess()
        {
            int index = 0;
            inputActionValues = GetActions();

            foreach (DictionaryItem<string> item in controller.data.inputInfo.InputOutput.dictionary)
            {
                if (item.key == inputActionValues[index].name)
                {
                    if (pawn.GetComponent<PawnInputHandler>().GetType().GetMethod(item.value) != null)
                    {
                        SetAction(index, pawn, item.value);
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
            PlayerInput self;

            public override void Enter()
            {
                self = (PlayerInput)selfObj;
                self.CallInputMethod("Enable");
            }

            public override void Update()
            {

            }
        }

        public void CallInputMethod(string name, object[] parameters = null)
        {
            inputActionsType.GetMethod(name).Invoke(inputActions, parameters);
        }

        public InputAction[] GetActions()
        {
            Type actionMap = inputActionsType.GetNestedType(controller.data.inputInfo.actionMap);
            MethodInfo get = actionMap.GetMethod("Get");
            InputActionMap map = (InputActionMap)get.Invoke(actionMap, null);
            ReadOnlyArray<InputAction> actions = (ReadOnlyArray<InputAction>)map.GetType().GetField("actions").GetValue(map);
            InputAction[] arr = (InputAction[]) actions.GetType().GetMethod("ToArray").Invoke(actions, null);
            return arr;
        }

        public void SetAction(int index, Pawn pawn, string Action)
        {
            Type actionMap = inputActionsType.GetNestedType(controller.data.inputInfo.actionMap);
            MethodInfo get = actionMap.GetMethod("Get");
            InputActionMap map = (InputActionMap)get.Invoke(actionMap, null);
            ReadOnlyArray<InputAction> actions = (ReadOnlyArray<InputAction>)map.GetType().GetField("actions").GetValue(map);

            actions[index].performed += ctx => pawn.GetComponent<PawnInputHandler>().GetType().GetMethod(controller.data.inputInfo.InputOutput.Get(Action)).Invoke(pawn.GetComponent<PawnInputHandler>(), null);
            if (pawn.GetComponent<PawnInputHandler>().GetType().GetMethod(Action + "_Cancel") != null)
                actions[index].canceled += ctx => pawn.GetComponent<PawnInputHandler>().GetType().GetMethod(controller.data.inputInfo.InputOutput.Get(Action) + "_Cancel").Invoke(pawn.GetComponent<PawnInputHandler>(), null);
        }

    }
}
