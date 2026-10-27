using UnityEngine;
using UnityEngine.InputSystem.XR;

/// <summary>
/// ロックオン出来るポイント
/// </summary>
public class TargetPoint : MonoBehaviour
{
    [SerializeField, Tooltip("ターゲットポイントの半径")] 
    float radius;

    //半径取得用
    public float Radius => radius;


    /// <summary>
    /// 接地判定の可視化
    /// </summary>
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
