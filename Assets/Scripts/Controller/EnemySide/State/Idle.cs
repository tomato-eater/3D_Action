using UnityEngine;

namespace Enemy
{
    /// <summary>
    /// EnemyƒAƒCƒhƒ‹
    /// </summary>
    public class Idle : State
    {
        public Idle(EnemyController controller) : base(controller) { }

        float time = 0;
        float timer = 0;

        public override void Start()
        {
            _controller.Action.MyAnimator.SetFloat("MoveVelocity", 0);
            timer = 0;
            time = Random.Range(_controller.IdleTime.x, _controller.IdleTime.y);
        }

        public override void Update()
        {
            if (_controller.Scouting())
            {
                return;
            }

            timer += Time.deltaTime;
            if (timer >= time)
            {
                _controller.ChangeState(_controller.Loiter);
            }
        }
    }
}
