using UnityEngine;
using UnityEngine.InputSystem.XR;

/// <summary>
/// óéâ∫
/// </summary>
public class Player_Fall : IStateBase
{
    PlayableBehaviorController controller;
    public Player_Fall(PlayableBehaviorController player) => controller = player;

    public override void Start()
    {
        controller.Animator.SetTrigger("Fall");
    }

    public override void Update()
    {
        //à⁄ìÆ
        var moveValue = SInputSystem.instance.MoveValue;
        if (moveValue != Vector2.zero)
        {
            var forward = controller.CameraForward();
            forward = (forward * moveValue.y + controller.cameraTrans.right * moveValue.x) * controller.CharData.AirSpeed;
            controller.moveVector.x = forward.x;
            controller.moveVector.z = forward.z;
        }

        //â∫ç~ó ÇÃåvéZ
        controller.moveVector.y += controller.CharData.GravityValue * controller.ElapsedTime();
        //óéâ∫ë¨ìxÇÃí≤êﬂ
        if (controller.moveVector.y < controller.CharData.MaxFallSpeed)
            controller.moveVector.y = controller.CharData.MaxFallSpeed;

        //à⁄ìÆÇ∆âÒì]
        controller.MoveAndRotate(controller.moveVector);

        var moveSpeed = new Vector3(controller.Controller.velocity.x, 0, controller.Controller.velocity.z).magnitude;
        controller.Animator.SetFloat("MoveVelocity", moveSpeed);

        //íÖínîªíË
        if (controller.IsGround())
        {
            controller.ChangeState(moveSpeed == 0 ? controller.StateIdle : controller.StateMove);
        }
    }

    public override void End()
    {
        controller.Animator.SetTrigger("Land");

    }
}