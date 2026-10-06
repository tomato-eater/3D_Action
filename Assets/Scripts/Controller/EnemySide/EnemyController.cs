using System.Reflection;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// “G‚ÌƒRƒ“ƒgƒ[ƒ‰[
/// </summary>
public class EnemyController : BehaviorController
{
    [Header("Parameter")]
    [SerializeField] Transform eyeTrans;
    [SerializeField, Tooltip("‚»‚Ìê‚É‚Æ‚Ç‚Ü‚éŠÔ")] Vector2 idleTime;
    [SerializeField, Tooltip("œpœj”ÍˆÍ”{—¦"), Range(0.01f, 1.0f)] float walkArea;
    [SerializeField, Tooltip("•à‚­‘¬“x")] float walkSpeed;
    [SerializeField, Tooltip("Œ©‚¦‚é‹——£")] float scouting;
    [SerializeField, Tooltip("õ“G‹–ì")] float angle;

    [Header("Refer")]
    [SerializeField, Tooltip("ƒoƒgƒ‹ƒGƒŠƒA")] BattleArea battleArea;
    [Tooltip("Še•”ˆÊ")] public TargetPoint[] points;

    [Header("Component")]
    [SerializeField] NavMeshAgent agent;


    public Enemy_Idle StateIdle { get; private set; }
    public Enemy_Loiter StateLoiter {  get; private set; }
    public Enemy_Discovery StateDiscovery { get; private set; }


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

        ChangeState(StateIdle);
    }

    private void Update()
    {
        state?.Update();
    }

    /// <summary>
    /// “G(Player)‚ğ’T‚·
    /// </summary>
    /// <returns>“G(Player)‚ğ”­Œ©‚µ‚½ ? true : false</returns>
    public bool Scouting()
    {
        //í“¬”ÍˆÍ“à‚É‚¢‚é‚©”»’è
        if(!Area.Within) return false;
        //·‚ğæ“¾
        var diff = Area.PlayerPos.position - eyeTrans.position;
        if(diff.magnitude <= scouting)
        {
            //©g‚©‚ç“G‚Ö‚ÌƒxƒNƒgƒ‹AŠp“x‚ğ’²‚×‚é
            var forward = Vector3.Scale(eyeTrans.forward, new Vector3(1, 0, 1)).normalized;
            var eAngle = Vector3.Angle(forward, diff.normalized);
            //”ÍˆÍ“à‚È‚ç
            if (eAngle < angle * 0.5f)
            {
                ChangeState(StateDiscovery);
                return true;
            }
        }
        return false;
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Hit");
    }


    /// <summary>
    /// õ“G”ÍˆÍ‚Ì‰Â‹‰»
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
