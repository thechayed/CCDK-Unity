using System.Collections;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace CCDKObjects
{
    public class PrefabSO : ScriptableObject
    {
        [HideInInspector] public string objectName = "Default";

        [HideInInspector] public GameObject defaultObjectPrefab;

        [Tooltip("The name of the Prefab made for this Object.")]
        public GameObject prefab;

        [ReadOnly] public GameObject lastPrefab;


        [ReadOnly] public bool updated = false;

        [HideInInspector] public string path;

        [HideInInspector] public string className;

        [HideInInspector] public string lastName;

        private bool originalMade = false;


#if UNITY_EDITOR
        /** When the Scriptable Object is created, add it to the Editor Window's list of managed objects **/
        public virtual void OnEnable()
        {

            if (defaultObjectPrefab == null)
            {
                defaultObjectPrefab = Resources.Load<GameObject>("CCDK/PrefabDefaults/Object");
            }

            /** Set path to the Folder this object is in **/
            path = AssetDatabase.GetAssetPath(this);
            path = path.Replace(name + ".asset", "");

            if (Application.isEditor)
            {
                CCDKEngine.Engine.AddEdObject(this);
            }

            if (objectName == "")
                objectName = "Default";

            //if (AssetDatabase.LoadAssetAtPath<GameObject>(path + objectName + ".prefab") == null)
            //{
            //    Debug.Log("Original prefab was made");
            //    GameObject gameObject = GameObject.Instantiate(defaultObjectPrefab);
            //    CCDKEngine.Object component = (CCDKEngine.Object)gameObject.AddComponent(this.className.GetAssemblyType());
            //    if (component != null)
            //    {
            //        component.data = this;
            //    }
            //    prefab = PrefabUtility.SaveAsPrefabAssetAndConnect(gameObject, this.path + this.objectName + ".prefab", InteractionMode.AutomatedAction);
            //    GameObject.DestroyImmediate(gameObject);
            //    originalMade = true;
            //}
        }

        public virtual void Awake()
        {
            /** Set path to the Folder this object is in **/
            path = AssetDatabase.GetAssetPath(this);
            path = path.Replace(name + ".asset", "");


            if (objectName == "")
                objectName = "Default";


            if (defaultObjectPrefab == null)
            {
                defaultObjectPrefab = Resources.Load<GameObject>("CCDK/PrefabDefaults/Object");
            }

            if (AssetDatabase.LoadAssetAtPath<GameObject>(path + objectName + ".prefab") == null)
            {
                //Debug.Log("Original prefab was made");
                GameObject gameObject = GameObject.Instantiate(defaultObjectPrefab);
                CCDKEngine.Object component = (CCDKEngine.Object)gameObject.AddComponent(this.className.GetAssemblyType());
                if (component != null)
                {
                    component.data = this;
                }
                prefab = PrefabUtility.SaveAsPrefabAssetAndConnect(gameObject, path + objectName + ".prefab", InteractionMode.AutomatedAction);
                //Debug.Log(path);
                GameObject.DestroyImmediate(gameObject);
                originalMade = true;
            }
        }

        /** Whenever a change is made to the Object, update it **/
        public virtual void OnValidate()
        {
            if (objectName == "")
                objectName = this.name + "Object";

            /** Set path to the Folder this object is in **/
            path = AssetDatabase.GetAssetPath(this);
            path = path.Replace(name + ".asset", "");

            if (prefab != lastPrefab && prefab != null)
            {
                objectName = prefab.name;
                lastPrefab = prefab;
            }

            updated = true;
        }
#endif
    }
}