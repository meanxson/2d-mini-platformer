namespace Client
{
    public class BossEnemy : BaseEnemy
    {
        private PlayerBehaviour _player;

        protected override void OnEnable()
        {
            base.OnEnable();
            PlayerDetected += OnPlayerDetect;
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            PlayerDetected -= OnPlayerDetect;
        }

        protected override void Die()
        {
            Destroy(gameObject);
        }

        private void OnPlayerDetect(PlayerBehaviour player)
        {
            if (ReferenceEquals(_player, null))
            {
                _player = player;
            }
        }
    }
}