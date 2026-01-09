using UnityEngine;

namespace Mirror.Examples.NetworkRoom
{
    [AddComponentMenu("")]
    public class PlayerScore : NetworkBehaviour
    {
        [Networked]
        public int index;

        [Networked]
        public uint score;

        void OnGUI()
        {
            GUI.Box(new Rect(10f + (index * 110), 10f, 100f, 25f), $"P{index}: {score:0000000}");
        }
    }
}
