namespace Mirror.Examples.Chat
{
    public class Player : NetworkBehaviour
    {
        [Networked]
        public string playerName;

        public override void OnStartServer()
        {
            playerName = (string)connectionToClient.authenticationData;
        }

        public override void OnStartLocalPlayer()
        {
            ChatUI.localPlayerName = playerName;
        }
    }
}
