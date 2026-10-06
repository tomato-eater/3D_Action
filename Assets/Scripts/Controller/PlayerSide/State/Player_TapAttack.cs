using UnityEngine;

/// <summary>
/// UŒ‚
/// </summary>
public class Player_TapAttack : StateBase
{
    PlayableBehaviorController controller;
    public Player_TapAttack(PlayableBehaviorController player) => controller = player;

    uint combo = 0;

    bool endAni = false;

    float time;

    public override void Start()
    {
        if(combo >= controller.Individual.MaxCombo)
        {
            combo = 0;
            controller.ChangeState(controller.StateIdle);
            return;
        }

        controller.Animator.SetTrigger("TapAttack");
        combo++;
        endAni = false;
        time = 0;
    }

    public override void Update()
    {
        if (!endAni)
            endAni = controller.Individual.EndAnimator;

        if (endAni)
        {
            time += controller.ElapsedTime();
            if (time > 0.1f)
            {
                //‰ñ”ğ“ü—ÍŒŸ’m
                if (SGameManager.instance.ActivityMode == ActivityMode.Battle && controller.Avoid())
                {
                    combo = 0;
                    controller.Animator.SetTrigger("EndAttack");
                    return;
                }

                //UŒ‚“ü—ÍŒŸ’m
                if (combo < controller.Individual.MaxCombo && controller.Attack(out var comboAdd)) 
                {
                    if (!comboAdd)
                    {
                        combo = 0;
                        controller.Animator.SetTrigger("EndAttack");
                    }
                    return;
                }
            }
            if (time > 0.15f)
            {
                bool tapAttackTrigger = SInputSystem.instance.TapAttackTrigger;
                combo = 0;
                controller.Animator.SetTrigger("EndAttack");
                controller.ChangeState(controller.StateIdle);
                return;
            }
        }
    }
}
