using System;
using UnityEngine;

namespace Client
{
    public class EnemyPlayerDetector : MonoBehaviour
    {
        public event Action<PlayerBehaviour> PlayerDetectEntered;
        public event Action<PlayerBehaviour> PlayerDetectExited;

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.TryGetComponent(out PlayerBehaviour playerBehaviour))
            {
                PlayerDetectEntered?.Invoke(playerBehaviour);
            }
        }


        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.TryGetComponent(out PlayerBehaviour playerBehaviour))
            {
                PlayerDetectExited?.Invoke(playerBehaviour);
            }
        }
    }
}