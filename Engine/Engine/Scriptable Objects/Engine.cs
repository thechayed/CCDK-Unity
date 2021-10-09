using CCDKEngine;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Audio;

namespace CCDKObjects
{
    public class Engine : ScriptableObject, ISerializationCallbackReceiver
    {
        [Tooltip("Optional graph controlling the Runtime")]
        public StateGraphAsset runtimeGraph;

        [Tooltip("Whether this Engine object has been loaded.")]
        [ReadOnly] public bool loaded = false;
        [Header(" - Levels - ")]
        [Tooltip("The name of the first level to load.")]
        public string startingLevelName = "";
        [Tooltip("When in the Editor, whether we should use the Scene that is open in the Scene View as the starting Level.")]
        public bool useOpenScene = true;
        [Tooltip("Whether the Starting Level has been loaded.")]
        [ReadOnly] public bool startingLevelLoaded = false;
        [Tooltip("Whether to destroy that last Level when a new one is loaded.")]
        [HideInInspector] public bool dropLevelOnLoad;


        [Header(" - Game Type - ")]
        public bool useGameTypes = true;

        [Header("Engine Defaults")]

        public CCDKObjects.Controller defaultPlayerController;
        public CCDKObjects.Pawn defaultPawn;


        [HideInInspector]
        [SerializeField]public static Engine engineObj;


        [Header("  - Audio - ")]
        public AudioMixer audioMixer;
        public Audio audioData;

        /** Initialize **/
        void OnEnable()
        {
            loaded = false;
        }

        public void OnBeforeSerialize()
        {
            engineObj = this;
        }

        public void OnAfterDeserialize()
        {
            engineObj = this;
        }

        private void OnValidate()
        {
            if (!Application.isPlaying)
            {
                startingLevelLoaded = false;
            }
        }

        ///** Tell the Engine to load it's Data from this object when Runtime starts **/
        //[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        //public static void load()
        //{
        //    if(CCDKEngine.Engine.data != null)
        //    {
        //        Debug.Log("Engine: Only one Engine SCriptable Object can be used in the project, this one has been ignored!", Engine.engineObj);
        //        return;
        //    }
        //    if (Engine.engineObj != null)
        //    {
        //        CCDKEngine.Engine.data = Engine.engineObj;
        //        CCDKEngine.Engine.data.loaded = true;
        //        CCDKEngine.Engine.data.startingLevelLoaded = false;
        //    }
        //}
    }
}