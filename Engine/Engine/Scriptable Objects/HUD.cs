using System.Collections;
using UnityEngine;
using System.Collections.Generic;
using CCDKEngine;
using UnityEngine.SceneManagement;
using B83.Unity.Attributes;

namespace CCDKObjects
{
    [CreateAssetMenu(menuName = "HUD/Heads Up Display")]
    public class HUD : ScriptableObject
    {
        /** The name of this HUD **/
        public string name;
        /** The scene that should be created/destroyed with this HUD object **/
        [Scene]
        public string scene;
        /** The Extended HUD class for interfacing with the HUD scene **/
        [MonoScript(type = typeof(CCDKGame.HUD))] public string script;


        [Header("Default Values")]
        [Tooltip("Default font to use when drawing text")]
        public Font defaultFont;

        [Header("Elements")]
        /**<summary>The canvas this HUD is using to draw.</summary>**/
        [Tooltip("The canvas this HUD is using to draw.")]
        public Canvas canvas;
        /**<summary>The GameObject of the Canvas</summary>**/
        [Tooltip("The GameObject of said Canvas")]
        [ReadOnly] public GameObject canvasGameObject;


        /**List of scale modes for drawing**/
        public enum scaleModes
        {
            Pixel,
            Physical,
            Screen
        };
        [Header("Sizing")]
        /**<summary>What Scale mode to use for this HUD's canvas</summary>**/
        [Tooltip("What Scale mode to use for this HUD's canvas")]
        public scaleModes scaleMode;
        /**<summary>Scale the canvas in with the cinematic black bars. Default behavior.</summary>**/
        [Tooltip("Scale the canvas in with the cinematic black bars. Default behavior.")]
        public bool scaleCanvasForCinematicMode;
    }
}