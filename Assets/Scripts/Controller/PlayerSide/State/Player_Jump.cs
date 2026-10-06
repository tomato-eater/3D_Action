using UnityEngine;

/// <summary>
/// ジャンプ
/// </summary>
public class Player_Jump : StateBase
{
    PlayableBehaviorController controller;
    public Player_Jump(PlayableBehaviorController player) => controller = player;

    public override void Start()
    {
        controller.Animator.SetTrigger("Jump");

        //ジャンプ初速計算
        float g = Mathf.Abs(controller.CharData.GravityValue);
        controller.moveVector.y = Mathf.Sqrt(2.0f * controller.CharData.JumpTop * g);
    }

    public override void Update()
    {
        //攻撃入力検知
        if (controller.Attack(out var comboAdd))
        {
            return;
        }

        //移動
        var moveValue = SInputSystem.instance.MoveValue;
        if(moveValue != Vector2.zero)
        {
            var forward = controller.CameraForward();
            forward = (forward * moveValue.y + controller.cameraTrans.right * moveValue.x) * controller.CharData.AirSpeed;
            controller.moveVector.x = forward.x;
            controller.moveVector.z = forward.z;
        }

        //上昇量の計算　調整
        controller.moveVector.y += controller.CharData.GravityValue * controller.ElapsedTime();
        //移動と回転
        controller.MoveAndRotate(controller.moveVector);

        //ジャンプで頂点に達した
        if (controller.moveVector.y <= 0) 
        {
            controller.ChangeState(controller.StateFall);
            return;
        }
    }
}
