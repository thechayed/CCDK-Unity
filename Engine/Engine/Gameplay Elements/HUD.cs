/*The HUD class contains base code for Heads Up Displays in game, extend it to make another! */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using CCDKEngine;
using UnityEngine.SceneManagement;

namespace CCDKGame
{
    public class HUD : PossessableObject
    {
        public CCDKObjects.HUD data;

        // Use this for initialization
        public override void Start()
        {
            base.Start();

            Scene activeScene = SceneManager.GetActiveScene();
            SceneManager.LoadScene(data.scene,LoadSceneMode.Additive);
            SceneManager.SetActiveScene(activeScene);
            
            /** If the HUD does not have a Canvas to draw on set from the editor, create a new one **/
            //if (data.canvas == null)
            //{
            //    data.canvasGameObject = new GameObject();
            //    data.canvasGameObject.name = "HUDCanvas";
            //    data.canvasGameObject.AddComponent<Canvas>();
            //    data.canvas = data.canvasGameObject.GetComponent<Canvas>();
            //    data.canvasGameObject.AddComponent<CanvasScaler>();
            //    data.canvasGameObject.AddComponent<GraphicRaycaster>();
            //}
            /** Otherwise, get the GO from the Canvas that has been given to us**/
            //else
            //{
            //    data.canvasGameObject = data.canvas.gameObject;
            //}

            //SetScaleMode((int)data.scaleMode);
        }

        /* Simple HUD Drawing Functions */
        public void DrawText(string name = "Text", string text = "", Font font = null, int fontSize = 16, float x = 0, float y = 0, float width = 100, float height = 16)
        {
            if (font == null)
            {
                font = data.defaultFont;
            }

            GameObject myText;

            myText = new GameObject();
            myText.transform.parent = data.canvasGameObject.transform;
            myText.name = "wibble";

            Text textC = myText.AddComponent<Text>();
            textC.font = font;
            textC.text = text;
            textC.fontSize = fontSize;

            // Text position
            RectTransform rectTransform = textC.GetComponent<RectTransform>();
            rectTransform.localPosition = new Vector3(0, 0, 0);
            rectTransform.sizeDelta = new Vector2(400, 200);
        }
    }

}