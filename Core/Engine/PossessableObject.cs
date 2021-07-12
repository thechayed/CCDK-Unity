/* Possessable Objects are Object's that can be possessed by a Controller */

using System.Collections;
using UnityEngine;


namespace CCDKEngine
{
    public class PossessableObject : Object
    {
        /**This object's Player Controller **/
        public Controller controller;
        /**Whether this Object is currently being controlled**/
        public bool isControlled;


        /* Set the object's Player Controller (This should typically be done in the editor!) */
        public void setController(Controller controller)
        {
            this.controller = controller;
            isControlled = true;
        }
    }
}
