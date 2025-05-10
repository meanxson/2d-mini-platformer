using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

namespace Client
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class PlayerBehaviour : MonoBehaviour
    {
        [SerializeField, BoxGroup("Stats")] private float _health;
        [SerializeField, BoxGroup("Stats")] private float _damage;
        [SerializeField, BoxGroup("Stats")] private float _moveSpeed = 7f;
        [SerializeField, BoxGroup("Stats")] private float _jumpForce = 12f;

        [SerializeField, BoxGroup("Parameter")]
        private Transform _mesh;

        [SerializeField, BoxGroup("Parameter")]
        private Transform _groundCheck;

        [SerializeField, BoxGroup("Parameter")]
        private float _groundCheckRadius = 0.2f;

        [SerializeField, BoxGroup("Parameter")]
        private LayerMask _groundLayer;

        [SerializeField, BoxGroup("Parameter")]
        private Animator _animator;

        [SerializeField, BoxGroup("Shoot parameter")]
        private Bullet _bulletPrefab;

        [SerializeField, BoxGroup("Shoot parameter")]
        private Transform _shootPoint;

        [SerializeField, BoxGroup("Shoot parameter")]
        private float _bulletSpeed = 15f;

        [SerializeField, BoxGroup("Attack Delay")]
        private float _meleeCooldown = 0.5f;

        [SerializeField, BoxGroup("Attack Delay")]
        private float _shootCooldown = 0.5f;


        [SerializeField] private PlayerEnemyDetector _detector;
        [SerializeField] private AudioSource _audioSource;

        public bool IsDied { get; private set; }
        public bool IsWon { get; set; }


        public event Action<float> HealthChanged;
        public event Action Died;
        public event Action Won;

        private Rigidbody2D _rigidbody;
        private bool _isGrounded;
        private float _horizontal;
        private float _hitLockTimer;

        private List<BaseEnemy> _enemies;

        private SceneLoader _sceneLoader;

        private float _meleeTimer;
        private float _shootTimer;

        private static readonly int Run = Animator.StringToHash("Run");
        private static readonly int Jump = Animator.StringToHash("IsJump");
        private static readonly int Attack = Animator.StringToHash("Attack");
        private static readonly int IsDieHash = Animator.StringToHash("IsDie");
        private static readonly int IsWin = Animator.StringToHash("IsWin");

        [Inject]
        public void Constructor(SceneLoader sceneLoader)
        {
            _sceneLoader = sceneLoader;
        }

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
            _enemies = new List<BaseEnemy>();
        }

        private void OnEnable()
        {
            _detector.EnemyDetectEntered += OnEnemyDetectEnter;
            _detector.EnemyDetectExited += OnEnemyDetectExit;
        }

        private void OnDisable()
        {
            _detector.EnemyDetectEntered -= OnEnemyDetectEnter;
            _detector.EnemyDetectExited -= OnEnemyDetectExit;
        }

        private void Update()
        {
            if (IsDied)
                return;

            if (IsWon)
                return;

            if (_hitLockTimer > 0f)
                _hitLockTimer -= Time.deltaTime;

            if (_meleeTimer > 0f)
                _meleeTimer -= Time.deltaTime;

            if (_shootTimer > 0f)
                _shootTimer -= Time.deltaTime;

            _horizontal = Input.GetAxisRaw("Horizontal");

            if (_horizontal > 0.01f)
                _mesh.localScale = new Vector3(1f, 1f, 1f);
            else if (_horizontal < -0.01f)
                _mesh.localScale = new Vector3(-1f, 1f, 1f);

            _isGrounded = Physics2D.OverlapCircle(_groundCheck.position, _groundCheckRadius, _groundLayer);
            _animator.SetBool(Jump, !_isGrounded);

            if (Input.GetButtonDown("Jump") && _isGrounded)
            {
                _rigidbody.velocity = new Vector2(_rigidbody.velocity.x, _jumpForce);
            }

            if (_horizontal == 0)
            {
                if (Input.GetButtonDown("Fire1") && _meleeTimer <= 0f)
                {
                    if (_enemies.Count != 0)
                    {
                        foreach (var enemy in _enemies)
                        {
                            Vector2 hitDirection = (enemy.transform.position - transform.position).normalized;
                            enemy.ApplyDamage(_damage, hitDirection);
                        }
                    }

                    _animator.SetTrigger(Attack);
                    _meleeTimer = _meleeCooldown;
                    _audioSource.PlayOneShot(_audioSource.clip);
                }

                if (Input.GetButtonDown("Fire2") && _shootTimer <= 0f)
                {
                    Shoot();
                    _animator.SetTrigger(Attack);
                    _shootTimer = _shootCooldown;
                }
            }
        }

        private void FixedUpdate()
        {
            if (_hitLockTimer <= 0f)
            {
                var direction = new Vector2(_horizontal * _moveSpeed, _rigidbody.velocity.y);
                _rigidbody.velocity = direction;
                _animator.SetFloat(Run, Mathf.Abs(_horizontal));
            }
        }

        private void Shoot()
        {
            if (_bulletPrefab == null || _shootPoint == null) return;

            var bullet = Instantiate(_bulletPrefab, _shootPoint.position, Quaternion.identity);
            var direction = new Vector2(_mesh.localScale.x, 0f);
            bullet.Rigidbody.velocity = direction * _bulletSpeed;
        }

        public void ApplyDamage(float damage, Vector2 direction)
        {
            _health -= damage;

            if (_health <= 0)
            {
                Die();
            }

            ApplyHitForce(direction);
            HealthChanged?.Invoke(_health);
        }


        [Button]
        public void Die()
        {
            if (!IsDied)
            {
                _animator.SetFloat(Run, 0);
                _health = 0;
                _rigidbody.velocity = new Vector2(0, 0);
                _animator.SetBool(IsDieHash, true);


                IsDied = true;
                Died?.Invoke();
                HealthChanged?.Invoke(_health);

                _sceneLoader.ReloadCurrentScene(1.5f);
            }
        }

        public void Win()
        {
            IsWon = true;
            _animator.SetFloat(Run, 0);

            _rigidbody.velocity = new Vector2(0, 0);
            _animator.SetBool(IsWin, true);

            var currentIndex = SceneManager.GetActiveScene().buildIndex;
            var nextIndex = currentIndex + 1;

            if (nextIndex < SceneManager.sceneCountInBuildSettings)
            {
                var nextSceneName = SceneUtility.GetScenePathByBuildIndex(nextIndex);
                nextSceneName = System.IO.Path.GetFileNameWithoutExtension(nextSceneName);
                _sceneLoader.Load(nextSceneName, 1.5f);
            }
            else
            {
                var firstSceneName = SceneUtility.GetScenePathByBuildIndex(0);
                firstSceneName = System.IO.Path.GetFileNameWithoutExtension(firstSceneName);
                _sceneLoader.Load(firstSceneName, 1.5f);
            }

            Won?.Invoke();
        }

        private void ApplyHitForce(Vector2 direction)
        {
            _hitLockTimer = 0.2f;
            _rigidbody.velocity = Vector2.zero;
            _rigidbody.AddForce(direction.normalized * 10f, ForceMode2D.Impulse);
        }

        private void OnEnemyDetectEnter(BaseEnemy enemy)
        {
            _enemies.Add(enemy);
        }

        private void OnEnemyDetectExit(BaseEnemy enemy)
        {
            _enemies.Remove(enemy);
        }

        private void OnDrawGizmosSelected()
        {
            if (_groundCheck == null) return;
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(_groundCheck.position, _groundCheckRadius);
        }
    }
}