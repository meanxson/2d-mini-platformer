using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

namespace Client
{
    public class WinView : MonoBehaviour
    {
        [SerializeField] private TMP_Text _winLabel;
        [SerializeField] private Button _okayButton;
        [SerializeField] private CanvasGroup _canvasGroup;
        [SerializeField] private float _animationDuration = 0.4f;

        [SerializeField] private BossEnemy _bossEnemy;
        [SerializeField] private PlayerBehaviour _player;

        private Tween _currentTween;

        private void OnEnable()
        {
            _okayButton.onClick.AddListener(OnOkayButtonClick);
            _bossEnemy.Died += OnBossEnemyDie;
        }

        private void OnDisable()
        {
            _okayButton.onClick.RemoveListener(OnOkayButtonClick);
            _bossEnemy.Died -= OnBossEnemyDie;
        }

        private void Start()
        {
            _canvasGroup.alpha = 0f;
            _canvasGroup.interactable = false;
            _canvasGroup.blocksRaycasts = false;
        }

        private void OnBossEnemyDie()
        {
            Show("Победа!");
        }

        private void OnOkayButtonClick()
        {
            _player.IsWon = true;
            _player.Win();
            Hide();
        }

        public void Show(string message)
        {
            _winLabel.text = message;
            _canvasGroup.alpha = 0f;
            _canvasGroup.interactable = false;
            _canvasGroup.blocksRaycasts = false;
            transform.localScale = Vector3.zero;

            gameObject.SetActive(true);

            _currentTween?.Kill();
            _currentTween = DOTween.Sequence()
                .Append(transform.DOScale(1f, _animationDuration).SetEase(Ease.OutBack))
                .Join(_canvasGroup.DOFade(1f, _animationDuration))
                .OnComplete(() =>
                {
                    _canvasGroup.interactable = true;
                    _canvasGroup.blocksRaycasts = true;
                });
        }

        public void Hide(Action onComplete = null)
        {
            _canvasGroup.interactable = false;
            _canvasGroup.blocksRaycasts = false;

            _currentTween?.Kill();
            _currentTween = DOTween.Sequence()
                .Append(transform.DOScale(0f, _animationDuration).SetEase(Ease.InBack))
                .Join(_canvasGroup.DOFade(0f, _animationDuration))
                .OnComplete(() =>
                {
                    gameObject.SetActive(false);
                    onComplete?.Invoke();
                });
        }
    }
}