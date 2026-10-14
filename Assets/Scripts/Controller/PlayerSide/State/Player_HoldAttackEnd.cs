using UnityEngine;

/// <summary>
/// í∑âüÇµçUåÇèIÇÌÇË
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
    }

    public override void Update()
    {
        if (!endAni)
            endAni = controller.Individual.EndAnimator;

        if (endAni)
        {
            controller.Animator.SetTrigger("EndAttack");
            controller.ChangeState(controller.StateIdle);
        }
       

    }

}
