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
    public class Level : FSM.Component
    {
        [Header(" - Level - ")]
        /** The Level Game Object this Level belongs to **/
        public GameObject levelObj;

        public CCDKObjects.Level levelData;

        /** The scene that belongs to this level **/
        public Scene scene;

        public string sceneName;

        public bool isReady=false;

        public string levelName;

        public bool setActiveWhenLoaded;


        [Header(" - Level Children - ")]
        public List<GameObject> Objects = new List<GameObject>();

        public List<GameObject> spawnPoints = new List<GameObject>();

        /* Add/Remove the Level object when  */

        public void Add()
        {
            LevelManager.AddLevel(gameObject);
        }

        public override void OnDestroy()
        {
            LevelManager.RemoveLevel(gameObject);
        }

        private void Update()
        {
            if (scene.isLoaded&&!isReady)
            {
                LevelManager.CheckIfNextLevel(this.levelName, transform);
                Add();
                isReady = true;
                sceneName = scene.name;
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
            return scene = SceneManager.GetSceneByName(name);
        }

        public void GetSpawn(GameObject spawnPointObject)
        {
            spawnPoints.Add(spawnPointObject);
        }

        public Transform FindSpawn(int team)
        {
            int teamSpawnCount = 0;
            List<GameObject> spawns = new List<GameObject>();

            foreach(GameObject spawn in spawnPoints)
            {
                if (spawn.GetComponent<SpawnPoint>().team == team)
                {
                    teamSpawnCount++;
                    spawns.Add(spawn);
                }
            }

            return spawns[(int)UnityEngine.Random.Range(0,teamSpawnCount)].transform;
        }
    }
}