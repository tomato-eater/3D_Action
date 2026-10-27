using UnityEngine;

namespace Enemy
{
    /// <summary>
    /// Enemyのアクションベース
    /// </summary>
    public class ActionControllerBase : MonoBehaviour
    {
        [Header("Parameter")]


        [Header("Refer")]


        [Header("Component")]
        [SerializeField] Animator _animator;


        public Animator MyAnimator => _animator;

    }
}
