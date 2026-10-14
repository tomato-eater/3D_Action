
namespace Interface
{
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
        void Damage(float damage, float stopTime);
    }

    /// <summary>
    /// 吹き飛ばす為のインターフェース
    /// </summary>
    public interface IBlowAway
    {
        /// <summary>
        /// 吹き飛ばすための関数
        /// </summary>
        /// <param name="damage">与えるダメージ</param>
        void BlowAway(float damage);
    }


}