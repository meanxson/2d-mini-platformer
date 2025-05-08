using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Client
{
    public class MainMenuWindow : BaseWindow
    {
        [SerializeField] private BaseWindowsManager _windowsManager;
        [SerializeField] private TMP_Text _titleLabel;

        [SerializeField] private Button _playButton;
        [SerializeField] private Button _settingsButton;
        [SerializeField] private Button _exitButton;

        private SceneLoader _sceneLoader;

        private const string _levelName = "Level_1";

        [Inject]
        public void Constructor(SceneLoader sceneLoader)
        {
            _sceneLoader = sceneLoader;
        }

        private void Start()
        {
            AnimateTitle();
        }

        private void OnDisable()
        {
            if (_titleLabel != null)
                _titleLabel.transform.DOKill(true);
        }

        protected override void OnOpen()
        {
            _playButton.onClick.AddListener(OnPlayButton);
            _settingsButton.onClick.AddListener(OnSettingsButtonClick);
            _exitButton.onClick.AddListener(OnExitButtonClick);
        }

        protected override void OnClose()
        {
            _playButton.onClick.RemoveListener(OnPlayButton);
            _settingsButton.onClick.RemoveListener(OnSettingsButtonClick);
            _exitButton.onClick.RemoveListener(OnExitButtonClick);
        }

        private void OnPlayButton()
        {
            _sceneLoader.Load(_levelName);
        }

        private void OnSettingsButtonClick()
        {
            _windowsManager.OpenWindow<SettingsWindow>();
        }

        private void OnExitButtonClick()
        {
            Application.Quit(0);
        }

        private void AnimateTitle()
        {
            if (_titleLabel == null) return;

            _titleLabel.transform.localScale = Vector3.one;
            _titleLabel.transform
                .DOScale(1.1f, 1f)
                .SetEase(Ease.InOutSine)
                .SetLoops(-1, LoopType.Yoyo);
        }
    }
}