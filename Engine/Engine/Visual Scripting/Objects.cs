using System.Collections;
using UnityEngine;
using System;
using CCDKEngine;
using CCDKGame;

namespace CCDKVisualScripting
{
    public class Objects : MonoBehaviour
    {
        /** Create an object and return it **/
        public static GameObject CreateObject()
        {
            return new GameObject();
        }

        /** Add a specified component and return it **/
        public static Type AddComponent(GameObject gameObject, Type componentType)
        {
            gameObject.AddComponent(componentType);
            return componentType;
        }

        /** Add a component to an object by class  name **/
        public static Type AddComponentByName(GameObject gameObject, string componentName)
        {
            if (Type.GetType(componentName) != null)
            { 
                gameObject.AddComponent(Type.GetType(componentName));
                return Type.GetType(componentName);
            }
            else
            {
                return null;
            }
        }
    }
}