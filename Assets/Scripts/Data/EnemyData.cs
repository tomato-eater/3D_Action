using UnityEngine;

/// <summary>
/// Enemyごとで変わるデータ(移動速度等)
/// </summary>
[CreateAssetMenu(fileName = "EnemyData", menuName = "Data/EnemyData")]
public class EnemyData : ScriptableObject
{
    [SerializeField, Tooltip("その場に留まる時間(s)"), Min(0)] private Vector2 _idleTime;
    [SerializeField, Tooltip("移動速度")] private float _moveSpeed;
    [SerializeField, Tooltip("警戒時間(s)")] private Vector2 _waitTime;
    [SerializeField, Tooltip("警戒時の移動距離")] private Vector2 _moveRange;
    [SerializeField, Tooltip("警戒時の速度倍率")] private float _moveMag;



    /// <summary>
    /// 待機時間 最小_最大
    /// </summary>
    public Vector2 IdleTime => _idleTime;
    /// <summary>
    /// 移動速度
    /// </summary>
    public float MoveSpeed => _moveSpeed;
    /// <summary>
    /// 警戒時の攻撃前の隙時間
    /// </summary>
    public Vector2 WaitTime => _waitTime;
    /// <summary>
    /// 警戒時の移動距離　最短_最長
    /// </summary>
    public Vector2 MoveRange => _moveRange;
    /// <summary>
    /// 警戒時の移動速度倍率(移動速度が基準)
    /// </summary>
    public float MoveMag => _moveMag;
}
