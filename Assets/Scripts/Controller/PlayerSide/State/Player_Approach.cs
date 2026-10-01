using UnityEngine;

/// <summary>
/// 接近
/// </summary>
public class Player_Approach : IStateBase
{
    PlayableBehaviorController controller;
    public Player_Approach(PlayableBehaviorController player) => controller = player;

    Vector3 startPos;

    public override void Start()
    {
        Debug.Log("接敵");
        startPos = controller.MyTransform.position;
    }

    public override void Update()
    {
        //接敵中に倒された等　遠距離でも可な攻撃
        if (!controller.target || controller.JudgeAttackDistance == 0)
        {
            controller.ChangeState(controller.StateAttack);
            return;
        }

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
        if (Vector3.Distance(myPos, enemyPos) <= controller.JudgeAttackDistance)
        {
            controller.ChangeState(controller.StateAttack);
            return;
        }
        //移動距離が限界か
        if (Vector3.Distance(startPos, controller.MyTransform.position) >= controller.CharData.MaxMoveDistance)
        {
            controller.ChangeState(controller.StateAttack);
            return;
        }

        //移動量計算
        controller.moveVector = (enemyPos - myPos).normalized * controller.CharData.ApproachSpeed;

        controller.MoveAndRotate(controller.moveVector);
    }
}
