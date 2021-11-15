/**An Objective is a game Object used for keeping score. **/
using System.Collections;
using UnityEngine;
using CCDKEngine;
using CCDKGame;
using System.Collections.Generic;

namespace CCDKGame
{
    public class Objective : CCDKEngine.Object
    {
        public CCDKObjects.Objective objectiveData;
        /**<summary>The collider to use for collision checks. Used most often with Zones.</summary>**/
        public Collider volume;
        [Tooltip("The time that has been given to Acquiring this Objective. Each index is that of each active Team in the Game Type.")]
        public List<float> timeGiven = new List<float>();

        public bool loggedInGameType = false;

        public bool completed = false;

        public override void Start()
        {
            base.Start();
            objectiveData = (CCDKObjects.Objective)data;
            health = objectiveData.health;
            
        }

        public override void Update()
        {
            base.Update();

            if (Engine.currentGameType != null)
            {
                if(!loggedInGameType)
                {
                    Engine.currentGameType.objectives.Add(this);
                    loggedInGameType = true;
                }
            }

            switch (objectiveData.objectiveType.ToString())
            {
                /**Loop through all the current Colliders for this Objective, and update Time Given.**/
                case "Zone":
                    foreach(GameObject collider in currentCollisions)
                    {
                        Pawn pawn = collider.GetComponent<Pawn>();
                        if (pawn != null)
                            if (pawn.controller!=null)
                            {

                            }
                    }
                    break;

                /**Call extendable methods for Captuing/Obtaining, Losing, etc., this Objective.**/
                case "Item":

                    break;

                /**When this objective is Damaged completely, the team that damages it accumulates points.**/
                case "Damage":
                    
                    break;
            }
        }

        public void GiveToPawn(Pawn pawn)
        {
            if (!objectiveData.attachToCollector)
            {
                pawn.inventory.Add(objectiveData.itemToCollect);
            }
        }

    }
}