using UnityEngine;
using Steamworks;
using Steamworks.Data;
using System.Threading.Tasks;

namespace HLAPI {
    public class HLNetwork : MonoBehaviour
    {
        public static Lobby CreateLobby(HLLobbyInfo info)
        {
            return CreateLobbyInternal(info).Result;
        }

        internal static async Task<Lobby> CreateLobbyInternal(HLLobbyInfo info)
        {
            var task = SteamMatchmaking.CreateLobbyAsync(info.maxMembers);
            await task;
            if (task.Result.HasValue)
            {
                var lobby = task.Result.Value;
                switch (info.visibility)
                {
                    case HLLobbyVisibility.Private:
                        lobby.SetPrivate();
                        break;
                    case HLLobbyVisibility.Public:
                        lobby.SetPublic();
                        break;
                    case HLLobbyVisibility.FriendsOnly:
                        lobby.SetFriendsOnly();
                        break;
                }
                lobby.SetJoinable(info.joinable);
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