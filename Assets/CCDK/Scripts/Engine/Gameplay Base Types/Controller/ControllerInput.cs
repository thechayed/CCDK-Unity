/* Controller Input handles interaction between the Controller and it's Possessed objects */

using System.Collections;
using UnityEngine;
using System.Collections.Generic;

namespace CCDKEngine
{
    public class ControllerInput : ControllerClass
    {
        public override void Start()
        {
            base.Start();
        }

        public class AxisValue
        {
            /** The index of the input in the Values List **/
            public int index;
            /** Value **/
            public float value;
        }

        /** The List of input Values **/
        public List<int> values;
        public List<AxisValue> axisValues;

        /** The Input function sets the value of an Input (If the value is not 0 or 1, axisValues will be used
         * Axis values will be a predetermined fraction of their actual Axis Value, which will allow for smoother
         * multiplayer syncing **/
        public void Input(int index, float value = 1)
        {

        }
    }

}
