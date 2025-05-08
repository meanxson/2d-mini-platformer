using DG.Tweening;
using UnityEngine;

namespace Client
{
    public class Enemy : BaseEnemy
    {
        protected override void Die()
        {
            transform.DORotate(new Vector3(0f, 0f, 360f), 0.5f, RotateMode.FastBeyond360)
                .SetEase(Ease.InBack);

            transform.DOScale(Vector3.zero, 0.5f)
                .SetEase(Ease.InBack)
                .OnComplete(() => Destroy(gameObject));
        }
    }
}