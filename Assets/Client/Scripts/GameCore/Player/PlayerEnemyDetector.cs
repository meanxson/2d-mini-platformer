using System;
using UnityEngine;

namespace Client
{
    public class PlayerEnemyDetector : MonoBehaviour
    {
        public event Action<BaseEnemy> EnemyDetectEntered;
        public event Action<BaseEnemy> EnemyDetectExited;

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.TryGetComponent(out BaseEnemy enemy))
            {
                EnemyDetectEntered?.Invoke(enemy);
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.TryGetComponent(out BaseEnemy enemy))
            {
                EnemyDetectExited?.Invoke(enemy);
            }
        }
    }
}