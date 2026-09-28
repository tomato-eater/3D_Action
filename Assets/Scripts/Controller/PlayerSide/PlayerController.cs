using UnityEngine;

/// <summary>
/// プレイヤー操作
/// </summary>
[RequireComponent(typeof(CharacterController))]
public class PlayerController : Controller
{
    public int attack = 0;

    [Header("Parameter")]
    [SerializeField] PlayableData charData;
    [SerializeField] Transform attackPoint;
    [SerializeField] float judgeAttackDistance;

    [Header("Refer")]
    [SerializeField] LayerMask groundMask;
    public Transform cameraTrans;
    public TargetPoint target;

    [Header("Component")]
    [SerializeField] CharacterController controller;
    [SerializeField] Animator animator;

    public Player_Idle StateIdle {  get; private set; }
    public Player_Move StateMove { get; private set; }
    public Player_Jump StateJump {  get; private set; }
    public Player_Fall StateFall {  get; private set; }
    public Player_Approach StateApproach {  get; private set; }
    public Player_Attack StateAttack { get; private set; }
    public Player_Defense StateDefense {  get; private set; }
    public Player_Avoid StateAvoid { get; private set; }

    /// <summary>
    /// 自分のTransform
    /// </summary>
    public Transform MyTransform { get; private set; }
    /// <summary>
    /// 移動量
    /// </summary>
    public Vector3 moveVector;

    public PlayableData CharData => charData;
    public float JudgeAttackDistance => judgeAttackDistance;
    public CharacterController Controller => controller;
    public Animator Animator => animator;
    public Transform AttackPoint => attackPoint;

    public bool IsBattle { get; private set; } = false;

    float attackJudgeTimer = 0;
    bool jumpTrigger = true;
    bool avoidTrigger = true;

    bool justSlowTrigger = false;
    float slowTimer = 0;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        MyTransform = transform;

        StateIdle = new Player_Idle(this);
        StateMove = new Player_Move(this);
        StateJump = new Player_Jump(this);
        StateFall = new Player_Fall(this);
        StateApproach = new Player_Approach(this);
        StateAttack = new Player_Attack(this);
        StateDefense = new Player_Defense(this);
        StateAvoid = new Player_Avoid(this);

        ChangeState(StateIdle);
    }

    protected override void Update()
    {
        IsBattle = SGameManager.instance.ActivityMode == ActivityMode.Battle;
        base.Update();

        if (justSlowTrigger)
        {
            NowSlow();
        }
    }

    /// <summary>
    /// 経過時間取得
    /// </summary>
    /// <returns>プレイヤー以外スロー ? timeScaleの影響を受けない : timeScaleの影響を受ける </returns>
    public float ElapsedTime()
    {
        return justSlowTrigger ? Time.unscaledDeltaTime : Time.deltaTime;
    }

    /// <summary>
    /// 着地判定
    /// </summary>
    /// <returns>着地している ? true : false</returns>
    public bool IsGround()
    {
        return Physics.CheckSphere(MyTransform.position, Controller.radius, groundMask);
    }

    /// <summary>
    /// ジャンプの判断
    /// </summary>
    /// <returns>ジャンプの判断</returns>
    public bool CheckJump()
    {
        if (!SInputSystem.instance.JumpButton)
        {
            jumpTrigger = true;
        }
        else if (jumpTrigger)
        {
            jumpTrigger = false;
            ChangeState(StateJump);
            return true;
        }
        return false;
    }

    /// <summary>
    /// 攻撃の判断　いま、空中で範囲攻撃できてしまったりしてる
    /// </summary>
    /// <returns>攻撃へ移行したか</returns>
    public bool Attack()
    {
        if (SInputSystem.instance.AttackButton)
        {
            attackJudgeTimer += Time.unscaledDeltaTime;

            //長押し攻撃
            if (attackJudgeTimer >= CharData.JudgeAttackTime)
            {
                attackJudgeTimer = 0;
                //////////////////////////////////////////長押ししたときの攻撃ステートに切り替わるようにする
                return true;
            }
        }
        else
        {
            //通常攻撃
            if (attackJudgeTimer != 0) 
            {
                attackJudgeTimer = 0;
                ChangeState(StateApproach);
                return true;
            }
        }
        return false;
    }
    /// <summary>
    /// 攻撃判定のリセット
    /// </summary>
    public void AttackReset()
    {
        attackJudgeTimer = 0;
    }

    /// <summary>
    /// 回避の判断
    /// </summary>
    /// <returns>回避へ移行したか</returns>
    public bool Avoid()
    {
        if(!SInputSystem.instance.AvoidButton)
        {
            avoidTrigger = true;
        }
        else if (avoidTrigger)
        {
            avoidTrigger = false;
            ChangeState(StateAvoid);
            return true;
        }
        return false;
    }

    /// <summary>
    /// カメラの前方座標を取得
    /// </summary>
    /// <returns>カメラの前方座標</returns>
    public Vector3 CameraForward()
    {
        return Vector3.Scale(cameraTrans.forward, new Vector3(1, 0, 1)).normalized;
    }
    
    /// <summary>
    /// 移動と回転
    /// </summary>
    /// <param name="rot">体を向ける方向</param>
    public void MoveAndRotate(Vector3 rot)
    {
        Controller.Move(moveVector * ElapsedTime());

        rot.Normalize();
        var moveForward = Vector3.Scale(rot, new Vector3(1, 0, 1));
        if (moveForward.sqrMagnitude > 0.001f)
        {
            MyTransform.rotation = Quaternion.Slerp(MyTransform.rotation, Quaternion.LookRotation(moveForward), CharData.RotateSpeed * ElapsedTime());
        }
    }
    /// <summary>
    /// 水平面の移動速度を取得
    /// </summary>
    /// <returns>水平面の移動速度</returns>
    public float GetMoveSpeed()
    {
         return new Vector2(Controller.velocity.x, Controller.velocity.z).magnitude;
    }

    /// <summary>
    /// コライダーが触れたら(攻撃を喰らったら)通る
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter(Collider other)
    {
        if (StateAvoid.judgeJustAvoid <= CharData.JudgeAvoidPercent)
        {
            StartSlow();
        }
        else
        {
            Debug.Log("hit");
        }
    }

    /// <summary>
    /// ジャスト回避成功　スローになる
    /// </summary>
    void StartSlow()
    {
        justSlowTrigger = true;
        slowTimer = CharData.SlowTime;

        Time.timeScale = CharData.SlowScale;
        Animator.updateMode = AnimatorUpdateMode.UnscaledTime;
    }

    /// <summary>
    /// ジャスト回避のスロー中
    /// </summary>
    void NowSlow()
    {
        if(slowTimer <= 0)
        {
            justSlowTrigger = false;

            Time.timeScale = 1.0f;
            Animator.updateMode = AnimatorUpdateMode.Normal;
        }
        slowTimer -= Time.unscaledDeltaTime;
    }




    /// <summary>
    /// 接地判定の可視化
    /// </summary>
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(transform.position, Controller.radius);
    }
}
