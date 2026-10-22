using UnityEngine;

namespace Playable.Player
{
    /// <summary>
    /// Player長押し攻撃終了
    /// </summary>
    public class HoldAttackEnd : State
    {
        public HoldAttackEnd(PlayerController controller) : base(controller) { }

        bool endAni;


        public override void Start()
        {
            controller.Action.Mode.ReleaseAttack();
            endAni = false;
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


            //アニメーションの経過割合・対応
            if (!endAni)
                endAni = controller.Action.GetTransferAnime;

            if (endAni)
            {
                controller.Action.Animator.SetTrigger("EndAttack");
                controller.ChangeState(controller.Idle);
            }
        }

    }
}
