using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using CCDKGame;
using B83.Unity.Attributes;
using System.Threading.Tasks;
using UnityEngine.SceneManagement;

namespace CCDKEngine
{
    public class Engine 
    {
        /**<summary> The Engine Data, recieved from the last created Engine object </summary>**/
        public static CCDKObjects.Engine data;

        /**<summary> Whether the Runtime has started </summary>**/
        public static bool running;

        /** Initialize the engine loop **/
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        static async void Init()
        {

            /*Begin play!*/
            running = true;

            while (running)
            {
                Engine.Update();

                // Update Game at 60fps
                await Task.Delay(8);
            }
        }

        
        static void Update()
        {
            /** Create the Engine scene and load the first level **/
            if(data!=null&&!data.startingLevelLoaded)
            {
                Engine.InitEngineTools();
            }
        }

        static void InitEngineTools()
        {
            LevelManager.MakeEngineScene();
            GameObject runtimeObj = new GameObject();
            runtimeObj.name = "Runtime";
            StateMachine sm = runtimeObj.AddComponent<StateMachine>();
            sm.nest.macro = data.runtimeGraph;
            sm.enabled = true;

            LevelManager.GoToLevel(data.startingLevelName);
            data.startingLevelLoaded = true;
        }
    }
}