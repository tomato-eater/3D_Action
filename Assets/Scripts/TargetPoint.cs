using UnityEngine;

/// <summary>
/// ロックオン出来るポイント
/// </summary>
public class TargetPoint : MonoBehaviour
{
    [SerializeField, Tooltip("ターゲットポイントの半径")] 
    float radius;

    //半径取得用
    public float Radius => radius;

}
