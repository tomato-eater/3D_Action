using UnityEngine;

/// <summary>
/// Playableキャラの攻撃やスキルの基盤
/// </summary>
public abstract class PlayableIndividualController : MonoBehaviour
{
    /// <summary>
    /// 現在のコントローラー
    /// </summary>
    protected PlayableBehaviorController controller;

    /// <summary>
    /// コントローラーの登録
    /// </summary>
    /// <param name="player"></param>
    public virtual void Set(PlayableBehaviorController player) => controller = player;

    /// <summary>
    /// 接近する ? true : false
    /// </summary>
    public abstract bool Approach {  get; }

    /// <summary>
    /// 長押し、離して発動 ? true : false
    /// </summary>
    public abstract bool Hold {  get; }

    /// <summary>
    /// 接近時
    /// </summary>
    public abstract void ApproachMove();

    /// <summary>
    /// 短押し攻撃
    /// </summary>
    public abstract void TapAttack();

    /// <summary>
    /// 長押し攻撃
    /// </summary>
    public abstract void HoldAttack();

    /// <summary>
    /// 長押しから離した
    /// </summary>
    public abstract void HoldRelease();

    /// <summary>
    /// スキル
    /// </summary>
    public abstract void Skill();
}
