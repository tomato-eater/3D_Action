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
            if (controller.Action.Mode.HoldAttack())
            {
                controller.ChangeState(controller.HoldAttackE);
                return;
            }

            controller.moveVector = Vector3.zero;
        }

        public override void Update()
        {
            if (controller.target)
            {
                //自分の座標
                var myPos = controller.Action.AttackPoint.position;
                //目的地(ターゲットの座標)の座標
                var enemyPos = controller.target.transform.position;
                //自身からターゲットの方向のベクトル
                var adjustment = Vector3.Scale(enemyPos - myPos, new Vector3(1, 0, 1)).normalized;

                //回転方向計算
                var moveVector = (enemyPos - myPos).normalized;

                controller.MoveAndRotate(moveVector, 2.5f);
            }

            if (controller.Action.Mode.HoldAttack(controller.ElapsedTime()) || !SInputSystem.Instance.DerivedAttackTrigger)
            {
                controller.ChangeState(controller.HoldAttackE);
            }
        }

    }
}