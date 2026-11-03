using NUnit.Framework;
using UnityEngine;

namespace Enemy
{
    /// <summary>
    /// Enemyのアクションベース
    /// </summary>
    public abstract class ActionControllerBase : MonoBehaviour
    {
        [Header("Parameter")]


        [Header("Refer")]


        [Header("Component")]
        [SerializeField] Animator _animator;


        public Animator MyAnimator => _animator;
        
        /// <summary>
        /// 誰に、何を行うか選ぶ
        /// </summary>
        public abstract void SelectAction();

        /// <summary>
        /// 行動する
        /// </summary>
        public abstract void Action();
    }
}
