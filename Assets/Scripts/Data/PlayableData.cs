using UnityEngine;

/// <summary>
/// 移動などキャラクターごとでもっ変わらないデータ
/// </summary>
[CreateAssetMenu(fileName = "PlayableData", menuName = "Data/PlayableData")]
public class PlayableData : ScriptableObject
{
    [Header("Move")]
    [SerializeField, Tooltip("歩く速度")] float walkSpeed;
    [SerializeField, Tooltip("走る速度")] float runSpeed;
    [SerializeField, Tooltip("防御速度")] float defenseSpeed;
    [SerializeField, Tooltip("変速度")] float variableSpeed;

    [Header("Rotate")]
    [SerializeField, Tooltip("回転速度")] float rotateSpeed;

    [Header("Jump / Fall")]
    [SerializeField, Tooltip("空中移動速度")] float airSpeed;
    [SerializeField, Tooltip("跳躍力")] float jumpTop;
    [SerializeField, Tooltip("重さ")] float gravityValue;
    [SerializeField, Tooltip("最高降下速度")] float maxFallSpeed;

    [Header("Approach")]
    [SerializeField, Tooltip("攻撃時の接近速度")] float approachSpeed;
    [SerializeField, Tooltip("攻撃時の接近出来る最大距離")] float maxMoveDistance;

    [Header("Avoid")]
    [SerializeField, Tooltip("回避距離")] float avoidDistance;
    [SerializeField, Tooltip("回避時間")] float avoidTime;

    [Header("JustAvoid")]
    [SerializeField, Tooltip("ジャスト回避判定の割合"), Range(0.01f, 1.0f)] float judgeAvoidPercent;
    [SerializeField, Tooltip("ジャスト回避時の時間の流れ"), Range(0.01f, 1.0f)] float slowScale;
    [SerializeField, Tooltip("スロー時間")] float slowTime;

    [Header("JustDefense")]
    [SerializeField, Tooltip("ジャスト防御判定の時間")] float judgeDefenseTime;

    /// <summary> 歩く速度 </summary>
    public float WalkSpeed => walkSpeed;

    /// <summary> 走る速度 </summary>
    public float RunSpeed => runSpeed;

    /// <summary> 防御時の移動速度 </summary>
    public float DefenseSpeed => defenseSpeed;

    /// <summary> 変速度 </summary>
    public float VariableSpeed => variableSpeed;

    /// <summary> 回転速度 </summary>
    public float RotateSpeed => rotateSpeed;

    /// <summary> 空中移動速度 </summary>
    public float AirSpeed => airSpeed;

    /// <summary> 跳躍力 </summary>
    public float JumpTop => jumpTop;

    /// <summary> 重さ </summary>
    public float GravityValue => -gravityValue;

    /// <summary> 最高降下速度 </summary>
    public float MaxFallSpeed => -maxFallSpeed;

    /// <summary> 攻撃時の接近速度 </summary>
    public float ApproachSpeed => approachSpeed;

    /// <summary> 攻撃時の接近出来る最大距離 </summary>
    public float MaxMoveDistance => maxMoveDistance;

    /// <summary> 回避距離 </summary>
    public float AvoidDistance => avoidDistance;

    /// <summary> 回避時間 </summary>
    public float AvoidTime => avoidTime;

    /// <summary> ジャスト回避判定の割合 </summary>
    public float JudgeAvoidPercent => judgeAvoidPercent;

    /// <summary> ジャスト回避時の時間の流れ </summary>
    public float SlowScale  => slowScale;

    /// <summary> スロー時間 </summary>
    public float SlowTime => slowTime;

    /// <summary> ジャスト防御判定の時間 </summary>
    public float JudgeDefenseTime => judgeDefenseTime;
}