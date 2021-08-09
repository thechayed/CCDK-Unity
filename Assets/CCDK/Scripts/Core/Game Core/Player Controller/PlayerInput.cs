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
        public object pawnInputHandler;

        /** Input Action Collection script. Input actions are used in the Player Controller to get control from the Player **/
        public object inputActions;
        public Type inputActionsType;

        /** Action Map Values **/
        object actionMap;
        MethodInfo get;
        InputActionMap map;
        ReadOnlyArray<InputAction> actions;

        /** Whether we've translated Pawn methods to input methods yet **/
        bool hasCommandedPawn;

        public override void Start()
        {
            inputActionsType = Type.GetType(controller.data.inputInfo.inputActions);
            inputActions = Activator.CreateInstance(inputActionsType);

            controller.Possessed += OnPossess;
            base.Start();
        }


        public void OnPossess()
        {
            pawnInputHandler = pawn.GetComponent<PawnInputHandler>();
            SetupInput();

            foreach (InputAction action in actions)
            {
                action.performed += ctx => pawnInputHandler.GetType().GetMethod("InputPerformed").Invoke(pawnInputHandler, new object[] { ctx });
                action.canceled += ctx => pawnInputHandler.GetType().GetMethod("InputCanceled").Invoke(pawnInputHandler, new object[] { ctx });
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

        public void SetupInput()
        {
            //actionMap = Activator.CreateInstance(inputActions.GetType().GetNestedType(controller.data.inputInfo.actionMap));
            actionMap = inputActions.GetType().GetProperty(controller.data.inputInfo.actionMap).GetValue(inputActions);
            get = actionMap.GetType().GetMethod("Get");
            map = (InputActionMap)get.Invoke(actionMap, null);
            actions = (ReadOnlyArray<InputAction>)map.GetType().GetProperty("actions").GetValue(map);
            inputActions.GetType().GetMethod("Enable").Invoke(inputActions,null);
        }
    }
}
