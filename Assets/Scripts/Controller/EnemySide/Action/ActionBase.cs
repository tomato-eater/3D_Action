using UnityEngine;
using UnityEngine.AI;

namespace Enemy
{
    /// <summary>
    /// Enemy行動基準
    /// </summary>
    public abstract class ActionBase
    {
        /// <summary>
        /// 一回目か記録
        /// </summary>
        protected bool _callAnimator = false;

        /// <summary>
        /// アニメーションの切り替え準備が整ったことを記録
        /// </summary>
        protected bool _animeEvent = false;

        protected int _actionCount = 0;

        /// <summary>
        /// アニメーションが切り替われる事を受け取る
        /// </summary>
        public void EndAnimeCall() => _animeEvent = true; 

        /// <summary>
        /// 最初の呼び出し
        /// </summary>
        public abstract void StartCall();
        /// <summary>
        /// アニメーションの再生
        /// </summary>
        /// <param name="animator">Animator</param>
        /// <param name="name">再生したいやつの名前</param>
        protected void PlayAnimator(Animator animator, string name)
        {
            animator.SetTrigger(name);
            _callAnimator = true;
        }
        /// <summary>
        /// その行動が終わった
        /// </summary>
        protected void EndAction()
        {
            _actionCount++;
            _callAnimator = false;
            _animeEvent = false;
        }

        /// <summary>
        /// 処理の呼び出し
        /// </summary>
        /// <param name="agent">Agent</param>
        /// <param name="nowPos">現在地</param>
        /// <param name="target">標的</param>
        /// <param name="animator">Animetor</param>
        /// <returns>処理の終了を伝える</returns>
        public abstract bool Call(NavMeshAgent agent, Vector3 nowPos, TargetPoint target, Animator animator);

    }

}