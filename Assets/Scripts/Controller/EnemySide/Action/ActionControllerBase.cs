using UnityEngine;
using UnityEngine.AI;

namespace Enemy
{
    /// <summary>
    /// Enemyのアクションベース
    /// </summary>
    [RequireComponent(typeof(Animator))]
    public abstract class ActionControllerBase : MonoBehaviour
    {
        [Header("Parameter")]
        [SerializeField] EnemyData _enemyData;

        [Header("Refer")]
        [SerializeField] private Transform _attackPoint;


        [Header("Component")]
        [SerializeField] Animator _animator;

        /// <summary>
        /// 読み取り専用　挙動の値を持つデータ
        /// </summary>
        public EnemyData EnemyData => _enemyData;
        /// <summary>
        /// 読み取り専用　攻撃の基準点
        /// </summary>
        public Transform AttackPoint => _attackPoint;
        /// <summary>
        /// 読み取り専用　アニメーター
        /// </summary>
        public Animator MyAnimator => _animator;

        protected NavMeshAgent _agent;

        internal int _index = -1;

        protected ActionBase _action;

        public ActionBase Action => _action;

        private void OnValidate()
        {
            _animator = GetComponent<Animator>();
        }

        /// <summary>
        /// 何を行うか選ぶ、対象者を返す
        /// </summary>
        /// <param name="agent">Agent</param>
        /// <returns>ターゲット　0:変更しない　1:Player側の誰かに変える　2:Enemy側の誰かに変える　3:自分にする</returns>
        public abstract int SelectExecution(NavMeshAgent agent);

        /// <summary>
        /// 行動する
        /// </summary>
        /// <param name="nowPos">現在の位置</param>
        /// <param name="target">標的</param>
        /// <returns>trueならIdleに移行</returns>
        public abstract bool Execution(Vector3 nowPos, TargetPoint target);
    }
}
