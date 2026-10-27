using Interface;
using UnityEngine;

namespace Playable.Player
{
    /// <summary>
    /// PlayerのStateBase
    /// </summary>
    public abstract class State : StateBase<PlayerController>
    {
        protected State(PlayerController controller) : base(controller) { }
    }

    /// <summary>
    /// プレイヤー操作
    /// </summary>
    [RequireComponent(typeof(CharacterController))]
    public class PlayerController : BehaviorControllerBase<PlayerController, State>, IElapsedTime, IDamage
    {
        [Header("Parameter")]
        [SerializeField] PlayableData charData;

        [Header("Refer")]
        [SerializeField] LayerMask groundMask;
        public Transform cameraTrans;
        public TargetPoint target;

        [Header("Component")]
        [SerializeField] CharacterController controller;
        [SerializeField] ActionControllerBase action;


        /// <summary> アイドル </summary>
        public Idle Idle { get; private set; }
        /// <summary> 移動 </summary>
        public Move Move { get; private set; }
        /// <summary> ジャンプ </summary>
        public Jump Jump { get; private set; }
        /// <summary> 落下 </summary>
        public Fall Fall { get; private set; }

        /// <summary> 接近 </summary>
        public Approach Approach { get; private set; }
        /// <summary> 単押し攻撃 </summary>
        public TapAttack TapAttack { get; private set; }
        /// <summary> 長押し攻撃開始 </summary>
        public HoldAttackStart HoldAttackB { get; private set; }
        /// <summary> 長押し攻撃終了 </summary>
        public HoldAttackEnd HoldAttackE { get; private set; }
        /// <summary> 固有技 </summary>
        public Unique Unique { get; private set; }

        /// <summary> 防御 </summary>
        public Defense Defense { get; private set; }
        /// <summary> 回避 </summary>
        public Avoid Avoid { get; private set; }


        /// <summary>
        /// 自分のTransform
        /// </summary>
        public Transform MyTransform { get; private set; }
        /// <summary>
        /// 移動量
        /// </summary>
        public Vector3 MoveVector;

        /// <summary> キャラクターの共通のパラメーター </summary>
        public PlayableData CharData => charData;
        /// <summary> キャラクターコントローラー </summary>
        public CharacterController Controller => controller;
        /// <summary> キャラクター固有のパラメーター </summary>
        public ActionControllerBase Action => action;

        /// <summary>
        /// ジャスト回避時の残りスロー時間
        /// </summary>
        float slowTimer = 0;



        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            MyTransform = transform;

            Idle = new Idle(this);
            Move = new Move(this);
            Jump = new Jump(this);
            Fall = new Fall(this);

            Approach = new Approach(this);
            TapAttack = new TapAttack(this);
            HoldAttackB = new HoldAttackStart(this);
            HoldAttackE = new HoldAttackEnd(this);

            Unique = new Unique(this);

            Defense = new Defense(this);
            Avoid = new Avoid(this);

            Action.EquipWeapon();
            Action.Set(this);

            ChangeState(Idle);
        }

        protected override void Update()
        {
            base.Update();

            if (slowTimer != 0)
            {
                NowSlow();
            }
        }

        /// <summary>
        /// 繰り出す技の判断
        /// </summary>
        /// <param name="comboAdd">コンボは途切れてない</param>
        /// <returns>Stateが切り替わった ? 替わった : 替わってない</returns>
        public bool NextAction(out bool comboAdd)
        {
            comboAdd = false;
            //固有技
            if (SInputSystem.Instance.UniqueTrigger)
            {
                ChangeState(Unique);
                return true;
            }
            //単押し
            if (SInputSystem.Instance.TapAttackTrigger)
            {
                comboAdd = true;
                ChangeState(Approach);
                return true;
            }
            //長押し
            if (SInputSystem.Instance.HoldAttack && IsGround())
            {
                ChangeState(HoldAttackB);
                return true;
            }

            return false;
        }

        /// <summary>
        /// 行動速度取得
        /// </summary>
        /// <returns>プレイヤー以外スロー ? timeScaleの影響を受けない : timeScaleの影響を受ける </returns>
        public float ElapsedTime() => slowTimer > 0 ? Time.unscaledDeltaTime : Time.deltaTime;

        /// <summary>
        /// 着地判定
        /// </summary>
        /// <returns>着地している ? true : false</returns>
        public bool IsGround() => Physics.CheckSphere(MyTransform.position, Controller.radius * 0.5f, groundMask);

        /// <summary>
        /// ジャンプの判断
        /// </summary>
        /// <returns>ジャンプの判断</returns>
        public bool CheckJump()
        {
            if (SInputSystem.Instance.JumpTrigger)
            {
                ChangeState(Jump);
                return true;
            }
            return false;
        }

        /// <summary>
        /// 回避の判断
        /// </summary>
        /// <returns>回避へ移行したか</returns>
        public bool ChangeAvoid()
        {
            if (SInputSystem.Instance.TapAvoidTrigger)
            {
                ChangeState(Avoid);
                return true;
            }
            return false;
        }

        /// <summary>
        /// カメラの前方座標を取得
        /// </summary>
        /// <returns>カメラの前方座標</returns>
        public Vector3 CameraForward() => Vector3.Scale(cameraTrans.forward, new Vector3(1, 0, 1)).normalized;

        /// <summary>
        /// 移動と回転
        /// </summary>
        /// <param name="rot">体を向ける方向</param>
        public void MoveAndRotate(Vector3 rot, float rotMag = 1)
        {
            Controller.Move(MoveVector * ElapsedTime());

            rot.Normalize();
            var moveForward = Vector3.Scale(rot, new Vector3(1, 0, 1));
            if (moveForward.sqrMagnitude > 0.001f)
            {
                MyTransform.rotation = Quaternion.Slerp(MyTransform.rotation, Quaternion.LookRotation(moveForward), (CharData.RotateSpeed * rotMag) * ElapsedTime());
            }
        }
        /// <summary>
        /// 水平面の移動速度を取得
        /// </summary>
        /// <returns>水平面の移動速度</returns>
        public float GetMoveSpeed() => new Vector2(Controller.velocity.x, Controller.velocity.z).magnitude;

        /// <summary>
        /// ジャスト回避成功　スローになる
        /// </summary>
        void StartSlow()
        {
            slowTimer = CharData.SlowTime;

            Time.timeScale = CharData.SlowScale;
            Action.MyAnimator.updateMode = AnimatorUpdateMode.UnscaledTime;
        }

        /// <summary>
        /// ジャスト回避のスロー中
        /// </summary>
        void NowSlow()
        {
            if (slowTimer <= 0)
            {
                slowTimer = 0;
                Time.timeScale = 1.0f;
                Action.MyAnimator.updateMode = AnimatorUpdateMode.Normal;
            }
            slowTimer -= Time.unscaledDeltaTime;
        }



        //------------------------------------------------------------------------------interface



        public void Damage(float damage, float stopTime, Vector3 force)
        {
            //ジャスト回避か?
            if (Avoid.JudgeJustAvoid <= CharData.JudgeAvoidPercent)
            {
                StartSlow();
                return;
            }

            //ダメージを無効化する技中か?
            if(State == Unique || Action.Hit())
            {
                return;
            }

            Debug.Log("Damage");
        }

        public void BlowAway(float damage, Vector3 force)
        {
            throw new System.NotImplementedException();
        }


        /// <summary>
        /// 接地判定の可視化
        /// </summary>
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawSphere(transform.position, Controller.radius * 0.5f);
        }
    }
}