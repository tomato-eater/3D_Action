using UnityEngine;

/// <summary>
/// ’·‰Ÿ‚µUŒ‚I‚í‚è
/// </summary>
public class Player_HoldAttackEnd : StateBase
{
    PlayableBehaviorController controller;
    public Player_HoldAttackEnd(PlayableBehaviorController player) => controller = player;


    float time;

    public override void Start()
    {
        Debug.Log("HoldAttack");
    }

    public override void Update()
    {

        time += controller.ElapsedTime();
        if (time > 1.0f)
        {
            //‰ñ”ğ“ü—ÍŒŸ’m
            if (SGameManager.instance.ActivityMode == ActivityMode.Battle && controller.Avoid())
            {
                return;
            }
        }
        if (time > 1.8f)
        {
            controller.ChangeState(controller.StateIdle);
            return;
        }
    }

}
