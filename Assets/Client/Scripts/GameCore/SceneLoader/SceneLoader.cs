using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;

namespace Client
{
    public class SceneLoader : MonoBehaviour
    {
        [SerializeField] private Image _transitionImage;
        [SerializeField] private float _transitionDuration = 0.5f;

        public void Load(string sceneName, float delay = 0)
        {
            _transitionImage.rectTransform.anchoredPosition = new Vector2(-1920f, 0f);

            var sequence = DOTween.Sequence();
            sequence.AppendInterval(delay);
            sequence.Append(_transitionImage.rectTransform.DOAnchorPosX(0f, _transitionDuration)
                .SetEase(Ease.InOutCubic));
            sequence.AppendCallback(() =>
            {
                SceneManager.sceneLoaded += OnSceneLoaded;
                SceneManager.LoadScene(sceneName);
            });
        }

        public void ReloadCurrentScene(float delay)
        {
            _transitionImage.rectTransform.anchoredPosition = new Vector2(-1920f, 0f);

            var sequence = DOTween.Sequence();
            sequence.AppendInterval(delay);
            sequence.Append(_transitionImage.rectTransform.DOAnchorPosX(0f, _transitionDuration)
                .SetEase(Ease.InOutCubic));
            sequence.AppendCallback(() =>
            {
                SceneManager.sceneLoaded += OnSceneLoaded;
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            });
        }

        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;

            _transitionImage.rectTransform.anchoredPosition = new Vector2(0f, 0f);
            _transitionImage.rectTransform.DOAnchorPosX(1920f, _transitionDuration).SetEase(Ease.InOutCubic);
        }
    }
}