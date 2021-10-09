using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CCDKEngine
{
    public class CameraClass : PossessableObject
    {
        [SerializeField]
        /**List of Camera Modes**/
        public enum CameraModes
        {
            firstPerson,
            thirdPerson
        };
        public CameraModes cameraMode;

    }
}
