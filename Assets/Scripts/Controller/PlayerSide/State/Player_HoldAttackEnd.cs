using UnityEngine;

/// <summary>
/// 長押し攻撃終わり
/// </summary>
public class Player_HoldAttackEnd : StateBase
{
    PlayableBehaviorController controller;
    public Player_HoldAttackEnd(PlayableBehaviorController player) => controller = player;

    bool endAni;


    public override void Start()
    {
        controller.Animator.SetTrigger("HoldAttackE");
        endAni = false;
        controller.moveVector = Vector3.zero;
    }

    public override void Update()
    {
        if (controller.target)
        {
            //自分の座標
            var myPos = controller.AttackPoint.position;
            //目的地(ターゲットの座標)の座標
            var enemyPos = controller.target.transform.position;
            //自身からターゲットの方向のベクトル
            var adjustment = Vector3.Scale(enemyPos - myPos, new Vector3(1, 0, 1)).normalized;

            //回転方向計算
            var moveVector = (enemyPos - myPos).normalized;

            controller.MoveAndRotate(moveVector, 2.5f);
        }

        if (!endAni)
            endAni = controller.Individual.EndAnimator;

        if (endAni)
        {
            controller.Animator.SetTrigger("EndAttack");
            controller.ChangeState(controller.StateIdle);
        }
       

    }

}
