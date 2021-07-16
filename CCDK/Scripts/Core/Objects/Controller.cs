using System.Collections;
using UnityEngine;
using System.Collections.Generic;
using CCDKEngine;

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
        public StringsDictionary classes =
            new StringsDictionary
            (
                    new List<StringsDictionaryItem>
                    {
                    new StringsDictionaryItem("cameraClass", "Camera"),
                    new StringsDictionaryItem("inputClass", "PlayerInput"),
                    }
            );

        [System.Serializable]
        public class Input : StateEnabledObject
        {
        }
        public Input inputInfo;
    }
}