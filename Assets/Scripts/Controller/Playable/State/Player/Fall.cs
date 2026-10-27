using UnityEngine;

namespace Playable.Player
{
    /// <summary>
    /// Player—‰º
    /// </summary>
    public class Fall : State
    {
        public Fall(PlayerController controller) : base(controller) { }

        public override void Start()
        {
            _controller.Action.MyAnimator.SetTrigger("Fall");
        }

        public override void Update()
        {
            //UŒ‚“ü—ÍŒŸ’m
            if (_controller.NextAction(out _))
            {
                return;
            }

            //ˆÚ“®
            var moveValue = SInputSystem.Instance.MoveValue;
            if (moveValue != Vector2.zero)
            {
                var forward = _controller.CameraForward();
                forward = (forward * moveValue.y + _controller.cameraTrans.right * moveValue.x) * _controller.CharData.AirSpeed;
                _controller.MoveVector.x = forward.x;
                _controller.MoveVector.z = forward.z;
            }

            //‰º~—Ê‚ÌŒvZ
            _controller.MoveVector.y += _controller.CharData.GravityValue * _controller.ElapsedTime();
            //—‰º‘¬“x‚Ì’²ß
            if (_controller.MoveVector.y < _controller.CharData.MaxFallSpeed)
                _controller.MoveVector.y = _controller.CharData.MaxFallSpeed;

            //ˆÚ“®‚Æ‰ñ“]
            _controller.MoveAndRotate(_controller.MoveVector);

            var moveSpeed = new Vector3(_controller.Controller.velocity.x, 0, _controller.Controller.velocity.z).magnitude;
            _controller.Action.MyAnimator.SetFloat("MoveVelocity", moveSpeed);

            //’…’n”»’è
            if (_controller.Controller.isGrounded)
            {
                _controller.ChangeState(moveSpeed == 0 ? _controller.Idle : _controller.Move);
            }
        }

        public override void End()
        {
            _controller.Action.MyAnimator.SetTrigger("Land");
        }
    }
}

