using UnityEngine;

/// <summary>
/// ’·‰Ÿ‚µUŒ‚ŠJn
/// </summary>
public class Player_HoldAttackBegin : StateBase
{
    PlayableBehaviorController controller;
    public Player_HoldAttackBegin(PlayableBehaviorController player) => controller = player;

    float chargeValue = 0;

    public override void Start()
    {
        Debug.Log("¡‚¶‚á‚È‚¢");


        controller.Individual.OnHold(true, chargeValue);

    }

    public override void Update()
    {
        chargeValue += Time.unscaledDeltaTime;

        controller.Individual.OnHold(false, chargeValue);

        if ((chargeValue >= controller.Individual.MaxCharge && controller.Individual.AutoChange) || !SInputSystem.Instance.DerivedAttackTrigger)
        {

            controller.Individual.Release(chargeValue);
            controller.ChangeState(controller.StateHoldAttackE);
        }


    }
}
