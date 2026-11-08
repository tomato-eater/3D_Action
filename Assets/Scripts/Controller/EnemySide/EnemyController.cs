using UnityEngine;
using UnityEngine.AI;
using Interface;
using Cysharp.Threading.Tasks;
using System.Threading;
using System;

namespace Enemy
{
    /// <summary>
    /// EnemyのStateBase
    /// </summary>
    public abstract class State : StateBase<EnemyController>
    {
        protected State(EnemyController controller) : base(controller) { }
    }

    /// <summary>
    /// 敵のコントローラー
    /// </summary>
    public class EnemyController : BehaviorControllerBase<EnemyController, State>, IElapsedTime, IDamage
    {
        [Header("Parameter")]
        [SerializeField] Transform eyeTrans;
        [SerializeField, Tooltip("徘徊範囲倍率"), Range(0.01f, 1.0f)] float walkArea = 0.75f;
        [SerializeField, Tooltip("見える距離")] float scouting;
        [SerializeField, Tooltip("索敵視野")] float angle;

        [Header("Refer")]
        [SerializeField, Tooltip("バトルエリア")] BattleArea battleArea;
        [Tooltip("各部位")] public TargetPoint[] points;

        /// <summary>
        /// 標的の情報を見る
        /// playerSide ? Player側 : Enemy側   point　場所
        /// </summary>
        public (bool playerSide, TargetPoint point) target;   

        [Header("Component")]
        [SerializeField] NavMeshAgent agent;
        [SerializeField] ActionControllerBase _action;

        /// <summary>
        /// ヒットストップのキャンセルトークン
        /// </summary>
        CancellationTokenSource _hitStopToken;

        /// <summary>
        /// 動けない
        /// </summary>
        private bool _stop = false;

        /// <summary> アイドル </summary>
        public Idle Idle { get; private set; }
        /// <summary> 徘徊 </summary>
        public Loiter Loiter { get; private set; }

        /// <summary> 発見 </summary>
        public Discovery Discovery { get; private set; }
        /// <summary>
        /// 待機(隙)
        /// </summary>
        public Wait Wait { get; private set; }
        /// <summary>
        /// 行動の選択・実行
        /// </summary>
        public Execution Execution {  get; private set; }

        /// <summary> 死亡 </summary>
        public Death Death { get; private set; }


        public Transform MyTransform { get; private set; }
        public BattleArea Area => battleArea;
        public NavMeshAgent Agent => agent;
        /// <summary>
        /// 戦闘エリア基準で徘徊する距離の最大倍率
        /// </summary>
        public float WalkArea => walkArea;

        /// <summary>
        /// キャラクター固有の
        /// </summary>
        public ActionControllerBase Action => _action;

        /// <summary>
        /// バトル中かどうか
        /// </summary>
        private bool _battle = false;

        private void OnValidate()
        {
            agent = GetComponent<NavMeshAgent>();
        }

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            MyTransform = transform;
            if (!battleArea) MyTransform.parent.TryGetComponent<BattleArea>(out battleArea);

            target = (false, null);

            Idle = new Idle(this);
            Loiter = new Loiter(this);
            Discovery = new Discovery(this);

            Wait = new Wait(this);
            Execution = new Execution(this);

            Death = new Death(this);

            ChangeState(Idle);
        }

        protected override void Update()
        {
            base.Update();

            if (_battle)
            {

            }
        }

        private void OnDisable()
        {
            StopHitStop();
        }

        /// <summary>
        /// 発見したステートに切り替える(二重で発見しないように)
        /// </summary>
        public void BattleBegins()
        {
            if(State == Discovery || _battle) return;

            ChangeState(Discovery);
        }

        /// <summary>
        /// 行動速度取得
        /// </summary>
        /// <returns>動けない ? 0 : timeScale </returns>
        public float ElapsedTime() => _stop ? 0 : Time.deltaTime;

        public void BattleChanger(bool battle) => _battle = battle;

        /// <summary>
        /// 敵(Player)を探す
        /// </summary>
        /// <returns>敵(Player)を発見した ? true : false</returns>
        public bool Scouting()
        {
            //戦闘範囲内にいるか判定
            if (!Area.Within) return false;
            //差を取得
            var diff = Area.playerTarget.transform.position - eyeTrans.position;
            if (diff.magnitude <= scouting)
            {
                //自身から敵へのベクトル、角度を調べる
                var forward = Vector3.Scale(eyeTrans.forward, new Vector3(1, 0, 1)).normalized;
                var eAngle = Vector3.Angle(forward, diff.normalized);
                //範囲内なら
                if (eAngle < angle * 0.5f)
                {
                    target = (true, Area.playerTarget);

                    battleArea.BattleStart();
                    return true;
                }
            }
            return false;
        }


        public void Damage(float damage, float stopTime, Vector3 force)
        {
            Debug.Log("喰らった" + damage + " " + stopTime + " " + force);


            if (_hitStopToken != null) StopHitStop();
            _hitStopToken = new();
            HitStop(stopTime, _hitStopToken.Token).Forget();
            Debug.Log("とおった");
        }

        public void BlowAway(float damage, Vector3 force)
        {
            Debug.Log("吹き飛んだ!");
        }
        /// <summary>
        /// ヒットストップ中止
        /// </summary>
        private void StopHitStop()
        {
            _hitStopToken?.Cancel();
            _hitStopToken?.Dispose();
            _hitStopToken = null;
        }
        /// <summary>
        /// ヒットストップストップ
        /// </summary>
        /// <param name="stopTime">止める時間</param>
        /// <param name="token">キャンセルトークン</param>
        /// <returns>void</returns>
        async UniTaskVoid HitStop(float stopTime, CancellationToken token)
        {
            try
            {
                _stop = true;
                Action.MyAnimator.speed = 0.2f;

                await UniTask.Delay(TimeSpan.FromSeconds(stopTime), cancellationToken: token);

                _hitStopToken?.Dispose();
                _hitStopToken = null;
            }
            finally
            {
                _stop = false;
                Action.MyAnimator.speed = 1;
            }
        }










        /// <summary>
        /// 索敵範囲の可視化
        /// </summary>
        void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(eyeTrans.position, scouting);

            var forward = Vector3.Scale(eyeTrans.forward, new Vector3(1, 0, 1));
            Vector3 leftBoundary = Quaternion.Euler(0, -angle * 0.5f, 0) * forward;
            Vector3 rightBoundary = Quaternion.Euler(0, angle * 0.5f, 0) * forward;

            Gizmos.DrawLine(eyeTrans.position, eyeTrans.position + leftBoundary * angle);
            Gizmos.DrawLine(eyeTrans.position, eyeTrans.position + rightBoundary * angle);
        }

    }
}

