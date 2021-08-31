using CCDKEngine;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

namespace CCDKObjects
{
    [CreateAssetMenu(menuName = "Engine/Engine Core")]
    public class Engine : ScriptableObject, ISerializationCallbackReceiver
    {
        [Tooltip("Optional graph controlling the Runtime")]
        public StateGraphAsset runtimeGraph;

        [Tooltip("Whether this Engine object has been loaded.")]
        [ReadOnly] public bool loaded = false;
        [Header("Levels")]
        [Tooltip("The name of the first level to load.")]
        public string startingLevelName = "";
        [Tooltip("Whether the Starting Level has been loaded.")]
        [ReadOnly] public bool startingLevelLoaded = false;
        [Tooltip("Whether to destroy that last Level when a new one is loaded.")]
        public bool dropLevelOnLoad;

        [Header("Gameplay")]
        [Tooltip("The Game Mode object that is used when Runtime starts.")]
        [SerializeField] private GameMode startingGameMode;
        [Tooltip("The current Game Mode object.")]
        [ReadOnly] public GameMode gameMode;

        [Tooltip("The Game Info object that is used when Runtime starts.")]
        [SerializeField] private GameInfo startingGameInfo;
        [Tooltip("The current Game Info object.")]
        [ReadOnly] public GameInfo gameInfo;

        [SerializeField]public static ScriptableObject engineObj;


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

        /** Tell the Engine to load it's Data from this object when Runtime starts **/
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        public static void load()
        {
            if(CCDKEngine.Engine.data != null)
            {
                Debug.Log("Engine: Only one Engine SCriptable Object can be used in the project, this one has been ignored!", Engine.engineObj);
                return;
            }
            CCDKEngine.Engine.data = (Engine) Engine.engineObj;
            CCDKEngine.Engine.data.loaded = true;
            CCDKEngine.Engine.data.startingLevelLoaded = false;
        }
    }
}