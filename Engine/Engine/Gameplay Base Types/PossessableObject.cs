/* Possessable Objects are Object's that can be possessed by a Controller */

using System.Collections;
using UnityEngine;
# if USING_NETCODE
using Unity.Netcode;
#endif


namespace CCDKEngine
{
    public class PossessableObject : Object
    {
        [Header(" - Possessable Object Properties - ")]

        /**This object's Player Controller **/
        [ReadOnly] public Controller controller;
        /**Whether this Object is currently being controlled**/
        [HideInInspector] public bool isControlled;
#if USING_NETCODE
        bool enableNetworkedPossession = false;
        public NetworkVariable<bool> possessed = default;
        /**The ID of the Controller that has possessed this object, over Networking**/
        public NetworkVariable<ulong> controllerID = default;
#endif

        public override void Update()
        {
            base.Update();

            if (possessed.Value && controller == null)
                controller = NetworkManager.SpawnManager.SpawnedObjects[controllerID.Value].GetComponent<Controller>();
        }

        /**<summary>Set the Object's Controller</summary>*/
        public virtual bool SetController(Controller controller)
        {
#if USING_NETCODE
            /**If net is enabled**/
            if(Engine.enableNetworking)
                if (NetworkManager.Singleton.IsHost)
                {
                    NetworkObject controllerObject = controller.GetComponent<NetworkObject>();
                    SetControllerClientRPC(controllerObject.NetworkObjectId);
                    net.ChangeOwnership(controllerObject.OwnerClientId);
                    controllerID.Value = controllerObject.NetworkObjectId;
                    possessed.Value = true;
                }
#endif
            this.controller = controller;
            isControlled = true;
            return true;
        }

#if USING_NETCODE
        [ClientRpc]
        /**<summary>Set the Controller of the possessed Object, on the Client, from the Controller's NetworkObject ID</summary>**/
        public void SetControllerClientRPC(ulong controllerNetworkObjectID)
        {
            controller = NetworkManager.SpawnManager.SpawnedObjects[controllerNetworkObjectID].GetComponent<Controller>();
        }
#endif
    }
}
