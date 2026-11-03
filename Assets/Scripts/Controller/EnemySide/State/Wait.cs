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
            //うろつく時間
            _waitTime = Random.Range(2.0f, 3.05f);
            _nowTime = 0;

            //移動先
            var moveVector = Random.insideUnitCircle.normalized * Random.Range(1.5f, 2.5f);
            var targetPos = _controller.MyTransform.position + new Vector3(moveVector.x, 0, moveVector.y);
            _controller.Agent.SetDestination(targetPos);

            //速度
            _controller.Agent.speed = _controller.WalkSpeed * 0.1f;
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
                    //_controller.ChangeState()
                }
            }

            //向きの制御
            if (_controller.target == null) return;
            var myTrans = _controller.MyTransform;
            var targetPos = _controller.target.transform.position;

            var lookDirection = Vector3.Scale( targetPos - myTrans.position, new Vector3(1, 0, 1)).normalized;

            if(lookDirection.sqrMagnitude > 0.001f)
            {
                _controller.MyTransform.rotation = Quaternion.Slerp(myTrans.rotation, Quaternion.LookRotation(lookDirection), 50.0f * Time.deltaTime);

            }

            //移動している速度を取得
            var moveSpeed = _controller.Agent.velocity;

            //現在の向きから移動方向を求める
            var localVelocity = _controller.transform.InverseTransformDirection(moveSpeed).normalized;

            _controller.Action.MyAnimator.SetFloat("MoveVelocity", moveSpeed.magnitude * 10);
            _controller.Action.MyAnimator.SetFloat("MoveVectorX", localVelocity.x);
            _controller.Action.MyAnimator.SetFloat("MoveVectorY", localVelocity.z);


        }

        public override void End()
        {
            _repetition = false;
        }
    }
}

