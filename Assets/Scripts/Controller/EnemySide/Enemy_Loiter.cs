using UnityEngine;

/// <summary>
/// ぶらつく
/// </summary>
public class Enemy_Loiter : IStateBase
{

    EnemyController controller;
    public Enemy_Loiter(EnemyController enemy) => controller = enemy;

    bool move = false;

    public void Start()
    {
        //移動速度変更
        controller.Agent.speed = controller.WalkSpeed;
        //移動先の座標を指定
        var moveVector = Random.insideUnitCircle.normalized;
        moveVector *= Random.Range(0, controller.Area.Radius * controller.WalkArea);
        var targetPos = controller.Area.AreaPos + new Vector3(moveVector.x, 0, moveVector.y);
        //移動させる
        controller.Agent.SetDestination(targetPos);
    }

    public void Update()
    {
        if (controller.Scouting())
        {
            return;
        }

        //移動している速度を取得
        var moveSpeed = controller.Agent.velocity.magnitude;
        //動き出した
        if (!move && moveSpeed != 0.0f)
            move = true;
        //止まった
        if (move && moveSpeed == 0.0f)
            controller.ChangeState(controller.StateIdle);
    }

    public void End()
    {
        move = false;
    }
}
