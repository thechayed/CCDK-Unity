using System.Collections;
using UnityEngine;
using System.Collections.Generic;
using CCDKEngine;
using UnityEngine.InputSystem;
using System;
using UnityEditor;
using B83.Unity.Attributes;
using CCDKGame;
using System.Threading.Tasks;

namespace CCDKObjects
{
    [CreateAssetMenu(menuName = "Game Objects/Controller")]
    public class Controller : PrefabSO
    {
        [MonoScript(type = typeof(CCDKEngine.Controller))] public string controllerClass = "CCDKGame.PlayerController";

        [Tooltip("While true, the Controller can only possess one Pawn at a time.")]
        public bool forceOnePawn = true;

        public enum ControllerType
        {
            Player,
            AI,
            Custom
        }
        [Tooltip("The kind of Controller this is, determines how the Engine uses it.\nPlayer: Given to players that join a game\nAI: Given to AI's that join the game\nCustom: The engine does nothing.")]
        public ControllerType controllerType;

        public InputActionAsset inputActionAsset;

        public bool mouseToNavMesh = false;

#if UNITY_EDITOR
        public override void OnEnable()
        {
            className = controllerClass;
            defaultObjectPrefab = Resources.Load<GameObject>("CCDK/PrefabDefaults/PlayerController");
            base.OnEnable();
        }

        public override void Awake()
        {
            className = controllerClass;
            defaultObjectPrefab = Resources.Load<GameObject>("CCDK/PrefabDefaults/PlayerController");
            base.Awake();
        }

        public override void OnValidate()
        {
            className = controllerClass;
            base.OnValidate();
        }
#endif
    }
}