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
            if (!controller.IsGround())
            {
                controller.ChangeState(controller.Fall);
                return;
            }
            //回避入力検知
            if (controller.ChangeAvoid())
            {
                return;
            }
            //防御解除
            if (!SInputSystem.Instance.DefenseButton || SGameManager.instance.ActivityMode != ActivityMode.Battle)
            {
                controller.ChangeState(controller.Idle);
                return;
            }

            //移動
            var moveValue = SInputSystem.Instance.MoveValue;
            //移動ベクトル
            var forward = controller.CameraForward();
            //移動量計算
            controller.moveVector = (forward * moveValue.y + controller.cameraTrans.right * moveValue.x) * controller.CharData.DefenseSpeed;
            controller.moveVector.y = -0.01f;

            //移動と回転(ロックオン中ならロックオン対象の方向を向く)
            controller.MoveAndRotate(controller.target ? controller.target.transform.position - controller.MyTransform.position : Vector3.zero);
        }
    }
}
