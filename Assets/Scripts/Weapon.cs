using System.Collections.Generic;
using UnityEngine;

namespace Playable
{
    /// <summary>
    /// 武器の当たり判定と処理
    /// </summary>
    public class Weapon : MonoBehaviour
    {
        ActionControllerBase _controller;

        [SerializeField] LayerMask _hitLayer;

        [SerializeField, Tooltip("レイを飛ばす基準")] Transform _markOutside;
        [SerializeField, Tooltip("レイを飛ばす基準")] Transform _markInside;

        /// <summary>
        /// 1フレーム前の基準_外側
        /// </summary>
        Vector3 _beforeOut;
        /// <summary>
        /// 1フレーム前の基準_内側
        /// </summary>
        Vector3 _beforeIn;

        bool _attack = false;

        List<Collider> _colls = new ();


        public void Set(ActionControllerBase i)
        {
            _controller = i;
            EndAttack();
        }

        /// <summary>
        /// 攻撃判定開始
        /// </summary>
        public void StartAttack()
        {
            _attack = true;
            _colls.Clear();

            _beforeOut = _markOutside.position;
            _beforeIn = _markInside.position;
        }
        /// <summary>
        /// 攻撃判定終了
        /// </summary>
        public void EndAttack() => _attack = false;

        // Update is called once per frame
        void LateUpdate()
        {
            if (!_attack) return;

            var currentOut = _markOutside.position;
            var currentIn = _markInside.position;
            //４頂点の中心を求める
            var center = (currentOut + currentIn + _beforeOut + _beforeIn) * 0.25f;
            //移動ベクトル
            var moveDir = currentOut - _beforeOut;
            //長さ
            var lengthDir = currentOut - currentIn;

            //あまりに短いと次に持ち越し
            if (moveDir.sqrMagnitude <= 0.001f || lengthDir.sqrMagnitude <= 0.001f) return;

            //Boxの定義
            var boxHalfExtents = new Vector3(moveDir.magnitude * 0.5f, lengthDir.magnitude * 0.5f, 0.05f);
            //Boxの回転
            var faceNormal = Vector3.Cross(moveDir, lengthDir);
            Quaternion boxRotation = Quaternion.LookRotation(faceNormal.normalized, lengthDir.normalized);

            DebugDrawBox(center, boxHalfExtents * 2f, boxRotation, Color.green);
            //判定の取得
            Collider[] colliders = Physics.OverlapBox(center, boxHalfExtents, boxRotation, _hitLayer);
            bool hitStop = false;
            foreach (var collider in colliders)
            {
                //既に判定を取得していれば飛ばす
                if (_colls.Contains(collider)) continue;

                hitStop = true;
                _colls.Add(collider);
                if (collider.TryGetComponent<Interface.IDamage>(out var target))
                {
                    target.Damage(50, 0.1f, Vector3.zero);
                    Debug.Log("ヒットストップの時間を直接指定している");
                }
            }

            //攻撃が当たっていればPlayHitStop
            if (hitStop)
            {
                _controller.StartHitStop(0.1f);
                Debug.Log("ヒットストップの時間を直接指定している");

            }
            _beforeOut = currentOut;
            _beforeIn = currentIn;
        }



        // 当たり判定の確認
        private void DebugDrawBox(Vector3 center, Vector3 size, Quaternion rotation, Color color)
        {
            Vector3 half = size * 0.5f;

            // 箱のローカル座標における 8 つの頂点を定義
            Vector3[] vertices = new Vector3[8]
            {
                new (-half.x, -half.y, -half.z),
                new ( half.x, -half.y, -half.z),
                new ( half.x,  half.y, -half.z),
                new (-half.x,  half.y, -half.z),
                new (-half.x, -half.y,  half.z),
                new ( half.x, -half.y,  half.z),
                new ( half.x,  half.y,  half.z),
                new (-half.x,  half.y,  half.z)
            };

            // 8 つの頂点に、現在の「中心（center）」と「角度（rotation）」を掛け算して世界座標に変換
            for (int i = 0; i < 8; i++)
            {
                vertices[i] = rotation * vertices[i] + center;
            }

            // 12本の辺を Debug.DrawLine で繋いで箱の形にする（1.0f 秒間画面に残す）
            float duration = 1.0f;

            // 手前の面
            Debug.DrawLine(vertices[0], vertices[1], color, duration);
            Debug.DrawLine(vertices[1], vertices[2], color, duration);
            Debug.DrawLine(vertices[2], vertices[3], color, duration);
            Debug.DrawLine(vertices[3], vertices[0], color, duration);

            // 奥の面
            Debug.DrawLine(vertices[4], vertices[5], color, duration);
            Debug.DrawLine(vertices[5], vertices[6], color, duration);
            Debug.DrawLine(vertices[6], vertices[7], color, duration);
            Debug.DrawLine(vertices[7], vertices[4], color, duration);

            // つなぎの4本
            Debug.DrawLine(vertices[0], vertices[4], color, duration);
            Debug.DrawLine(vertices[1], vertices[5], color, duration);
            Debug.DrawLine(vertices[2], vertices[6], color, duration);
            Debug.DrawLine(vertices[3], vertices[7], color, duration);
        }
    }
}
