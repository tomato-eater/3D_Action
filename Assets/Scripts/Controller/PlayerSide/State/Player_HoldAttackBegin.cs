using UnityEngine;

/// <summary>
/// 長押し攻撃開始
/// </summary>
public class Player_HoldAttackBegin : StateBase
{
    PlayableBehaviorController controller;
    public Player_HoldAttackBegin(PlayableBehaviorController player) => controller = player;

    float chargeValue = 0;

    public override void Start()
    {
        controller.moveVector = Vector3.zero;
        controller.Individual.OnHold(true, chargeValue);

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

        chargeValue += Time.unscaledDeltaTime;

        controller.Individual.OnHold(false, chargeValue);

        if ((chargeValue >= controller.Individual.MaxCharge && controller.Individual.AutoChange) || !SInputSystem.Instance.DerivedAttackTrigger)
        {

            controller.Individual.Release(chargeValue);
            controller.ChangeState(controller.StateHoldAttackE);
        }


    }
}
