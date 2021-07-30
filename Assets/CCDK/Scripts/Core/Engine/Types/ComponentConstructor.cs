/** The Component Constructor adds a Dictionary of Components to
 * an object and stores the classes. **/

using System;
using System.Collections;
using UnityEngine;

namespace CCDKEngine
{
    public class ComponentConstructor
    {
        /** The object that is using this Component Constructor **/
        GameObject gameObject;
        
        /** The Component Classes created for the object **/
        public Dictionary<Type> classes = new Dictionary<Type>();
        /** The list of class names and their types **/
        public Dictionary<string> classNames = new Dictionary<string>();
        /** The list of defaults from a static class **/
        public Dictionary<string> defaults = new Dictionary<string>();
        /** Has default Components **/
        bool hasDefaults;


        /** Initialize the Component Constructor with this Constructor in order to add classes to the Game Object **/
        public ComponentConstructor(GameObject gameObject, Dictionary<string> classNames, Dictionary<string> defaults = null) 
        {
            this.gameObject = gameObject;
            this.classNames.Load(classNames);
            if (defaults != null)
            {
                this.defaults.Load(classNames);
                hasDefaults = true;
            }
            else
            {
                hasDefaults = false;
            }

            /** Make sure that the Classes exist **/
            if (hasDefaults)
            {
                ValidateClasses();
            }
            /** Remove components that have been previously added **/
            RemovePreviousClasses();
            /** Add the Component Classes **/
            AddClassComponents();
        }

        /** Called by the Constructer to make sure that all the classes are valid **/
        public void ValidateClasses()
        {
            if (classNames.length > 0)
            {
                foreach (DictionaryItem<string> item in classNames.dictionary)
                {
                    /** Check if any of the classes can't be found, alert the user and change the Class name to default **/
                    if (Type.GetType(item.value) == null)
                    {
                        classNames.Set(item.key, defaults.Get(item.key));
                    }
                }
            }
            else
            {
                /** If there are no classes added, load Classes from Defaults **/
                Debug.LogWarning("Pawn's classes do not exist, filling with defaults!");
                classNames.Load(defaults);
            }
        }

        /** Called by the Constructer to remove previously added components **/
        public void RemovePreviousClasses()
        {
            if(classes.length > 0)
            {
                foreach (DictionaryItem<Type> item in classes.dictionary)
                {
                    if (item.value != null)
                    {
                        GameObject.DestroyImmediate(gameObject.GetComponent(item.value));
                    }
                }
                classes = default(Dictionary<Type>);
            }
        }

        /** Called by the Constructor to Add Component classes and set their PawnClass values **/
        public void AddClassComponents()
        {
            foreach (DictionaryItem<string> item in classNames.dictionary)
            {
                gameObject.AddComponent(Type.GetType(item.value));
                classes.Set(item.value, Type.GetType(item.value));
            }
        }
    }
}