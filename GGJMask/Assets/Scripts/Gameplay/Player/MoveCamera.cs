using UnityEngine;

namespace Gameplay.Player
{
    public class MoveCamera : MonoBehaviour
    {
        public Transform cameraPosition;

        private void LateUpdate()
        {
            if (cameraPosition == null) return;
            transform.position = cameraPosition.position;
            transform.rotation = cameraPosition.rotation;
        }
    }
}