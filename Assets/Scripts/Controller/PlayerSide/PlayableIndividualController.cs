using UnityEngine;

/// <summary>
/// Playableキャラの攻撃やスキルの基盤
/// </summary>
public abstract class PlayableIndividualController
{
    /// <summary>
    /// 攻撃時の接近ステート
    /// </summary>
    protected Player_Approach StateApproach;
    /// <summary>
    /// 単攻撃ステート
    /// </summary>
    protected Player_TapAttack StateTapAttack;


    /// <summary>
    /// コントローラー、各ステートの登録
    /// </summary>
    /// <param name="player"></param>
    public abstract void Set(PlayableBehaviorController player);


}
