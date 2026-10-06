using UnityEngine;
using UnityEngine.InputSystem.XR;

/// <summary>
/// —‰º
/// </summary>
public class Player_Fall : StateBase
{
    PlayableBehaviorController controller;
    public Player_Fall(PlayableBehaviorController player) => controller = player;

    public override void Start()
    {
        controller.Animator.SetTrigger("Fall");
    }

    public override void Update()
    {
        //UŒ‚“ü—ÍŒŸ’m
        if (controller.Attack(out var comboAdd))
        {
            return;
        }

        //ˆÚ“®
        var moveValue = SInputSystem.instance.MoveValue;
        if (moveValue != Vector2.zero)
        {
            var forward = controller.CameraForward();
            forward = (forward * moveValue.y + controller.cameraTrans.right * moveValue.x) * controller.CharData.AirSpeed;
            controller.moveVector.x = forward.x;
            controller.moveVector.z = forward.z;
        }

        //‰º~—Ê‚ÌŒvZ
        controller.moveVector.y += controller.CharData.GravityValue * controller.ElapsedTime();
        //—‰º‘¬“x‚Ì’²ß
        if (controller.moveVector.y < controller.CharData.MaxFallSpeed)
            controller.moveVector.y = controller.CharData.MaxFallSpeed;

        //ˆÚ“®‚Æ‰ñ“]
        controller.MoveAndRotate(controller.moveVector);

        var moveSpeed = new Vector3(controller.Controller.velocity.x, 0, controller.Controller.velocity.z).magnitude;
        controller.Animator.SetFloat("MoveVelocity", moveSpeed);

        //’…’n”»’è
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