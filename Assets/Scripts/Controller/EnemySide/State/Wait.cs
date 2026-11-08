using UnityEngine;

namespace Enemy
{
    /// <summary>
    /// Enemy攻撃前の隙
    /// </summary>
    public class Wait : State
    {
        public Wait(EnemyController controller) : base(controller) { }

        float _waitTime = 0;
        float _nowTime = 0;

        bool _repetition = false;

        public override void Start()
        {
            _controller.Agent.isStopped = false;

            //うろつく時間
            _nowTime = 0;
            var waitTime = _controller.Action.EnemyData.WaitTime;
            _waitTime = Random.Range(waitTime.x, waitTime.y);

            //移動先
            var moveRange = _controller.Action.EnemyData.MoveRange;
            var moveVector = Random.insideUnitCircle.normalized * Random.Range(moveRange.x, moveRange.y);
            var targetPos = _controller.MyTransform.position + new Vector3(moveVector.x, 0, moveVector.y);
            _controller.Agent.SetDestination(targetPos);

            //速度
            _controller.Agent.speed = _controller.Action.EnemyData.MoveSpeed * _controller.Action.EnemyData.MoveMag;
            _controller.Agent.acceleration = 8;//初期値(8)に
        }

        public override void Update()
        {
            //時間で移行
            _nowTime += Time.deltaTime;
            if( _nowTime >= _waitTime )
            {
                //2回目ではない && 70%以上なら繰り返し
                if (!_repetition && Random.value >= 0.3f)
                {
                    Start();
                }
                else
                {
                    _controller.ChangeState(_controller.Execution);
                }
            }

            _repetition = true;

            //向きの制御
            if (_controller.target.point == null) return;
            var myTrans = _controller.MyTransform;
            var targetPos = _controller.target.point.transform.position;

            var lookDirection = Vector3.Scale( targetPos - myTrans.position, new Vector3(1, 0, 1)).normalized;

            if(lookDirection.sqrMagnitude > 0.001f)
            {
                _controller.MyTransform.rotation = Quaternion.Slerp(myTrans.rotation, Quaternion.LookRotation(lookDirection), 50.0f * Time.deltaTime);

            }

            //移動している速度を取得
            var moveSpeed = _controller.Agent.velocity;

            //現在の向きから移動方向を求める
            var localVelocity = _controller.transform.InverseTransformDirection(moveSpeed).normalized;

            _controller.Action.MyAnimator.SetFloat("MoveVelocity", moveSpeed.magnitude / _controller.Action.EnemyData.MoveMag);
            _controller.Action.MyAnimator.SetFloat("MoveVectorX", localVelocity.x);
            _controller.Action.MyAnimator.SetFloat("MoveVectorY", localVelocity.z);


        }

        public override void End()
        {
            _controller.Agent.updateRotation = true;
            _controller.Agent.isStopped = false;
            _repetition = false;
        }
    }
}

