using System.Collections;
using UnityEngine;
using CCDKEngine;

namespace CCDKGame
{
    public class PawnCamera : CCDKEngine.Object
    {
        // Use this for initialization
        public override void Start()
        {
            base.Start();
            transform.parent.SendMessageUpwards("SetCamera", gameObject.GetComponent<Camera>());
        }
    }
}