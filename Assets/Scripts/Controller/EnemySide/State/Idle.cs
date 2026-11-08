using UnityEngine;

namespace Enemy
{
    /// <summary>
    /// Enemyアイドル
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
            //待機する時間を決める
            var idolTime = _controller.Action.EnemyData.IdleTime;
            time = Random.Range(idolTime.x, idolTime.y);
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
