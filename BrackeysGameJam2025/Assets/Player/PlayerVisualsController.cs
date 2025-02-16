#nullable enable

using UnityEngine;

namespace GameJam.Player
{
    public class PlayerVisualsController : MonoBehaviour
    {
        [SerializeField]
        private Renderer? visualsRenderer;

        public void ShowParryState()
        {
            visualsRenderer!.material.color = Color.red;
        }

        public void ShowNormalState()
        {
            visualsRenderer!.material.color = Color.white;
        }

        public void ShowDashState()
        {

        }
    }
}
