using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CCDKEngine;
using UnityEngine.InputSystem;

namespace CCDKGame
{
    public class PlayerController : Controller
    {
        [Header(" - Player - ")]
        /**Player Info*/
        public Player player;

        [Header(" - Player Input - ")]
        public Vector3 clickPosition = default;
        public Transform clickedTransform = default;
        public int lastMouseButton = 0;


        private void OnDestroy()
        {
            PlayerManager.RemovePC(this);
        }

        public virtual void Update()
        {
            if (Mouse.current.leftButton.IsPressed())
            {
                lastMouseButton = 0;

                if (possessedCamera != null)
                {
                    Ray ray = possessedCamera.ScreenPointToRay(Mouse.current.position.ReadValue());
                    RaycastHit hit;
                    if (Physics.Raycast(ray, out hit))
                    {
                        clickPosition = hit.point;
                        clickedTransform = hit.transform;
                    }
                }
            }

            if (Mouse.current.rightButton.IsPressed())
            {
                lastMouseButton = 1;

                if (possessedCamera != null)
                {
                    Ray ray = possessedCamera.ScreenPointToRay(Mouse.current.position.ReadValue());
                    RaycastHit hit;
                    if (Physics.Raycast(ray, out hit))
                    {
                        clickPosition = hit.point;
                        clickedTransform = hit.transform;
                    }
                }
            }

            if(controllerData!=null)
                if(controllerData.mouseToNavMesh)
                {
                    navMeshAgentDestination = clickPosition;
                    navMeshAgentDestionationTransform = clickedTransform;
                }
        }


    }

}