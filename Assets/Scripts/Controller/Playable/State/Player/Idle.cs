using UnityEngine;
using Playable;

namespace Player
{
    /// <summary>
    /// Playerアイドル
    /// </summary>
    public class Idle : State
    {
        public Idle(PlayerController controller) : base(controller) { }

        public override void Start()
        {
            controller.Action.Animator.SetFloat("MoveVelocity", 0);
            controller.moveVector = Vector3.zero;
        }

        public override void Update()
        {
            //攻撃入力検知
            if (controller.Attack(out var comboAdd))
            {
                return;
            }

            //ジャンプ入力検知
            if (controller.CheckJump())
            {
                return;
            }
            //落下した
            if (!controller.IsGround())
            {
                controller.ChangeState(controller.Fall);
                return;
            }
            //戦闘中なら
            if (SGameManager.instance.ActivityMode == ActivityMode.Battle)
            {
                //回避入力検知
                if (controller.ChangeAvoid())
                {
                    return;
                }
                //防御入力検知
                if (SInputSystem.Instance.DefenseButton)
                {
                    controller.ChangeState(controller.Defense);
                    return;
                }
            }

            //移動入力検知
            if (SInputSystem.Instance.MoveValue != Vector2.zero)
            {
                controller.ChangeState(controller.Move);
                return;
            }

            //浮かないようにする
            controller.moveVector.y = -0.1f;

            controller.MoveAndRotate(Vector3.zero);
        }
    }

}
