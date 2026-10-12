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

    /// <summary>
    /// 自分自身のコントローラー(親)
    /// </summary>
    EnemyBehaviorController enemyController;

    private void Start()
    {
        transform.parent.TryGetComponent<EnemyBehaviorController>(out enemyController);
    }

    public void Damage(float damage)
    {
        Debug.Log(damage);
    }
}
