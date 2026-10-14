using UnityEngine;

public class Impact : MonoBehaviour
{
    /// <summary>
    /// 距離
    /// </summary>
    [SerializeField] float radius;
    /// <summary>
    /// 角度
    /// </summary>
    [SerializeField] float angle;
    /// <summary>
    /// 判定を持たせるレイヤー
    /// </summary>
    [SerializeField] LayerMask mask;

    private void OnEnable()
    {
        //範囲内で索敵
        Collider[] hit = Physics.OverlapSphere(transform.position, radius, mask);

        foreach (var coll in hit)
        {
            //自身から敵へのベクトル、角度を調べる
            var dir = (coll.transform.position - transform.position).normalized;
            var eAngle = Vector3.Angle(transform.forward, dir);
            //範囲内なら
            if (eAngle < angle * 0.5f)
            {
                if(coll.TryGetComponent<Interface.IBlowAway>(out var target))
                {
                    target.BlowAway(0);
                }
            }
        }
    }

    /// <summary>
    /// 索敵範囲の可視化
    /// </summary>
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, radius);

        Vector3 leftBoundary = Quaternion.Euler(0, -angle * 0.5f, 0) * transform.forward;
        Vector3 rightBoundary = Quaternion.Euler(0, angle * 0.5f, 0) * transform.forward;

        Gizmos.DrawLine(transform.position, transform.position + leftBoundary * angle);
        Gizmos.DrawLine(transform.position, transform.position + rightBoundary * angle);
    }
}
