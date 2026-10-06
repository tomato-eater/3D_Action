using UnityEngine;

/// <summary>
/// 接近
/// </summary>
public class Player_Approach : StateBase
{
    PlayableBehaviorController controller;
    public Player_Approach(PlayableBehaviorController player) => controller = player;

    Vector3 startPos;

    bool endAni;

    public override void Start()
    {
        controller.Animator.SetTrigger("Approach");

        startPos = controller.MyTransform.position;
        endAni = false;
    }

    public override void Update()
    {
        if (!endAni)
            endAni = controller.Individual.EndAnimator;


        //接敵中に倒された等　ターゲットが居ない時
        if (!controller.target)
        {
            if (endAni)
            {
                controller.ChangeState(controller.StateTapAttack);
                return;
            }
        }
        else
        {
            //自分の座標
            var myPos = controller.AttackPoint.position;

            //目的地(ターゲットの座標)の座標
            var enemyPos = controller.target.transform.position;
            //自身からターゲットの方向のベクトル
            var adjustment = Vector3.Scale(enemyPos - myPos, new Vector3(1, 0, 1)).normalized;

            //調査する座標を調整
            myPos += adjustment * controller.Controller.radius;
            enemyPos -= adjustment * controller.target.Radius;

            //十分に近ければ
            if (Vector3.Distance(myPos, enemyPos) <= controller.Individual.AttackDistance)
            {
                if (endAni)
                {
                    controller.ChangeState(controller.StateTapAttack);
                    return;
                }
            }
            else
            {
                //移動距離が限界か
                if (Vector3.Distance(startPos, controller.MyTransform.position) >= controller.CharData.MaxMoveDistance)
                {
                    if (endAni)
                    {
                        controller.ChangeState(controller.StateTapAttack);
                        return;
                    }
                }

                //移動量計算
                controller.moveVector = (enemyPos - myPos).normalized * controller.CharData.ApproachSpeed;

                controller.MoveAndRotate(controller.moveVector, 2.5f);
            }
        }
    }
}
