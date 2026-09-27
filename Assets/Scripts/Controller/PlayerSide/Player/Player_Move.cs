using UnityEngine;
using UnityEngine.UIElements;

/// <summary>
/// 移動
/// </summary>
public class Player_Move : IStateBase
{
    PlayerController controller;
    public Player_Move(PlayerController player) => controller = player;

    float currentSpeed;

    public void Start()
    {

    }

    public void Update()
    {
        //ジャンプした
        if (controller.CheckJump())
        {
            return;
        }
        //攻撃入力検知
        if (controller.Attack())
        {
            return;
        }
        //落下した
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
        

        //移動入力
        var moveValue = SInputSystem.instance.MoveValue;
        //移動ベクトル
        var forward = controller.CameraForward();
        //最大速度指定　バトル中 || 走り中 ? 走り速度 : 歩き速度
        var maxSpeed = controller.IsBattle || SInputSystem.instance.AvoidButton ? controller.CharData.RunSpeed : controller.CharData.WalkSpeed;
        //入力状況に合わせて最高速度を変える
        maxSpeed = Mathf.Lerp(0, maxSpeed, moveValue.magnitude);
        Debug.Log(maxSpeed);
        controller.moveVector = (forward * moveValue.y + controller.cameraTrans.right * moveValue.x) * maxSpeed;
        //移動量計算
        controller.moveVector.y = -0.01f;
        //移動と回転
        controller.MoveAndRotate(controller.moveVector);

        //移動速度でアニメーションの速度を変える
        var moveVelocity = controller.GetMoveSpeed() * Time.timeScale;
        controller.Animator.SetFloat("MoveVelocity", moveVelocity);

        //停止した
        if (moveVelocity == 0)
        {
            controller.ChangeState(controller.StateIdle);
            return;
        }
    }

    public void End()
    {

    }
}
