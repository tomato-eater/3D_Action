using UnityEngine;
using UnityEngine.InputSystem.XR;

/// <summary>
/// 待機
/// </summary>
public class Player_Idle : StateBase
{
    PlayableBehaviorController controller;
    public Player_Idle(PlayableBehaviorController player) => controller = player;

    public override void Start()
    {
        controller.Animator.SetFloat("MoveVelocity", 0);
        controller.moveVector = Vector3.zero;
    }

    public override void Update()
    {
        //攻撃入力検知
        if (controller.Attack(out var comboAdd))
        {
            return;
        }

        //ジャンプ入力検知
        if (controller.CheckJump())
        {
            return;
        }
        //落下した
        if (!controller.IsGround())
        {
            controller.ChangeState(controller.StateFall);
            return;
        }
        //戦闘中なら
        if (SGameManager.instance.ActivityMode == ActivityMode.Battle)
        {
            //回避入力検知
            if (controller.Avoid())
            {
                return;
            }
            //防御入力検知
            if (SInputSystem.instance.DefenseButton)
            {
                controller.ChangeState(controller.StateDefense);
                return;
            }
        }

        //移動入力検知
        if (SInputSystem.instance.MoveValue != Vector2.zero)
        {
            controller.ChangeState(controller.StateMove);
            return;
        }

        //浮かないようにする
        controller.moveVector.y = -0.1f;

        controller.MoveAndRotate(Vector3.zero);
    }
}
