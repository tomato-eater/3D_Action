using UnityEngine;

namespace Enemy
{
    /// <summary>
    /// Enemy徘徊
    /// </summary>
    public class Loiter : EnemyState
    {
        public Loiter(EnemyBehaviorController controller) : base(controller) { }

        bool move = false;

        public override void Start()
        {
            //移動速度変更
            _controller.Agent.speed = _controller.WalkSpeed;
            //移動先の座標を指定
            var moveVector = Random.insideUnitCircle.normalized;
            moveVector *= Random.Range(0, _controller.Area.Radius * _controller.WalkArea);
            var targetPos = _controller.Area.AreaPos + new Vector3(moveVector.x, 0, moveVector.y);
            //移動させる
            _controller.Agent.SetDestination(targetPos);
        }

        public override void Update()
        {
            if (_controller.Scouting())
            {
                return;
            }

            //移動している速度を取得
            var moveSpeed = _controller.Agent.velocity.magnitude;
            //動き出した
            if (!move && moveSpeed != 0.0f)
                move = true;
            //止まった
            if (move && moveSpeed == 0.0f)
                _controller.ChangeState(_controller.Idle);
        }

        public override void End()
        {
            move = false;
        }
    }
}
