using UnityEngine;

namespace Playable
{
    /// <summary>
    /// Playable行動基準
    /// </summary>
    public abstract class ActionBase : MonoBehaviour
    {
        /// <summary>
        /// アニメーター
        /// </summary>
        Animator animator;
        /// <summary>
        /// アニメーターの登録
        /// </summary>
        /// <param name="animator">アニメーター</param>
        public void Set(Animator animator) => this.animator = animator;
        /// <summary>
        /// 読み取り専用　アニメーター
        /// </summary>
        protected Animator MyAnimator => animator;

        /// <summary>
        /// 通常攻撃の最大コンボ数
        /// </summary>
        public abstract int MaxCombo {  get; }
        /// <summary>
        /// 通常攻撃に移行する敵との距離
        /// </summary>
        public abstract float AttackDistance { get; }
        /// <summary>
        /// ホールド出来る最大時間
        /// </summary>
        public abstract float MaxHoldTime { get; }
        /// <summary>
        /// ホールド中動けるか
        /// </summary>
        public abstract bool ChargeMove { get; }

        /// <summary>
        /// ホールド攻撃のホールドした時間
        /// </summary>
        protected float chargeAttackTime = 0;
        /// <summary>
        /// ホールドスキルのホールドした時間
        /// </summary>
        protected float chargeSkillTime = 0;


        /// <summary>
        /// 接近開始
        /// </summary>
        public virtual void Approach()
        {
            MyAnimator.SetTrigger("Approach");
        }

        /// <summary>
        /// 単押し攻撃
        /// </summary>
        public virtual void TapAttack()
        {
            MyAnimator.SetTrigger("TapAttack");
        }


        /// <summary>
        /// 長押し攻撃
        /// </summary>
        /// <param name="time">押してる時間</param>
        /// <returns>チャージしきったら ? 切り替える : 切り替えない</returns>
        public abstract bool HoldAttack(float time = 0);
        /// <summary>
        /// 攻撃を離した後の処理
        /// </summary>
        public abstract void ReleaseAttack();
    }
}
