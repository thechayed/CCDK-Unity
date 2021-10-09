/** The Component Constructor adds a Dictionary of Components to
 * an object and stores the classes. **/

using B83.Unity.Attributes;
using System;
using System.Collections;
using UnityEngine;

namespace CCDKEngine
{
    public class ComponentConstructor<T>
    {
        /** The object that is using this Component Constructor **/
        GameObject gameObject;
        
        /** The Component Classes created for the object **/
        public Dictionary<Type> classes = new Dictionary<Type>();
        /** The list of class names and their types **/
        public Dictionary<T> classNames = new Dictionary<T>();
        /** The list of defaults from a static class **/
        public Dictionary<T> defaults = new Dictionary<T>();
        /** Has default Components **/
        bool hasDefaults;


        /** Initialize the Component Constructor with this Constructor in order to add classes to the Game Object **/
        public ComponentConstructor(GameObject gameObject, Dictionary<T> classNames, Dictionary<T> defaults = null) 
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

        /*Construction*/
        #region
        /** Called by the Constructer to make sure that all the classes are valid **/
        public void ValidateClasses()
        {
            if (classNames.length > 0)
            {
                foreach (DictionaryItem<T> item in classNames.dictionary)
                {
                    string script = item.value.ToString();

                    /** Check if any of the classes can't be found, alert the user and change the Class name to default **/
                    if (Type.GetType(script) == null)
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
            foreach (DictionaryItem<T> item in classNames.dictionary)
            {
                string name = item.value.ToString();
                gameObject.AddComponent(Type.GetType(name));
                classes.Set(name, Type.GetType(name));
            }
        }
        #endregion

        /** Get an instance of a Component by name **/
        public Component Get(string name)
        {
            return gameObject.GetComponent(classes.Get(name));
        }
    }
}