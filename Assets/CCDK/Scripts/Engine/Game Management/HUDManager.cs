using CCDKGame;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CCDKEngine
{
    public class HUDManager
    {
        /** The currently active HUD object. **/
        public static HUD active;

        /** The HUD's that have been created **/
        private static List<GameObject> liveHuds = new List<GameObject>();

        /** Create a new HUD object **/
        public static void Create(CCDKObjects.HUD HUDObject)
        {
            GameObject hudObject = new GameObject();
            hudObject.name = HUDObject.name;
            hudObject.AddComponent(Type.GetType(HUDObject.HUDClass.script));

            if (liveHuds.Count == 0)
            {
                active = hudObject.GetComponent<HUD>();
            }
        }
    }
}