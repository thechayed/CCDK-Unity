/* The Level class contains the name, Scene, and properties of a Level to be loaded 
 When creating a Level, you should create a new Level prefab in the edior, and set
it's information according the level you're creating. */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using CCDKEngine;
using System;

namespace CCDKGame
{
    public class Level : StateMachineComponent
    {
        /** The Level Game Object this Level belongs to **/
        public GameObject levelObj;
        /** The scene that belongs to this level **/
        public Scene scene;

        public bool isReady=false;

        public string levelName;

        public bool setActiveWhenLoaded;

        private void Update()
        {
            if (scene.isLoaded&&!isReady)
            {
                LevelManager.LevelLoaded(this.levelName);
                isReady = true;
            }
        }

        public struct Properties
        {
            /** The list of Game Modes that can be played on this level **/
            public List<string> supportedGameModes;
        }

        /** Should Load this Level's scene additively **/
        public Scene LoadScene(string name)
        {
            SceneManager.LoadScene(name, LoadSceneMode.Additive);
            return SceneManager.GetSceneByName(name);
        }
    }
}