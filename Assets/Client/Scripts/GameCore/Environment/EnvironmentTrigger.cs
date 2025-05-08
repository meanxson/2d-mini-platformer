using System;
using UnityEngine;

namespace Client
{
    public class EnvironmentTrigger : MonoBehaviour
    {
        [SerializeField] private EnvironmentTriggerType _triggerType;

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.TryGetComponent(out PlayerBehaviour playerBehaviour))
            {
                HandleTrigger(playerBehaviour);
            }
        }

        private void HandleTrigger(PlayerBehaviour playerBehaviour)
        {
            switch (_triggerType)
            {
                case EnvironmentTriggerType.Die:
                    playerBehaviour.Die();
                    break;
                case EnvironmentTriggerType.Finish:
                    playerBehaviour.Win();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}