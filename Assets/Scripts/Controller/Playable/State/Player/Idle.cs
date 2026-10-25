using UnityEngine;

namespace Playable.Player
{
    /// <summary>
    /// Playerアイドル
    /// </summary>
    public class Idle : State
    {
        public Idle(PlayerController controller) : base(controller) { }

        public override void Start()
        {
            _controller.Action.MyAnimator.SetFloat("MoveVelocity", 0);
            _controller.MoveVector = Vector3.zero;
        }

        public override void Update()
        {
            //攻撃入力検知
            if (_controller.NextAction(out _))
            {
                return;
            }

            //ジャンプ入力検知
            if (_controller.CheckJump())
            {
                return;
            }
            //落下した
            if (!_controller.IsGround())
            {
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

            //移動入力検知
            if (SInputSystem.Instance.MoveValue != Vector2.zero)
            {
                _controller.ChangeState(_controller.Move);
                return;
            }

            //浮かないようにする
            _controller.MoveVector.y = -0.1f;

            _controller.MoveAndRotate(Vector3.zero);
        }
    }

}
