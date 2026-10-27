using UnityEngine;

namespace Playable.Player
{
    /// <summary>
    /// Player長押し攻撃開始
    /// </summary>
    public class HoldAttackStart : State
    {
        public HoldAttackStart(PlayerController controller) : base(controller) { }


        public override void Start()
        {
            //ホールドが不要なら移行
            if (_controller.Action.Mode.HoldAttack())
            {
                _controller.ChangeState(_controller.HoldAttackE);
                return;
            }

            _controller.MoveVector = Vector3.zero;
        }

        public override void Update()
        {
            if (_controller.target)
            {
                //自分の座標
                var myPos = _controller.Action.AttackPoint.position;
                //目的地(ターゲットの座標)の座標
                var enemyPos = _controller.target.transform.position;

                //回転方向計算
                var moveVector = (enemyPos - myPos).normalized;

                _controller.MoveAndRotate(moveVector, 2.5f);
            }

            if (_controller.Action.Mode.HoldAttack(_controller.ElapsedTime()) || !SInputSystem.Instance.DerivedAttackTrigger)
            {
                _controller.ChangeState(_controller.HoldAttackE);
            }
        }

    }
}