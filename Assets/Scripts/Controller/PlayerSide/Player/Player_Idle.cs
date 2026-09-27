using UnityEngine;
using UnityEngine.InputSystem.XR;

/// <summary>
/// 待機
/// </summary>
public class Player_Idle : IStateBase
{
    PlayerController controller;
    public Player_Idle(PlayerController player) => controller = player;

    public void Start()
    {
        controller.Animator.SetFloat("MoveVelocity", 0);
        controller.Animator.SetBool("Idle", true);
        controller.moveVector = Vector3.zero;
    }

    public void Update()
    {
        //ジャンプ入力検知
        if(controller.CheckJump())
        {
            return;
        }
        //攻撃入力検知
        if (controller.Attack())
        {
            return;
        }
        //落下中
        if (!controller.IsGround())
        {
            controller.ChangeState(controller.StateFall);
            return;
        }
        if (controller.IsBattle)
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
        controller.moveVector.y = -0.01f;

        controller.MoveAndRotate(Vector3.zero);
    }

    public void End()
    {
        controller.Animator.SetBool("Idle", false);

    }
}
