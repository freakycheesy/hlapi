using Steamworks;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace HLAPI
{
    public class NetworkView : MonoBehaviour
    {
        public Component observed;
        public Dictionary<string, RpcHandler> rpcs { get; private set; } = new();
        public uint netId { get; private set; }
        public string assetId;
        public void SerializeView()
        {
            Message outgoing = new Message((uint)MessageIds.Serialize, true);
            observed.SendMessage("OnSerializeNetworkView", outgoing);
        }
        public void SendRPC(string rpcId, SteamId target, Message stream, P2PSend sendType)
        {
            SendRPCInternal(rpcId, target, stream, sendType);
        }
        internal void SendRPCInternal(string rpcId, SteamId target, Message stream, P2PSend sendType)
        {
            Message outgoing = new Message((uint)MessageIds.Rpc, true);
            outgoing.Write(netId);
            outgoing.Write(rpcId);
            outgoing.Write(stream.ToArray());
            NetworkManager.SendMessage(target, outgoing, sendType);
        }
    }
}
