namespace HLAPI
{
    public struct CreateLobbyData
    {
        public int maxMembers;
        public LobbyVisibility visibility;
        public bool joinable;

        public static CreateLobbyData CreateDefault()
        {
            return new CreateLobbyData()
            {
                joinable = true,
                visibility = LobbyVisibility.Public,
                maxMembers = 32
            };
        }
    }
}