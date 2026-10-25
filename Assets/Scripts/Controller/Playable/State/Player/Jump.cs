using UnityEngine;


namespace Playable.Player
{
    /// <summary>
    /// Playerジャンプ
    /// </summary>
    public class Jump : State
    {
        public Jump(PlayerController controller) : base(controller) { }

        public override void Start()
        {
            _controller.Action.MyAnimator.SetTrigger("Jump");

            //ジャンプ初速計算
            float g = Mathf.Abs(_controller.CharData.GravityValue);
            _controller.MoveVector.y = Mathf.Sqrt(2.0f * _controller.CharData.JumpTop * g);
        }

        public override void Update()
        {
            //攻撃入力検知
            if (_controller.NextAction(out var comboAdd))
            {
                return;
            }

            //移動
            var moveValue = SInputSystem.Instance.MoveValue;
            if (moveValue != Vector2.zero)
            {
                var forward = _controller.CameraForward();
                forward = (forward * moveValue.y + _controller.cameraTrans.right * moveValue.x) * _controller.CharData.AirSpeed;
                _controller.MoveVector.x = forward.x;
                _controller.MoveVector.z = forward.z;
            }

            //上昇量の計算　調整
            _controller.MoveVector.y += _controller.CharData.GravityValue * _controller.ElapsedTime();
            //移動と回転
            _controller.MoveAndRotate(_controller.MoveVector);

            //ジャンプで頂点に達した
            if (_controller.MoveVector.y <= 0)
            {
                _controller.ChangeState(_controller.Fall);
                return;
            }
        }
    }

}
