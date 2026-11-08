using UnityEngine;

namespace Enemy
{
    /// <summary>
    /// Enemy徘徊
    /// </summary>
    public class Loiter : State
    {
        public Loiter(EnemyController controller) : base(controller) { }

        bool move = false;

        public override void Start()
        {
            //移動速度変更
            _controller.Agent.speed = _controller.Action.EnemyData.MoveSpeed;
            //移動先の座標を指定
            var moveVector = Random.insideUnitCircle * Random.Range(0.1f, _controller.Area.Radius * _controller.WalkArea);
            var targetPos = _controller.Area.AreaPos + new Vector3(moveVector.x, 0, moveVector.y);
            //移動開始
            _controller.Agent.SetDestination(targetPos);
            //移動方向は前方のみ
            _controller.Action.MyAnimator.SetFloat("MoveVectorY", 1);
            _controller.Action.MyAnimator.SetFloat("MoveVectorX", 0);
        }

        public override void Update()
        {
            if (_controller.Scouting())
            {
                return;
            }

            //移動している速度を取得
            var moveSpeed = _controller.Agent.velocity.magnitude;
            _controller.Action.MyAnimator.SetFloat("MoveVelocity", moveSpeed);

            //動き出した
            if (!move && moveSpeed != 0.0f)
                move = true;
            //止まった
            if (move && moveSpeed == 0.0f)
                _controller.ChangeState(_controller.Idle);
        }

        public override void End()
        {
            _controller.Action.MyAnimator.SetFloat("MoveVectorY", 0);
            move = false;
        }
    }
}
