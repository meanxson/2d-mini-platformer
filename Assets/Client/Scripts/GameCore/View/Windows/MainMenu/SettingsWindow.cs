using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

namespace Client
{
    public class SettingsWindow : BaseWindow
    {
        [SerializeField] private BaseWindowsManager _windowsManager;
        [SerializeField] private AudioMixer _audioMixer;
        [SerializeField] private Slider _musicSlider;
        [SerializeField] private Slider _effectSlider;
        [SerializeField] private Button _backButton;

        private const string MusicVolumeKey = "MusicVolume";
        private const string EffectVolumeKey = "EffectVolume";
        private const string MusicVolumeParam = "MusicVolume";
        private const string EffectVolumeParam = "EffectVolume";

        protected override void OnOpen()
        {
            LoadVolume();
            
            _musicSlider.onValueChanged.AddListener(SetMusicVolume);
            _effectSlider.onValueChanged.AddListener(SetEffectVolume);
            _backButton.onClick.AddListener(OnBackButtonClick);
        }

        protected override void OnClose()
        {
            _musicSlider.onValueChanged.RemoveListener(SetMusicVolume);
            _effectSlider.onValueChanged.RemoveListener(SetEffectVolume);
            _backButton.onClick.RemoveListener(OnBackButtonClick);
        }

        private void LoadVolume()
        {
            var music = PlayerPrefs.GetFloat(MusicVolumeKey, 0.5f);
            var effect = PlayerPrefs.GetFloat(EffectVolumeKey, 0.5f);

            _musicSlider.value = music;
            _effectSlider.value = effect;

            SetMusicVolume(music);
            SetEffectVolume(effect);
        }

        private void SetMusicVolume(float value)
        {
            var volume = Mathf.Lerp(-80f, 0f, value);
            _audioMixer.SetFloat(MusicVolumeParam, volume);
            PlayerPrefs.SetFloat(MusicVolumeKey, value);
        }

        private void SetEffectVolume(float value)
        {
            var volume = Mathf.Lerp(-80f, 0f, value);
            _audioMixer.SetFloat(EffectVolumeParam, volume);
            PlayerPrefs.SetFloat(EffectVolumeKey, value);
        }

        private void OnBackButtonClick()
        {
            PlayerPrefs.Save();
            _windowsManager.OpenWindow<MainMenuWindow>();
        }
    }
}
