using UnityEngine;
using UnityEngine.Audio;

namespace Client
{
    public class AppStartup : MonoBehaviour
    {
        [SerializeField] private AudioMixer _mixer;
        
        private const string MusicVolumeKey = "MusicVolume";
        private const string EffectVolumeKey = "EffectVolume";
        
        private const string MusicVolumeParam = "MusicParameter";
        private const string EffectVolumeParam = "EffectsParameter";

        private void Start()
        {
            LoadVolume();
        }

        private void LoadVolume()
        {
            var music = PlayerPrefs.GetFloat(MusicVolumeKey, 0.5f);
            var effect = PlayerPrefs.GetFloat(EffectVolumeKey, 0.5f);

            _mixer.SetFloat(MusicVolumeParam, Mathf.Lerp(-80f, 0f, music));
            _mixer.SetFloat(EffectVolumeParam, Mathf.Lerp(-80f, 0f, effect));
        }
    }
}