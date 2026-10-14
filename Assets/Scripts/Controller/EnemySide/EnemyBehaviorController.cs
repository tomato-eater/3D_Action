using UnityEngine;
using UnityEngine.AI;
using Interface;

/// <summary>
/// 敵のコントローラー
/// </summary>
public class EnemyBehaviorController : BehaviorController, IDamage, IBlowAway
{
    [Header("Parameter")]
    [SerializeField] Transform eyeTrans;
    [SerializeField, Tooltip("その場にとどまる時間")] Vector2 idleTime;
    [SerializeField, Tooltip("徘徊範囲倍率"), Range(0.01f, 1.0f)] float walkArea;
    [SerializeField, Tooltip("歩く速度")] float walkSpeed;
    [SerializeField, Tooltip("見える距離")] float scouting;
    [SerializeField, Tooltip("索敵視野")] float angle;

    [Header("Refer")]
    [SerializeField, Tooltip("バトルエリア")] BattleArea battleArea;
    [Tooltip("各部位")] public TargetPoint[] points;
    [SerializeField, Tooltip("ステータス")] StatusBase status;

    [Header("Component")]
    [SerializeField] NavMeshAgent agent;


    public Enemy_Idle StateIdle { get; private set; }
    public Enemy_Loiter StateLoiter {  get; private set; }
    public Enemy_Discovery StateDiscovery { get; private set; }
    public Enemy_Death StateDeath { get; private set; }


    public Transform MyTransform { get; private set; }
    public BattleArea Area => battleArea;
    public NavMeshAgent Agent => agent;
    public Vector2 IdleTime => idleTime;
    public float WalkArea => walkArea;
    public float WalkSpeed => walkSpeed;



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        MyTransform = transform;
        if (!battleArea) MyTransform.parent.TryGetComponent<BattleArea>(out battleArea);
        if(!agent) MyTransform.TryGetComponent<NavMeshAgent>(out agent);

        StateIdle = new Enemy_Idle(this);
        StateLoiter = new Enemy_Loiter(this);
        StateDiscovery = new Enemy_Discovery(this);
        StateDeath = new Enemy_Death(this);

        ChangeState(StateIdle);
    }

    private void Update()
    {
        if (status.Hp <= 0 && state != StateDeath) Death();

        state?.Update();
    }

    /// <summary>
    /// 敵(Player)を探す
    /// </summary>
    /// <returns>敵(Player)を発見した ? true : false</returns>
    public bool Scouting()
    {
        //戦闘範囲内にいるか判定
        if(!Area.Within) return false;
        //差を取得
        var diff = Area.PlayerPos.position - eyeTrans.position;
        if(diff.magnitude <= scouting)
        {
            //自身から敵へのベクトル、角度を調べる
            var forward = Vector3.Scale(eyeTrans.forward, new Vector3(1, 0, 1)).normalized;
            var eAngle = Vector3.Angle(forward, diff.normalized);
            //範囲内なら
            if (eAngle < angle * 0.5f)
            {
                ChangeState(StateDiscovery);
                return true;
            }
        }
        return false;
    }


    public void Damage(float damage, float stopTime)
    {
        Debug.Log(damage);
        status.Damage(damage);

    }

    public void BlowAway(float damage)
    {
        Debug.Log("吹き飛んだ!");
    }

    void Death()
    {
        agent.enabled = false;

        ChangeState(StateDeath);
    }

    /// <summary>
    /// 索敵範囲の可視化
    /// </summary>
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(eyeTrans.position, scouting);

        var forward = Vector3.Scale(eyeTrans.forward, new Vector3(1, 0, 1));
        Vector3 leftBoundary = Quaternion.Euler(0, -angle * 0.5f, 0) * forward;
        Vector3 rightBoundary = Quaternion.Euler(0, angle * 0.5f, 0) * forward;

        Gizmos.DrawLine(eyeTrans.position, eyeTrans.position + leftBoundary * angle);
        Gizmos.DrawLine(eyeTrans.position, eyeTrans.position + rightBoundary * angle);
    }
}
