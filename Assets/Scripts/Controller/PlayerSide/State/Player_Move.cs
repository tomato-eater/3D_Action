using UnityEngine;
using UnityEngine.UIElements;

/// <summary>
/// 移動
/// </summary>
public class Player_Move : IStateBase
{
    PlayableBehaviorController controller;
    public Player_Move(PlayableBehaviorController player) => controller = player;

    /// <summary>
    /// 現在の移動速度
    /// </summary>
    float currentSpeed = 0.1f;

    Vector2 finalInput = Vector2.zero;


    public override void Update()
    {
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

        //移動入力
        var moveValue = SInputSystem.instance.MoveValue;
        //カメラの前方ベクトル
        var forward = controller.CameraForward();

        //入力中なら
        if (moveValue.sqrMagnitude > 0.0f)
        {
            //移動ベクトルを更新
            finalInput = moveValue;
        }

        controller.moveVector = (forward * finalInput.y + controller.cameraTrans.right * finalInput.x);
        //最大速度指定　バトル中 || 走り中 ? 走り速度 : 歩き速度
        var maxSpeed = SGameManager.instance.ActivityMode == ActivityMode.Battle || SInputSystem.instance.RunButton ? controller.CharData.RunSpeed : controller.CharData.WalkSpeed;
        //入力状況に合わせて最高速度を変える
        maxSpeed = Mathf.Lerp(0, maxSpeed, moveValue.magnitude);
        //徐々に速度を変える
        currentSpeed = Mathf.MoveTowards(currentSpeed, maxSpeed, controller.CharData.VariableSpeed * controller.ElapsedTime());

        //移動ベクトルに速度を掛ける
        controller.moveVector *= currentSpeed;
        //浮かないようにする
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

    public override void End()
    {
        finalInput = Vector2.zero;
    }
}
