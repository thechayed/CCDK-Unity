using System.Collections;
using UnityEngine;
using System.Collections.Generic;
using CCDKEngine;
using UnityEngine.InputSystem;
using System;
using UnityEditor;
using B83.Unity.Attributes;

namespace CCDKObjects
{
    [CreateAssetMenu(menuName = "Engine/Controller")]
    public class Controller : ScriptableObject
    {

        /** A dictionary storing all the Class names to add to the pawn on Construction **/
        public Dictionary<Script.ControllerClass> classes =
            new Dictionary<Script.ControllerClass>
            (
                    new List<DictionaryItem<Script.ControllerClass>>
                    {
                    new DictionaryItem<Script.ControllerClass>("inputClass", new Script.ControllerClass{script="CCDKGame.PlayerInput"}),
                    new DictionaryItem<Script.ControllerClass>("unitClass",new Script.ControllerClass{script="CCDKGame.AINEATUnit"})
                    }
            );

        [System.Serializable]
        public class Input 
        {
            /** Key = Input Name, Value = Method Name **/
            public Dictionary<string> InputOutput = new Dictionary<string>();
            /** Input Action Script **/
            [MonoScript(type=typeof(IInputActionCollection))] public string inputActions;
            public string actionMap;
        }
        public Input inputInfo;
    }
}