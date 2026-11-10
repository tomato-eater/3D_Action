using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Enemy.First
{
    /// <summary>
    /// Firstのアクション
    /// </summary>
    public class ActionController : ActionControllerBase
    {
        NormalAttack _normalAttack = new();

        [Header("Weapon")]
        bool _weapon = false;
        [SerializeField] Transform _markOutside;
        [SerializeField] Transform _markInside;
        (Vector3 _out, Vector3 _in) _beforeWeapon;
        List<Collider> _weaponHit = new ();


        public override int SelectExecution(NavMeshAgent agent)
        {
            _agent = agent;
            _action = _normalAttack;

            _action.StartCall();
            return 0;
        }

        public override bool Execution(Vector3 nowPos, TargetPoint target)
        {
            return _action.Call(_agent, nowPos, target, MyAnimator);
        }

        private void LateUpdate()
        {
            //武器の判定
            if (_weapon) Weapon();
        }

        public void OnWeapon()
        {
            _weapon = true;

            _beforeWeapon = (_markOutside.position,  _markInside.position);

            _weaponHit.Clear();
        }

        private void Weapon()
        {
            //現在地
            (Vector3 _out, Vector3 _in) current = (_markOutside.position, _markInside.position);
            //中心
            var center = (current._out + current._in + _beforeWeapon._out + _beforeWeapon._in) * 0.25f;
            //移動ベクトル
            var moveDir = current._out - _beforeWeapon._out;
            //長さ
            var lengthDir = current._out - current._in;

            //あまりに短いと次に持ち越し
            if (moveDir.sqrMagnitude <= 0.001f || lengthDir.sqrMagnitude <= 0.001f) return;

            //Boxの定義
            var boxHalfExtents = new Vector3(moveDir.magnitude * 0.5f, lengthDir.magnitude * 0.5f, 0.05f);
            //Boxの回転
            var faceNormal = Vector3.Cross(moveDir, lengthDir);
            Quaternion boxRotation = Quaternion.LookRotation(faceNormal.normalized, lengthDir.normalized);

            DebugDrawBox(center, boxHalfExtents * 2f, boxRotation, Color.green);
            //判定の取得
            Collider[] colliders = Physics.OverlapBox(center, boxHalfExtents, boxRotation, _hitMask);
            foreach (var collider in colliders)
            {
                //既に判定を取得していれば飛ばす
                if (_weaponHit.Contains(collider)) continue;

                _weaponHit.Add(collider);
                if (collider.TryGetComponent<Interface.IDamage>(out var target))
                {
                    target.Damage(50, 0, 0, transform.position);
                    Debug.Log("ダメージの値を直接指定している");
                }
            }

            _beforeWeapon = current;

        }

        public void OffWeapon() => _weapon = false;

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

    /// <summary>
    /// 通常攻撃・(接近して剣を振る)*1
    /// </summary>
    class NormalAttack : ActionBase
    {
        public override void StartCall()
        {
            _actionCount = 0;
            _callAnimator = false;
        }

        public override bool Call(NavMeshAgent agent, Vector3 nowPos, TargetPoint target, Animator animator)
        {
            switch (_actionCount)
            {
                case 0://接近
                    //初回の呼び出し
                    if (!_callAnimator)
                    {
                        PlayAnimator(animator, "Approach_1");

                        agent.speed = 20;
                        agent.acceleration = 50;
                        agent.isStopped = false;
                    }

                    //目的地
                    var targetPos = target.transform.position;
                    //目標ベクトル
                    var adjustment = Vector3.Scale(targetPos - nowPos, new Vector3(1, 0, 1)).normalized;

                    //微調整
                    var myPos = nowPos + adjustment * agent.radius;
                    targetPos -= adjustment * target.Radius;

                    //移動先の指定
                    agent.SetDestination(targetPos);

                    //十分な近さか
                    bool dis = Vector3.Distance(myPos, targetPos) <= 1.5f;

                    if(dis)
                    {
                        agent.isStopped = true;
                    }    
                    else
                    {
                        agent.isStopped = false;
                    }

                    //用意したアニメーターが再生仕切った && 十分な距離 of 速度が0 なら
                    if(_animeEvent)
                    {
                        EndAction();
                    }
                    break;

                case 1://剣を振った
                    //初回の呼び出し
                    if (!_callAnimator)
                    {
                        PlayAnimator(animator, "Attack_1");
                        agent.isStopped = true;
                    }
                    if (_animeEvent)
                    {
                        EndAction();
                    }
                    break;

                case 2://攻撃を終えた
                    PlayAnimator(animator, "EndAttack");
                    return true;

                default:
                    Debug.LogError("通る予定なし");
                    break;
            }
            return false;
        }


    }
}
