using Sirenix.OdinInspector;
using UnityEngine;
using DG.Tweening;

namespace Client
{
    public class MovingPlatform : MonoBehaviour
    {
        [SerializeField, BoxGroup("Move Point")] private Transform _aPoint;
        [SerializeField, BoxGroup("Move Point")] private Transform _bPoint;
        [SerializeField, BoxGroup("Move Settings")] private float _duration = 2f;
        [SerializeField, BoxGroup("Move Settings")] private Ease _ease = Ease.InOutSine;

        private void Start()
        {
            if (_aPoint == null || _bPoint == null)
            {
                Debug.LogWarning("MovingPlatform: Missing move points.");
                return;
            }

            transform.position = _aPoint.position;

            transform.DOMove(_bPoint.position, _duration)
                .SetEase(_ease)
                .SetLoops(-1, LoopType.Yoyo);
        }
    }
}