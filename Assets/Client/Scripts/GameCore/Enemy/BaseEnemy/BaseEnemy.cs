using System;
using UnityEngine;

namespace Client
{
    public abstract class BaseEnemy : MonoBehaviour
    {
        [field: SerializeField] public float Health { get; private set; }
        [field: SerializeField] public float Damage { get; private set; }
        [field: SerializeField] public Transform PlayerTarget { get; private set; }
        [SerializeField] private float _moveSpeed = 2f;
        [SerializeField] private EnemyPlayerDetector _detector;
        [SerializeField] private Transform _sprite;


        public event Action<float> HealthChanged;
        public event Action Died;
        public event Action<PlayerBehaviour> PlayerDetected;

        private Rigidbody2D _rigidbody;
        private float _hitLockTimer;

        protected virtual void Awake()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
        }

        protected virtual void OnEnable()
        {
            _detector.PlayerDetectEntered += OnPlayerDetectEnter;
            _detector.PlayerDetectExited += OnPlayerDetectExit;
        }

        protected virtual void OnDisable()
        {
            _detector.PlayerDetectEntered -= OnPlayerDetectEnter;
            _detector.PlayerDetectEntered -= OnPlayerDetectExit;
        }

        private void Update()
        {
            if (_hitLockTimer > 0f)
                _hitLockTimer -= Time.deltaTime;
        }

        private void FixedUpdate()
        {
            if (_hitLockTimer <= 0f)
            {
                FollowPlayer();
            }
        }

        private void FollowPlayer()
        {
            if (PlayerTarget == null) return;

            Vector2 direction = (PlayerTarget.position - transform.position).normalized;
            Vector2 targetPosition = _rigidbody.position + direction * (_moveSpeed * Time.fixedDeltaTime);

            _rigidbody.MovePosition(targetPosition);

            if (_sprite != null && direction.x != 0f)
            {
                _sprite.localScale = new Vector3(direction.x > 0f ? -1f : 1f, 1f, 1f);
            }
        }

        public void ApplyDamage(float damage, Vector2 direction)
        {
            Health -= damage;

            if (Health <= 0)
            {
                Health = 0f;
                Die();
                Died?.Invoke();
            }

            ApplyHitForce(direction);
            HealthChanged?.Invoke(Health);
        }

        private void ApplyHitForce(Vector2 direction)
        {
            _hitLockTimer = 0.2f;
            _rigidbody.velocity = Vector2.zero;
            _rigidbody.AddForce(direction.normalized * 2f, ForceMode2D.Impulse);
        }

        private void OnPlayerDetectEnter(PlayerBehaviour playerBehaviour)
        {
            PlayerTarget = playerBehaviour.transform;
            PlayerDetected?.Invoke(playerBehaviour);
        }

        private void OnPlayerDetectExit(PlayerBehaviour playerBehaviour)
        {
            PlayerTarget = null;
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            if (other.gameObject.TryGetComponent(out PlayerBehaviour playerBehaviour))
            {
                Vector2 hitDirection = (playerBehaviour.transform.position - transform.position).normalized;
                playerBehaviour.ApplyDamage(Damage, hitDirection);
            }
        }

        protected abstract void Die();
    }
}