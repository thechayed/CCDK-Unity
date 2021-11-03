using System.Collections;
using UnityEngine;
using System.Reflection;
#if USING_NETCODE
using Unity.Netcode;
#endif

namespace CCDKEngine
{
    public class MultiplayerManager : MonoBehaviour
    {
#if USING_NETCODE
        public Dictionary<string> multiplayerMessages = new Dictionary<string>();
        public CustomMessagingManager messagingManager;
        public static MultiplayerManager singleton;

        // Use this for initialization
        void Start()
        {
            singleton = this;
            messagingManager = NetworkManager.Singleton.CustomMessagingManager;
        }

        // Update is called once per frame
        void Update()
        {

        }

        public static void AddMessage(string messageName, object actingObject, MethodInfo method, object[] parameters)
        {
            //Receiving
            singleton.messagingManager.RegisterNamedMessageHandler(messageName, (senderClientId, reader) =>
            {
                reader.ReadValueSafe(out string message); //Example
                
                method.Invoke(actingObject, parameters);
            });
        }
#endif
    }
}