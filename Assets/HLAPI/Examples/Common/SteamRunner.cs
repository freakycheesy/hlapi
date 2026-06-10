using Steamworks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HLAPI.Examples
{
    public class SteamRunner : MonoBehaviour
    {
        public uint appId = 480;
        private void OnEnable()
        {
            DontDestroyOnLoad(gameObject);
            SteamClient.Init(appId);
        }

        private void Update()
        {
            SteamClient.RunCallbacks();
        }

        private void OnDisable()
        {
            SteamClient.Shutdown();
        }
    }
}
