/** Monobehavior's should use this class inside so the State Machine knows which classes to enable/disable **/

using System.Collections;
using UnityEngine;

namespace CCDKEngine
{
    public class State 
    {
        public bool enabled;
        public GameObject gameObject;
        public MonoBehaviour self;

        /** When the State is added to the game, it must be initialized with a GameObject **/
        public void Init(GameObject gameObject)
        {
            this.gameObject = gameObject;
        }

        public void Enable()
        {
            enabled = true;
            Enter();
        }

        public void Disable()
        {
            Exit();
            enabled = false;
        }

        public virtual void Enter()
        {

        }

        public virtual void Update()
        {

        }

        public virtual void Exit()
        {

        }
    }
}