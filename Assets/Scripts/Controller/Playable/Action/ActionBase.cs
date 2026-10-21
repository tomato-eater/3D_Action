using UnityEngine;

namespace Playable
{
    /// <summary>
    /// Playable行動基準
    /// </summary>
    public abstract class ActionBase : MonoBehaviour
    {
        /// <summary>
        /// 通常攻撃時のコンボ数
        /// </summary>
        public abstract int MaxCombo {  get; }
        /// <summary>
        /// 通常攻撃に移行する敵との距離
        /// </summary>
        public abstract float AttackDistance { get; }
        /// <summary>
        /// チャージ出来る最大時間
        /// </summary>
        public abstract float MaxCharge { get; }
        /// <summary>
        /// チャージ中動けるか
        /// </summary>
        public abstract bool ChargeMove { get; }

        protected float chargeTime;

        /// <summary>
        /// 単押し攻撃:開始
        /// </summary>
        /// <param name="weapon">武器にアクセス</param>
        public abstract void TapAttackS(Weapon weapon = null);
        /// <summary>
        /// 単押し攻撃:終了
        /// </summary>
        /// <param name="weapon">武器にアクセス</param>
        public abstract void TapAttackE(Weapon weapon = null);


        /// <summary>
        /// 長押し攻撃
        /// </summary>
        /// <param name="time">押してる時間</param>
        /// <returns>チャージしきったら ? 切り替える : 切り替えない</returns>
        public abstract bool HoldAttack(float time);
        /// <summary>
        /// 離した(アニメーターで制御)
        /// </summary>
        public abstract void ReleaseAttack();


        public abstract void Skill();
    }
}

