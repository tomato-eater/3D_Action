using UnityEngine;

namespace Playable.Player
{
    /// <summary>
    /// Player移動
    /// </summary>
    public class Move : State
    {
        public Move(PlayerController controller) : base(controller) { }

        /// <summary>
        /// 現在の移動速度 
        /// </summary>
        /// 初期が2(だいたい)未満だと、初めて移動するとき違和感が出る
        float currentSpeed = 2.0f;

        /// <summary>
        /// 最後に入力した移動ベクトル
        /// </summary>
        /// 滑らかに停止したい
        Vector2 finalInput = Vector2.zero;

        public override void Update()
        {
            //攻撃入力検知\
            if (_controller.NextAction(out _))
            {
                currentSpeed = 2.0f;
                return;
            }

            //ジャンプ入力検知
            if (_controller.CheckJump())
            {
                currentSpeed = 2.0f;
                return;
            }
            //落下した
            if (!_controller.IsGround())
            {
                currentSpeed = 2.0f;
                _controller.ChangeState(_controller.Fall);
                return;
            }
            //戦闘中なら
            if (SGameManager.instance.ActivityMode == ActivityMode.Battle)
            {
                //回避入力検知
                if (_controller.ChangeAvoid())
                {
                    return;
                }
                //防御入力検知
                if (SInputSystem.Instance.DefenseButton)
                {
                    _controller.ChangeState(_controller.Defense);
                    return;
                }
            }

            //移動入力
            var moveValue = SInputSystem.Instance.MoveValue;
            //カメラの前方ベクトル
            var forward = _controller.CameraForward();

            //入力中なら
            if (moveValue.sqrMagnitude > 0.0f)
            {
                //移動ベクトルを更新
                finalInput = moveValue;
            }

            _controller.MoveVector = (forward * finalInput.y + _controller.cameraTrans.right * finalInput.x);
            //最大速度指定　バトル中 || 走り中 ? 走り速度 : 歩き速度
            var maxSpeed = SGameManager.instance.ActivityMode == ActivityMode.Battle || SInputSystem.Instance.RunButton ? _controller.CharData.RunSpeed : _controller.CharData.WalkSpeed;
            //入力状況に合わせて最高速度を変える
            maxSpeed = Mathf.Lerp(0, maxSpeed, moveValue.magnitude);
            //徐々に速度を変える
            currentSpeed = Mathf.MoveTowards(currentSpeed, maxSpeed, _controller.CharData.VariableSpeed * _controller.ElapsedTime());

            //移動ベクトルに速度を掛ける
            _controller.MoveVector *= currentSpeed;
            //浮かないようにする
            _controller.MoveVector.y = -0.1f;
            //移動と回転
            _controller.MoveAndRotate(_controller.MoveVector);

            //移動速度でアニメーションの速度を変える
            var moveVelocity = _controller.GetMoveSpeed() * Time.timeScale;
            _controller.Action.MyAnimator.SetFloat("MoveVelocity", moveVelocity);

            //停止した
            if (moveVelocity == 0)
            {
                _controller.ChangeState(_controller.Idle);
                return;
            }
        }

        public override void End()
        {
            finalInput = Vector2.zero;
        }
    }
}
