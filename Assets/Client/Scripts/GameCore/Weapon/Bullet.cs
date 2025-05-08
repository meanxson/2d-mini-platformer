using System;
using UnityEngine;

namespace Client
{
    public class Bullet : MonoBehaviour
    {
        public Rigidbody2D Rigidbody { get; private set; }

        [SerializeField] private float _lifeTime = 2f;
        [SerializeField] private float _damage = 10f;

        private void Awake()
        {
            Rigidbody = GetComponent<Rigidbody2D>();
        }

        private void Start()
        {
            Destroy(gameObject, _lifeTime);
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.TryGetComponent(out BaseEnemy enemy))
            {
                Vector2 direction = (enemy.transform.position - transform.position).normalized;
                enemy.ApplyDamage(_damage, direction);
                Destroy(gameObject);
            }
        }
    }
}