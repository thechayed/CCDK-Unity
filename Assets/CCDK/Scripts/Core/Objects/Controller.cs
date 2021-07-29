using System.Collections;
using UnityEngine;
using System.Collections.Generic;
using CCDKEngine;
using UnityEngine.InputSystem;

namespace CCDKObjects
{
    [CreateAssetMenu(menuName = "Engine/Controller")]
    public class Controller : ScriptableObject
    {

        [System.Serializable]
        public class States : StateEnabledObject
        {

        }
        public States stateInfo;

        /** A dictionary storing all the Class names to add to the pawn on Construction **/
        public Dictionary<string> classes =
            new Dictionary<string>
            (
                    new List<DictionaryItem<string>>
                    {
                    new DictionaryItem<string>("cameraClass", "Camera"),
                    new DictionaryItem<string>("inputClass", "PlayerInput"),
                    new DictionaryItem<string>("unitClass","AINEATUnit")
                    }
            );

        [System.Serializable]
        public class Input : StateEnabledObject
        {
            /** Key = Input Name, Value = Method Name **/
            public Dictionary<string> InputOutput = new Dictionary<string>();
        }
        public Input inputInfo;
    }
}