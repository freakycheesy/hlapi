using UnityEngine;
using Steamworks;
using Steamworks.Data;
using System.Threading.Tasks;
using System.Collections.Generic;
using System;

namespace HLAPI {
    public class NetworkManager : MonoBehaviour
    {
        public static event MessageHandler OnMessageReceived;

        public static Dictionary<uint, NetworkView> spawned = new();
        public static NetworkManager singleton;
        private void Start()
        {
            DontDestroyOnLoad(this);
            if (!singleton) { 
                singleton = this;
                SteamNetworking.OnP2PSessionRequest = (steamid) =>
                {
                    // If we want to let this steamid talk to us
                    SteamNetworking.AcceptP2PSessionWithUser(steamid);
                };
                OnMessageReceived += HandleMessages;
            }
            else Destroy(gameObject);
        }
        private void HandleMessages(uint messageId, SteamId sender, Message incoming)
        {
            switch ((MessageIds)messageId) {
                case MessageIds.Rpc:
                    spawned[incoming.ReadUInt()].rpcs[incoming.ReadString()].Invoke(incoming);
                    break;
                case MessageIds.Spawn:

                    break;
            }
        }

        private void Update()
        {
            if (SteamNetworking.IsP2PPacketAvailable())
            {
                var packet = SteamNetworking.ReadP2PPacket();
                if (packet.HasValue)
                {
                    HandleMessageFrom(packet.Value.SteamId, packet.Value.Data);
                }
            }
        }

        private static void HandleMessageFrom(SteamId sender, byte[] data)
        {
            Message incoming = new(data, true);
            OnMessageReceived.Invoke(incoming.MessageId, sender, incoming);
        }

        public static void SendMessage(SteamId target, Message message, P2PSend sendType)
        {
            if(!SteamNetworking.SendP2PPacket(target, message.ToArray(), sendType: sendType))
            {
                Debug.LogError("Send P2P Packet Error");
            }
        }

        public static Lobby CreateLobby(CreateLobbyData data)
        {
            return CreateLobbyInternal(data).Result;
        }

        internal static async Task<Lobby> CreateLobbyInternal(CreateLobbyData data)
        {
            var task = SteamMatchmaking.CreateLobbyAsync(data.maxMembers);
            await task;
            if (task.Result.HasValue)
            {
                var lobby = task.Result.Value;
                switch (data.visibility)
                {
                    case LobbyVisibility.Private:
                        lobby.SetPrivate();
                        break;
                    case LobbyVisibility.Public:
                        lobby.SetPublic();
                        break;
                    case LobbyVisibility.FriendsOnly:
                        lobby.SetFriendsOnly();
                        break;
                }
                lobby.SetJoinable(data.joinable);
                return lobby;
            }
            else
            {
                Debug.LogError("Lobby Creation Error");
                return new Lobby();
            }
        }

        public static Lobby JoinLobby(string lobbyId)
        {
            return JoinLobbyInternal(lobbyId).Result;
        }

        internal static async Task<Lobby> JoinLobbyInternal(string lobbyId)
        {
            SteamId id = new SteamId()
            {
                Value = ulong.Parse(lobbyId)
            };
            var task = SteamMatchmaking.JoinLobbyAsync(id);
            await task;
            return task.Result.Value;
        }
    }
}