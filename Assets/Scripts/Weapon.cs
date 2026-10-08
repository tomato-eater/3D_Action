using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField] LayerMask enemyLayer;

    [SerializeField, Tooltip("レイを飛ばす基準")] List<Transform> mark = new List<Transform>();

    List<Vector3> beforeMark = new List<Vector3>();

    bool attack = false;

    List<Collider> enemyColl = new List<Collider>();

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        while (mark.Count > beforeMark.Count) beforeMark.Add(Vector3.zero);
        EndAttack();
    }

    /// <summary>
    /// 攻撃判定開始
    /// </summary>
    public void StartAttack()
    {
        attack = true;
        enemyColl.Clear();
        for (int i = 0; i < mark.Count; i++)
            beforeMark[i] = mark[i].position;
    }
    /// <summary>
    /// 攻撃判定終了
    /// </summary>
    public void EndAttack() => attack = false;

    // Update is called once per frame
    void LateUpdate()
    {
        if (!attack) return;

        for (int i = 0; i < mark.Count; i++) 
        {
            var pos = mark[i].position;

            var direction = beforeMark[i] - pos;
            var distance = direction.magnitude;
            if (distance <= 0.001f) continue;

            if (Physics.Raycast(beforeMark[i], direction.normalized, out var hit, distance, enemyLayer))
            {
                var collider = hit.collider;

                if (!enemyColl.Contains(collider))
                {
                    //enemyColl.Add(collider);
                    Debug.Log("hit");

                }
            }

            beforeMark[i] = pos;
        }


    }
}
/*
 private void TraceRay(Vector3 start, Vector3 end)
{
    Vector3 direction = end - start;
    float distance = direction.magnitude;

    if (distance <= 0.001f) return;

    // ★【修正の肝】線の代わりに「目に見えない直方体（ボックス）」を滑らせてぶつける！
    // 剣の刃の厚み（横幅）や肉厚をここで指定します（例: 幅10cm、高さ10cmの箱）
    Vector3 boxExtents = new Vector3(0.1f, 0.1f, 0.1f); 
    
    // 箱の向きを移動方向に合わせる
    Quaternion boxRotation = Quaternion.LookRotation(direction.normalized);

    // デバッグ用の赤い線はそのまま残して確認しやすくします
    Debug.DrawLine(start, end, Color.red, 1.0f);

    // Physics.Raycast を Physics.BoxCast に変更
    if (Physics.BoxCast(start, boxExtents, direction.normalized, out RaycastHit hit, boxRotation, distance, enemyLayer))
    {
        Collider enemyCollider = hit.collider;
        if (!alreadyHitEnemies.Contains(enemyCollider))
        {
            alreadyHitEnemies.Add(enemyCollider);
            if (enemyCollider.TryGetComponent<EnemyController>(out var enemy))
            {
                enemy.TakeDamage(10);
            }
        }
    }
}

 
 */