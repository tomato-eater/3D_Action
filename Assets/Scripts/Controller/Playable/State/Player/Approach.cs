using UnityEngine;

namespace Playable.Player
{
    /// <summary>
    /// Player接近
    /// </summary>
    public class Approach : State
    {
        public Approach(PlayerController controller) : base(controller) { }


        Vector3 startPos;

        bool endAni;

        public override void Start()
        {
            //接近の必要が無ければ攻撃に移行
            if(_controller.Action.Mode.AttackDistance <= 0)
            {
                _controller.ChangeState(_controller.TapAttack);
                return;
            }

            _controller.Action.Mode.Approach();

            startPos = _controller.MyTransform.position;
            endAni = false;
        }

        public override void Update()
        {
            if (!endAni)
                endAni = _controller.Action.GetTransferAnime;


            //接敵中に倒された等　ターゲットが居ない時
            if (!_controller.target)
            {
                if (endAni)
                {
                    _controller.ChangeState(_controller.TapAttack);
                    return;
                }
            }
            else
            {
                //自分の座標
                var myPos = _controller.Action.AttackPoint.position;

                //目的地(ターゲットの座標)の座標
                var enemyPos = _controller.target.transform.position;
                //自身からターゲットの方向のベクトル
                var adjustment = Vector3.Scale(enemyPos - myPos, new Vector3(1, 0, 1)).normalized;

                //調査する座標を調整
                myPos += adjustment * _controller.Controller.radius;
                enemyPos -= adjustment * _controller.target.Radius;

                //十分に近いか
                bool dis = Vector3.Distance(myPos, enemyPos) <= _controller.Action.Mode.AttackDistance;

                //移動・回転方向計算
                var moveVector = (enemyPos - myPos).normalized * _controller.CharData.ApproachSpeed;

                //移動の必要が．．．
                if (!dis) _controller.MoveVector = moveVector;   //ある
                else _controller.MoveVector = Vector3.zero;      //ない

                _controller.MoveAndRotate(moveVector, 2.5f);

                if (dis && endAni)
                {
                    _controller.ChangeState(_controller.TapAttack);
                    return;
                }
                //移動距離が限界か
                if (Vector3.Distance(startPos, _controller.MyTransform.position) >= _controller.CharData.MaxMoveDistance)
                {
                    if (endAni)
                    {
                        _controller.ChangeState(_controller.TapAttack);
                        return;
                    }
                }
            }
        }

    }
}
