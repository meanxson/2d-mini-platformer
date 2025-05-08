using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Client
{
    public class GameplayView : MonoBehaviour
    {
        [SerializeField] private PlayerBehaviour _playerBehaviour;
        [SerializeField] private Slider _healthBarSlider;

        private void OnEnable()
        {
            _playerBehaviour.HealthChanged += OnHealthChange;
        }

        private void OnDisable()
        {
            _playerBehaviour.HealthChanged -= OnHealthChange;
        }

        private void OnHealthChange(float currentHealth)
        {
            _healthBarSlider.DOValue(currentHealth, 0.5f);
        }
    }
}