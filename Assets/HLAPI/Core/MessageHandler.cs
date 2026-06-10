using Steamworks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HLAPI
{
    public delegate void MessageHandler(uint messageId, SteamId sender, Message incoming);
}
