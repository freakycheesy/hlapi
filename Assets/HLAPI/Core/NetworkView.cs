using Steamworks;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace HLAPI
{
    public class NetworkView : MonoBehaviour
    {
        public Component observed;
        public Dictionary<string, RpcHandler> rpcs = new();

        public void SerializeView()
        {
            Message message = new Message((uint)MessageIds.Serialize);
            observed.SendMessage("OnSerializeNetworkView", message);
        }
        public void SendRPC(string id, SteamId target, Message stream)
        {
            SendRPCInternal(id, target, stream.ToArray());
        }
        internal void SendRPCInternal(string id, SteamId target, byte[] stream)
        {
        }
    }
}
