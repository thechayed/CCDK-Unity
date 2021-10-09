using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;
using System.Reflection;
using System.Timers;
using UnityEditor.SceneManagement;

namespace CCDKEditor
{
    public class EditorManager : MonoBehaviour
    {
        private static bool initialized = false;
        private static float AutosaveTime = 5f;
        private static bool autosaveActive = false;
        static Timer autosaveTimer;


        /** Get the Game Object Prefab from the CCDK Resources **/
        private static GameObject gameObjectPrefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/CCDK/Resources/CCDK/PrefabDefaults/Object.prefab");
        private static GameObject pawnPrefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/CCDK/Resources/CCDK/PrefabDefaults/Pawn.prefab");


        /** Initialize the editor loop **/
        [InitializeOnLoadMethod]
        private static async void InitEditorAsync()
        {


            initialized = true;
            while (initialized && !EditorApplication.isPlayingOrWillChangePlaymode)
            {
                if(CCDKEngine.Engine.editorObjects.Count != CCDKEngine.Engine.lastEditorObjectsLength)
                {
                    UpdateObject(CCDKEngine.Engine.editorObjects[CCDKEngine.Engine.editorObjects.Count-1]);
                    CCDKEngine.Engine.lastEditorObjectsLength = CCDKEngine.Engine.editorObjects.Count;
                }

                if (UnityEditor.Menu.GetChecked("Tools/Timed Autosave"))
                {
                    if (autosaveTimer == null)
                    {
                        autosaveTimer = new Timer(5000);
                        autosaveTimer.Elapsed += AutoSave;
                        autosaveTimer.AutoReset = true;
                        autosaveTimer.Enabled = true;
                        Debug.Log("autosave enabled");
                    }
                }
                else
                {
                    if (autosaveTimer != null)
                    {
                        autosaveTimer.Elapsed -= AutoSave;
                        autosaveTimer = null;
                    }
                }

                foreach (CCDKObjects.PrefabSO scriptable in CCDKEngine.Engine.editorObjects)
                {
                    if (scriptable.updated)
                    {
                        if (!EditorApplication.isPlayingOrWillChangePlaymode)
                        {
                            UpdateObject(scriptable);
                        }
                    }

                }

                // update game at 60fps
                await Task.Delay(16);
            }
        }

        public static void UpdateObject(CCDKObjects.PrefabSO scriptable)
        {
            //Debug.Log(scriptable.objectName);

            if (scriptable.prefab == null && scriptable.lastPrefab != null)
                scriptable.prefab = scriptable.lastPrefab;

            /** If the Pawn's Game Object does not currently exist, make a new one **/
            if (scriptable.prefab == null&&AssetDatabase.LoadAssetAtPath<GameObject>(scriptable.path + scriptable.objectName + ".prefab") == null)
            {
                GameObject gameObject = GameObject.Instantiate(pawnPrefab);
                CCDKEngine.Object component = (CCDKEngine.Object)gameObject.AddComponent(scriptable.className.GetAssemblyType());
                if (component != null)
                {
                    component.data = scriptable;
                }
                if (scriptable.objectName == "")
                    scriptable.objectName = "Default";

                scriptable.prefab = PrefabUtility.SaveAsPrefabAssetAndConnect(gameObject, scriptable.path + scriptable.objectName + ".prefab", InteractionMode.AutomatedAction);
                GameObject.DestroyImmediate(gameObject);
                //Debug.Log("Remade from original prefab");
            }
            /** Otherwise, update it's Pawn class and Name **/
            else
            {
                string name = null;
                GameObject gameObject;
                if (scriptable.prefab == null)
                {
                    //scriptable.prefab = AssetDatabase.LoadAssetAtPath<GameObject>(scriptable.path + scriptable.objectName + ".prefab");
                }
                else
                {
                    name = scriptable.prefab.name;
                    scriptable.objectName = name;
                }
                scriptable.prefab = AssetDatabase.LoadAssetAtPath<GameObject>(scriptable.path + scriptable.objectName + ".prefab");
                gameObject = GameObject.Instantiate(scriptable.prefab);

                if (scriptable.prefab.name != scriptable.objectName&&scriptable.objectName!=null)
                {
                    Debug.Log("Deleted old object after rename");
                    AssetDatabase.DeleteAsset(PrefabUtility.GetPrefabAssetPathOfNearestInstanceRoot(scriptable.prefab));
                }
                foreach (Component item in gameObject.GetComponents(typeof(CCDKEngine.Object)))
                {
                    Component.DestroyImmediate(item, true);
                }
                CCDKEngine.Object component = (CCDKEngine.Object)gameObject.AddComponent(scriptable.className.GetAssemblyType());
                if (component != null)
                {
                    component.data = scriptable;
                }

                if(name==null)
                    scriptable.prefab = PrefabUtility.SaveAsPrefabAssetAndConnect(gameObject, scriptable.path + scriptable.objectName + ".prefab", InteractionMode.AutomatedAction);
                else
                    scriptable.prefab = PrefabUtility.SaveAsPrefabAssetAndConnect(gameObject, scriptable.path + name + ".prefab", InteractionMode.AutomatedAction);

                GameObject.DestroyImmediate(gameObject);
                //Debug.Log("Should update new Prefab");
            }
            if(scriptable.prefab != null)
                scriptable.updated = false;
        }

        /** Add a Scriptable Object to the list if it hasn't already **/
        public static void Add(CCDKObjects.PrefabSO data)
        {
            if (!initialized)
            {
                InitEditorAsync();
            }
        }

        /**<summary>Searches all Scripts in all assemblies, and their base types, to look for field duplicates</summary>**/
        public static void FindFieldDuplicates()
        {
            Assembly[] loadedAssemblies = AppDomain.CurrentDomain.GetAssemblies();
            foreach (Assembly assembly in loadedAssemblies)
            {
                if (assembly.GetName().Name == "CCDK")
                {
                    List<TypeInfo> types = new List<TypeInfo>();
                    types.AddRange(assembly.DefinedTypes);
                    foreach (TypeInfo type in types)
                    {
                        bool found = false;
                        string fieldName = "";

                        FieldInfo[] fields = type.GetFields();
                        foreach (FieldInfo field in fields)
                        {
                            found = type.BaseType.GetField(field.Name) != null;

                            fieldName = field.Name;
                        }

                        if (found)
                            Debug.Log(type.FullName+" and it's Base Type:"+ type.BaseType +", have the same field name: "+fieldName);
                    }
                }
            }
        }

        private static void AutoSave(System.Object source, ElapsedEventArgs e)
        {
            EditorSceneManager.SaveOpenScenes();
            AssetDatabase.SaveAssets();
            Debug.Log("Autosaved");
        }
    }

}