using UnityEngine;

/// <summary>
/// í∑âüÇµçUåÇäJén
/// </summary>
public class Player_HoldAttackBegin : StateBase
{
    PlayableBehaviorController controller;
    public Player_HoldAttackBegin(PlayableBehaviorController player) => controller = player;

    float chargeValue = 0;

    public override void Start()
    {
        Debug.Log("ç°Ç∂Ç·Ç»Ç¢");

        if(controller.Individual.MaxCharge <= 0)
        {
            controller.Individual.Release(0);
            controller.ChangeState(controller.StateHoldAttackE);
            return;
        }
        chargeValue = 0;

        controller.Individual.OnHold(true, chargeValue);

    }

    public override void Update()
    {
        chargeValue += Time.unscaledDeltaTime;

        controller.Individual.OnHold(false, chargeValue);

        if ((chargeValue >= controller.Individual.MaxCharge && controller.Individual.AutoChange) || !SInputSystem.instance.DerivedAttackTrigger)
        {

            controller.Individual.Release(chargeValue);
            controller.ChangeState(controller.StateHoldAttackE);
        }


    }
}
