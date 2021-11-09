using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CCDKEditor
{
    public class GOManager
    {
        public static Dictionary<GameObject> gameObjects;

        public static void AddObject(GameObject gameObject)
        {
            if (Find(gameObject))
            {
                gameObjects.Set("" + gameObjects.length, gameObject);
            }
        }

        public static bool Find(GameObject gameObject)
        {
            foreach (DictionaryItem<GameObject> item in gameObjects.dictionary)
            {
                if (item.value == gameObject)
                {
                    return false;
                }
            }
            return true;
        }
    }
}