using UnityEngine;

namespace Mirror.Examples.MultipleAdditiveScenes
{
    public class PlayerScore : NetworkBehaviour
    {
        [Networked]
        public int playerNumber;

        [Networked]
        public int scoreIndex;

        [Networked]
        public int matchIndex;

        [Networked]
        public uint score;

        public int clientMatchIndex = -1;

        void OnGUI()
        {
            if (!isServerOnly && !isLocalPlayer && clientMatchIndex < 0)
                clientMatchIndex = NetworkClient.connection.identity.GetComponent<PlayerScore>().matchIndex;

            if (isLocalPlayer || matchIndex == clientMatchIndex)
                GUI.Box(new Rect(10f + (scoreIndex * 110), 10f, 100f, 25f), $"P{playerNumber}: {score}");
        }
    }
}
