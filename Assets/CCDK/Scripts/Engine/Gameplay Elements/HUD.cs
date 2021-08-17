/*The HUD class contains base code for Heads Up Displays in game, extend it to make another! */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using CCDKEngine;

namespace CCDKGame
{
    public class HUD : PossessableObject
    {
        //Default values
        [System.Serializable]
        public struct DefaultValues
        {
            /**Default font to use when drawing text**/
            public Font defaultFont;
        }
        public DefaultValues hudDefaultValues;

        //Children
        [System.Serializable]
        public struct Children
        {
            /** The canvas this HUD is using to draw **/
            public Canvas canvas;
            /** The GameObject of said Canvas **/
            [HideInInspector] public GameObject canvasGameObject;
            /** Names of the objects added to the canvas **/
            [HideInInspector] public List<string> objects;
        }
        public Children children;

        //Sizing
        [System.Serializable]
        public struct Sizing
        {
            /**List of scale modes for drawing**/
            public enum scaleModes
            {
                Pixel,
                Physical,
                Screen
            };
            public scaleModes scaleMode;
            /**Specifies amount of screen-space to use (for TV's).**/
            public float hudCanvasScale;
            /** Use the full screen extents for the canvas. Ignores splitscreen and cinematic mode scaling. */
            public bool renderFullScreen;
            /** Scale the canvas in with the cinematic black bars. Default behavior. */
            public bool scaleCanvasForCinematicMode;
            /** Size of ViewPort in pixels */
            float SizeX, SizeY;
            /** Center of Viewport */
            float CenterX, CenterY;
            /** Ratio of viewport compared to native resolution 1024x768 */
            float RatioX, RatioY;
        }
        public Sizing sizing;

        //Rendering
        [System.Serializable]
        public struct Rendering
        {
            /** If true, render actor overlays */
            public bool bShowOverlays;
            /** Used to create DeltaTime */
            float LastHUDRenderTime;
            /** Time since last render */
            float RenderDelta;
        }
        public Rendering rendering;


        // Use this for initialization
        public override void Start()
        {
            base.Start();
            /** If the HUD does not have a Canvas to draw on set from the editor, create a new one **/
            if (children.canvas == null)
            {
                children.canvasGameObject = new GameObject();
                children.canvasGameObject.name = "HUDCanvas";
                children.canvasGameObject.AddComponent<Canvas>();
                children.canvas = children.canvasGameObject.GetComponent<Canvas>();
                children.canvasGameObject.AddComponent<CanvasScaler>();
                children.canvasGameObject.AddComponent<GraphicRaycaster>();
            }
            /** Otherwise, get the GO from the Canvas that has been given to us**/
            else
            {
                children.canvasGameObject = children.canvas.gameObject;
            }

            SetScaleMode((int)sizing.scaleMode);
        }

        // Update is called once per frame
        public void State_Default()
        {

        }

        public void DrawText(string name = "Text", string text = "", Font font = null, int fontSize = 16, float x = 0, float y = 0, float width = 100, float height = 16)
        {
            if (font == null)
            {
                font = hudDefaultValues.defaultFont;
            }

            GameObject myText;

            myText = new GameObject();
            myText.transform.parent = children.canvasGameObject.transform;
            myText.name = "wibble";
            children.objects.Add(name);

            Text textC = myText.AddComponent<Text>();
            textC.font = font;
            textC.text = text;
            textC.fontSize = fontSize;

            // Text position
            RectTransform rectTransform = textC.GetComponent<RectTransform>();
            rectTransform.localPosition = new Vector3(0, 0, 0);
            rectTransform.sizeDelta = new Vector2(400, 200);
        }

        public void SetScaleMode(int mode)
        {
            switch (mode)
            {
                case 0:
                    children.canvasGameObject.GetComponent<CanvasScaler>().uiScaleMode = UnityEngine.UI.CanvasScaler.ScaleMode.ConstantPixelSize;
                    break;
                case 1:
                    children.canvasGameObject.GetComponent<CanvasScaler>().uiScaleMode = UnityEngine.UI.CanvasScaler.ScaleMode.ConstantPhysicalSize;
                    break;
                case 2:
                    children.canvasGameObject.GetComponent<CanvasScaler>().uiScaleMode = UnityEngine.UI.CanvasScaler.ScaleMode.ScaleWithScreenSize;
                    break;
                default:
                    children.canvasGameObject.GetComponent<CanvasScaler>().uiScaleMode = UnityEngine.UI.CanvasScaler.ScaleMode.ConstantPixelSize;
                    break;
            }
        }
    }

}