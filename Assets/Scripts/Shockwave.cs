using System.Collections.Generic;
using UnityEngine;

public class Shockwave : MonoBehaviour
{
    /// <summary>
    /// 判定を持たせるレイヤー
    /// </summary>
    [SerializeField] LayerMask mask;
    /// <summary>
    /// 箱のサイズ(横)
    /// </summary>
    [SerializeField] float width;
    /// <summary>
    /// 箱のサイズ(縦)
    /// </summary>
    [SerializeField] float height;
    /// <summary>
    /// 速度
    /// </summary>
    [SerializeField] float speed;
    /// <summary>
    /// 生存時間
    /// </summary>
    [SerializeField] float lifeTime;

    /// <summary>
    /// 自分のコントローラー
    /// </summary>
    PlayableBehaviorController controller;
    /// <summary>
    /// １フレーム前のポジション
    /// </summary>
    Vector3 beforePos = Vector3.zero;
    /// <summary>
    /// 時間
    /// </summary>
    float timer;
    /// <summary>
    /// 既に触れたコライダー
    /// </summary>
    List<Collider> colls = new List<Collider>();

    private void OnEnable()
    {
        timer = 0;
        beforePos = transform.position;
        colls.Clear();
    }
    public void Set(PlayableBehaviorController controller) => this.controller = controller; 

    private void Update()
    {
        //経過時間
        var time = controller.ElapsedTime();
        //前方に直進
        transform.Translate(Vector3.forward * speed * time);
        timer += time;
        //寿命が来たら非表示
        if(timer >= lifeTime)
        {
            gameObject.SetActive(false);
            return;
        }

        var currentPos = transform.position;
        //中心を計算
        var center = (currentPos + beforePos) * 0.5f;
        //移動ベクトル
        var moveDis = (currentPos - beforePos).magnitude;

        //あまりに短いと次に持ち越し
        if (moveDis <= 0.001f) return;

        //箱の半分のサイズ
        var boxHalfExtents = new Vector3(width, height, moveDis * 0.5f);

        // 自分の現在の座標（中心）、サイズ、現在の回転角度を指定して敵を検知
        Collider[] colliders = Physics.OverlapBox(transform.position, boxHalfExtents, transform.rotation, mask);

        //衝撃波の判定サイズをScene画面に緑色の箱でリアルタイム表示
        DebugDrawBox(center, boxHalfExtents * 2f, transform.rotation, Color.green);

        foreach (var coll in colliders)
        {
            // まだこの衝撃波が当たっていない相手ならダメージ
            if (!colls.Contains(coll))
            {
                colls.Add(coll);

                // インターフェース経由でダメージを適用
                if (coll.TryGetComponent<Interface.IBlowAway>(out var target))
                {
                    Debug.Log(coll.name);
                }
            }
        }

        beforePos = currentPos;
    }

    // デバッグ用の箱描きメソッド
    private void DebugDrawBox(Vector3 center, Vector3 size, Quaternion rotation, Color color)
    {
        Vector3 half = size * 0.5f;
        Vector3[] v = new Vector3[8];
        v[0] = new Vector3(-half.x, -half.y, -half.z); v[1] = new Vector3(half.x, -half.y, -half.z);
        v[2] = new Vector3(half.x, half.y, -half.z); v[3] = new Vector3(-half.x, half.y, -half.z);
        v[4] = new Vector3(-half.x, -half.y, half.z); v[5] = new Vector3(half.x, -half.y, half.z);
        v[6] = new Vector3(half.x, half.y, half.z); v[7] = new Vector3(-half.x, half.y, half.z);
        for (int i = 0; i < 8; i++) v[i] = rotation * v[i] + center;
        Debug.DrawLine(v[0], v[1], color); Debug.DrawLine(v[1], v[2], color); Debug.DrawLine(v[2], v[3], color); Debug.DrawLine(v[3], v[0], color);
        Debug.DrawLine(v[4], v[5], color); Debug.DrawLine(v[5], v[6], color); Debug.DrawLine(v[6], v[7], color); Debug.DrawLine(v[7], v[4], color);
        Debug.DrawLine(v[0], v[4], color); Debug.DrawLine(v[1], v[5], color); Debug.DrawLine(v[2], v[6], color); Debug.DrawLine(v[3], v[7], color);
    }
}