/* Possessable Objects are Object's that can be possessed by a Controller */

using System.Collections;
using UnityEngine;


namespace CCDKEngine
{
    public class PossessableObject : Object
    {
        [Header(" - Possessable Object Properties - ")]

        /**This object's Player Controller **/
        [ReadOnly] public Controller controller;
        /**Whether this Object is currently being controlled**/
        [HideInInspector] public bool isControlled;


        /* Set the object's Player Controller (This should typically be done in the editor!) */
        public void setController(Controller controller)
        {
            this.controller = controller;
            isControlled = true;
        }
    }
}
