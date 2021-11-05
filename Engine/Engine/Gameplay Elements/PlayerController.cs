using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CCDKEngine;
using UnityEngine.InputSystem;
using Unity.Netcode;

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
#if USING_NETCODE
        public NetworkVariable<Vector3> networkClickPosition = default;
        public NetworkVariable<Vector3> networkClickTransformPosition = default;
#endif
        public int lastMouseButton = 0;


        public override void OnDestroy()
        {
            PlayerManager.RemovePC(this);
        }

        public override void Update()
        {
            base.Update();

            clickPosition = networkClickPosition.Value;
        }

        public override void NetworkUpdate()
        {
            if (Mouse.current.leftButton.IsPressed())
            {
                lastMouseButton = 0;

                if (possessedCamera != null)
                {
                    possessedCamera.gameObject.SetActive(true);

                    Ray ray = possessedCamera.ScreenPointToRay(Mouse.current.position.ReadValue());
                    RaycastHit hit;
                    if (Physics.Raycast(ray, out hit))
                    {
                        clickPosition = hit.point;
                        clickedTransform = hit.transform;
#if USING_NETCODE
                        networkClickPosition.Value = hit.point;
#endif
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
#if USING_NETCODE
                        networkClickPosition.Value = hit.point;
#endif
                    }
                }
            }

            if(controllerData!=null)
                if(controllerData.mouseToNavMesh)
                {
                    navMeshAgentDestination = clickPosition;
                    navMeshAgentDestionationTransform = clickedTransform;
                }

#if USING_NETCODE
            if (clickedTransform != null)
                networkClickTransformPosition.Value = clickedTransform.position;
#endif
        }


    }

}