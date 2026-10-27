using UnityEngine;

namespace Playable.Player
{
    /// <summary>
    /// Player回避
    /// </summary>
    public class Avoid : State
    {
        public Avoid(PlayerController controller) : base(controller) { }

        /// <summary>
        /// 開始地点
        /// </summary>
        Vector3 startPos;

        /// <summary>
        /// 移動先
        /// </summary>
        Vector3 targetPos;

        /// <summary>
        /// 回避時間経過割合
        /// </summary>
        float timer;

        /// <summary>
        /// 回避時間経過割合
        /// </summary>
        public float JudgeJustAvoid { get; private set; } = 1;

        public override void Start()
        {
            //回避入力が出来ないようにする
            SInputSystem.Instance.AvoidActive(false);

            timer = 0;
            JudgeJustAvoid = 0.01f;
            startPos = _controller.MyTransform.position;

            //移動方向と量を求める
            var moveValue = SInputSystem.Instance.MoveValue;
            if (moveValue != Vector2.zero)
            {
                var forward = _controller.CameraForward();
                targetPos = (forward * moveValue.y + _controller.cameraTrans.right * moveValue.x).normalized * _controller.CharData.AvoidDistance;
            }
            else
            {
                targetPos = -_controller.MyTransform.forward * _controller.CharData.AvoidDistance; ;
            }
            targetPos = startPos + targetPos;

            Debug.Log("かいひ");

        }

        public override void Update()
        {
            timer += _controller.ElapsedTime();

            var progress = Mathf.Clamp01(timer / _controller.CharData.AvoidTime);

            //移動割合計算
            JudgeJustAvoid = 1f - Mathf.Pow(1f - progress, 3f);

            //割合から移動先の位置を取得
            Vector3 idealPos = Vector3.Lerp(startPos, targetPos, JudgeJustAvoid);

            //差分を計算
            Vector3 moveVelocity = idealPos - _controller.MyTransform.position;
            //移動
            _controller.Controller.Move(moveVelocity);

            //移動しきったら戻す
            if (timer >= _controller.CharData.AvoidTime)
            {
                _controller.ChangeState(_controller.Idle);
                return;
            }

            //移動速度が0ならキャンセル
            var moveSpeed = new Vector3(_controller.Controller.velocity.x, 0, _controller.Controller.velocity.z).magnitude;
            if (moveSpeed == 0)
            {
                _controller.ChangeState(_controller.Idle);
                return;
            }
            ;
        }

        public override void End()
        {
            //回避入力ができるようにする
            SInputSystem.Instance.AvoidActive(true);
            JudgeJustAvoid = 1;
        }
    }
}
