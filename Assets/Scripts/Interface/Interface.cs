using UnityEngine;

namespace Interface
{
    /// <summary>
    /// 行動速度がTimeScaleに依存しない時がある
    /// </summary>
    public interface IElapsedTime
    {
        /// <summary>
        /// 行動取得取得
        /// </summary>
        /// <returns> 行動取得取得 </returns>
        float ElapsedTime();
    }

    /// <summary>
    /// ダメージを受けるためのインターフェース
    /// </summary>
    public interface IDamage
    {
        /// <summary>
        /// ダメージを受けるための関数
        /// </summary>
        /// <param name="damage">与えるダメージ</param>
        /// <param name="stopTime">ヒットストップの時間</param>
        /// <param name="force">力の方向</param>
        void Damage(float damage, float stopTime, Vector3 force);

        /// <summary>
        /// ダメージ + 吹き飛ばすための関数
        /// </summary>
        /// <param name="damage">与えるダメージ</param>
        /// <param name="force">力の方向</param>
        void BlowAway(float damage, Vector3 force);
    }

}
