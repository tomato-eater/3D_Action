using UnityEngine;

namespace Player
{
    /// <summary>
    /// Player—‰º
    /// </summary>
    public class Fall : State
    {
        public Fall(PlayerController controller) : base(controller) { }

        public override void Start()
        {
            controller.Action.Animator.SetTrigger("Fall");
        }

        public override void Update()
        {
            //UŒ‚“ü—ÍŒŸ’m
            if (controller.Attack(out var comboAdd))
            {
                return;
            }

            //ˆÚ“®
            var moveValue = SInputSystem.Instance.MoveValue;
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
            controller.Action.Animator.SetFloat("MoveVelocity", moveSpeed);

            //’…’n”»’è
            if (controller.Controller.isGrounded)
            {
                controller.ChangeState(moveSpeed == 0 ? controller.Idle : controller.Move);
            }
        }

        public override void End()
        {
            controller.Action.Animator.SetTrigger("Land");
        }
    }
}

