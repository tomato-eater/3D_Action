using UnityEngine;

namespace Playable.Player
{
    /// <summary>
    /// Player防御
    /// </summary>
    public class Defense : State
    {
        public Defense(PlayerController controller) : base(controller) { }

        public override void Start()
        {
            Debug.Log("ぼうぎょ");
        }

        public override void Update()
        {
            //落下中
            if (!_controller.IsGround())
            {
                _controller.ChangeState(_controller.Fall);
                return;
            }
            //回避入力検知
            if (_controller.ChangeAvoid())
            {
                return;
            }
            //防御解除
            if (!SInputSystem.Instance.DefenseButton || SGameManager.instance.ActivityMode != ActivityMode.Battle)
            {
                _controller.ChangeState(_controller.Idle);
                return;
            }

            //移動
            var moveValue = SInputSystem.Instance.MoveValue;
            //移動ベクトル
            var forward = _controller.CameraForward();
            //移動量計算
            _controller.MoveVector = (forward * moveValue.y + _controller.cameraTrans.right * moveValue.x) * _controller.CharData.DefenseSpeed;
            _controller.MoveVector.y = -0.01f;

            //移動と回転(ロックオン中ならロックオン対象の方向を向く)
            _controller.MoveAndRotate(_controller.target ? _controller.target.transform.position - _controller.MyTransform.position : Vector3.zero);
        }
    }
}
