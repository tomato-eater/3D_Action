using UnityEngine;

namespace Playable.Player
{
    /// <summary>
    /// Player長押し攻撃終了
    /// </summary>
    public class HoldAttackEnd : State
    {
        public HoldAttackEnd(PlayerController controller) : base(controller) { }

        bool endAni;


        public override void Start()
        {
            _controller.Action.Mode.ReleaseAttack();
            endAni = false;
            _controller.MoveVector = Vector3.zero;
        }

        public override void Update()
        {
            if (_controller.target)
            {
                //自分の座標
                var myPos = _controller.Action.AttackPoint.position;
                //目的地(ターゲットの座標)の座標
                var enemyPos = _controller.target.transform.position;
                //自身からターゲットの方向のベクトル
                var adjustment = Vector3.Scale(enemyPos - myPos, new Vector3(1, 0, 1)).normalized;

                //回転方向計算
                var moveVector = (enemyPos - myPos).normalized;

                _controller.MoveAndRotate(moveVector, 2.5f);
            }


            //アニメーションの経過割合・対応
            if (!endAni)
                endAni = _controller.Action.GetTransferAnime;

            if (endAni)
            {
                _controller.Action.MyAnimator.SetTrigger("EndAttack");
                _controller.ChangeState(_controller.Idle);
            }
        }

    }
}
