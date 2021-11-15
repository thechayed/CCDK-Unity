using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CCDKEngine;
using UnityEngine.InputSystem;
using Unity.Netcode;
using ToolBox.Serialization;

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
        public Vector2 mousePosition = default;
#if USING_NETCODE
        
        public NetworkVariable<Vector3> networkClickPosition = default;
        public NetworkVariable<Vector3> networkClickTransformPosition = default;
#endif
        public int lastMouseButton = 0;

        public override void Start()
        {
            base.Start();

            GoToState(Engine.currentGameType.state);
        }

        public override void OnDestroy()
        {
            PlayerManager.RemovePC(this);
        }

        public override void Update()
        {
            base.Update();

            //if (Engine.enableNetworking)
            //    clickPosition = networkClickPosition.Value;
        }

        public override void NetworkUpdate()
        {
            mousePosition = Mouse.current.position.ReadValue();

            if (Mouse.current.leftButton.IsPressed())
            {
                lastMouseButton = 0;

                if (possessedCamera != null)
                {
                    possessedCamera.gameObject.SetActive(true);
                    Engine.activeCamera = possessedCamera;

                    Ray ray = possessedCamera.ScreenPointToRay(Mouse.current.position.ReadValue());
                    RaycastHit hit;
                    if (Physics.Raycast(ray, out hit))
                    {
                        clickPosition = hit.point;
                        clickedTransform = hit.transform;
#if USING_NETCODE
                        //if (NetworkManager.IsHost)
                        //    networkClickPosition.Value = hit.point;
                        //else
                        //    SetClickPositionServerRpc(clickPosition);

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
                        //if(NetworkManager.IsHost)
                        //    networkClickPosition.Value = hit.point;
                        //else
                        //    SetClickPositionServerRpc(clickPosition);
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
            if (clickedTransform != null && NetworkManager.Singleton.IsHost)
                networkClickTransformPosition.Value = clickedTransform.position;
#endif
        }


#if USING_NETCODE
        [ServerRpc]
        public void SetClickPositionServerRpc(Vector3 clickPosition)
        {
            this.networkClickPosition.Value = clickPosition;
        }
#endif

        /**<summary>Adds replicated Player properties to the Player that owns this Controller.</summary>**/
        public void SetPlayerProp(string name, object value)
        {
#if USING_NETCODE
            if (NetworkManager.Singleton.IsClient)
            {
                if (net.IsOwner)
                {
                    player.playerProperties.Set(name, value);
                    SetPropertyServerRPC(name, DataSerializer.Serialize<object>(value));
                }
            }
            else
            {
                player.playerProperties.Set(name, value);
            }
            PropertyUpdate(name, value);
#endif
        }

        [ServerRpc]
        public void SetPropertyServerRPC(string name, byte[] value)
        {
            player.playerProperties.Set(name, DataSerializer.Deserialize<object>(value));

            SetPropertyClientRPC(name, value);
            PropertyUpdate(name, DataSerializer.Deserialize<object>(value));
        }

        [ClientRpc]
        public void SetPropertyClientRPC(string name, byte[] value)
        {
            player.playerProperties.Set(name, DataSerializer.Deserialize<object>(value));
            PropertyUpdate(name, DataSerializer.Deserialize<object>(value));
        }

        public virtual void PropertyUpdate(string name, object value)
        {

        }

        public object GetProperty(string name)
        {
            return player.playerProperties.Get(name);
        }
    }

}