namespace HLAPI
{
    public struct HLLobbyInfo
    {
        public int maxMembers;
        public HLLobbyVisibility visibility;
        public bool joinable;

        public static HLLobbyInfo CreateDefault()
        {
            return new HLLobbyInfo()
            {
                joinable = true,
                visibility = HLLobbyVisibility.Public,
                maxMembers = 32
            };
        }
    }
}