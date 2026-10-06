using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;

/// <summary>
/// 入力システム管理
/// </summary>
[RequireComponent(typeof(PlayerInput))]
public class SInputSystem : MonoBehaviour
{
    public static SInputSystem instance { get; private set; }

    PlayerInput playerInput;

    InputAction moveAction;
    InputAction lookAction;

    InputAction jumpAction;

    InputAction targetAction;
    InputAction targetTAction;
    InputAction targetPAction;

    InputAction defenseAction;

    InputAction runAction;

    /// <summary> 攻撃入力の受付状態 </summary>
    bool eventActive = false;

    /// <summary>
    /// 移動入力
    /// </summary>
    public Vector2 MoveValue => moveAction.ReadValue<Vector2>();
    /// <summary>
    /// カメラ入力
    /// </summary>
    public Vector2 CameraMove => lookAction.ReadValue<Vector2>();

    /// <summary>
    /// ジャンプ入力した
    /// </summary>
    public bool JumpTrigger => jumpAction.triggered;
    /// <summary>
    /// ジャンプ入力中
    /// </summary>
    public bool JumpPress => jumpAction.IsPressed();

    /// <summary>
    /// ロックオン・オフ変更
    /// </summary>
    public bool TargetTrigger => targetAction.triggered;
    /// <summary>
    /// ロックオン対象変更
    /// </summary>
    public bool ChangeTriggerT => targetTAction.triggered;
    /// <summary>
    /// ロックオン場所変更
    /// </summary>
    public bool ChangeTriggerP => targetPAction.triggered;

    /// <summary>
    /// 防御入力中
    /// </summary>
    public bool DefenseButton => defenseAction.IsPressed();
    
    /// <summary>
    /// 走り入力中
    /// </summary>
    public bool RunButton => runAction.IsPressed();



    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);

        TryGetComponent<PlayerInput>(out playerInput);

        moveAction     = playerInput.actions["Move"];
        lookAction     = playerInput.actions["Look"];
        jumpAction     = playerInput.actions["Jump"];
        targetAction   = playerInput.actions["TargetTrigger"];
        targetTAction  = playerInput.actions["ChangeTarget"];
        targetPAction  = playerInput.actions["ChangePoint"];
        defenseAction  = playerInput.actions["Defense"];
        runAction      = playerInput.actions["Run"];

        SwitchMap("Player");
    }
    /// <summary> ゲーム終了時、実行 </summary>
    private void OnApplicationQuit()
    {
        instance = null;
    }
    

    /// <summary>
    /// マップの切り替え
    /// </summary>
    /// <param name="map">切り替え先</param>
    void SwitchMap(string map)
    {
        playerInput.SwitchCurrentActionMap(map);

        var attackAction = playerInput.actions["Attack"];
        var voidAction = playerInput.actions["Avoid"];
        if (map == "Player")
        {
            if (!eventActive)
            {
                eventActive = true;
                attackAction.performed += OnAttackPerform;
                attackAction.canceled += OnAttackCancel;
                voidAction.performed += OnAvoidTap;
            }
            else
            {
                eventActive = false;
                attackAction.performed -= OnAttackPerform;
                attackAction.canceled -= OnAttackCancel;
                voidAction.performed -= OnAvoidTap;

            }
        }
    }


    //----------------------------------------------Attack--Avoid

    /// <summary> 攻撃タップトリガー状況 </summary>
    bool tapAttackTrigger = false;
    /// <summary> 攻撃タップトリガー状況取得 </summary>
    public bool TapAttackTrigger
    {
        get
        {
            if (tapAttackTrigger)
            {
                tapAttackTrigger = false;
                return true;
            }
            return false;
        }
    }
    /// <summary> 攻撃ホールド状況 </summary>
    bool holdAttack = false;
    /// <summary> 攻撃ホールド状況取得 </summary>
    public bool HoldAttack
    {
        get
        {
            if (holdAttack && !DerivedAttackTrigger)
            {
                DerivedAttackTrigger = true;
                return true;
            }
            return false;
        }
    }
    /// <summary> 攻撃ホールド解除状況取得 </summary>
    public bool DerivedAttackTrigger { get; private set; } = false;

    /// <summary> 回避受付状態 </summary>
    bool avoidReception = true;
    /// <summary> 回避トリガー </summary>
    bool tapAvoidTrigger = false;
    /// <summary> 回避トリガー状況取得 </summary>
    public bool TapAvoidTrigger
    {
        get
        {
            if (tapAvoidTrigger)
            {
                tapAvoidTrigger = false;
                return true;
            }
            return false;
        }
    }

    /// <summary> 回避受付状態更新 </summary>
    /// <param name="active">bool 有効 : 無効</param>
    public void AvoidActive(bool active) => avoidReception = active;
    /// <summary> 回避入力　押した時 </summary>
    /// <param name="context"></param>
    void OnAvoidTap(InputAction.CallbackContext context)
    {
        if (!avoidReception) return;

        tapAttackTrigger = false;
        holdAttack = false;
        if (context.interaction is TapInteraction)
        {
            tapAvoidTrigger = true;
        }
    }
    
    /// <summary> 攻撃入力　押した時 </summary>
    /// <param name="context"></param>
    void OnAttackPerform(InputAction.CallbackContext context)
    {
        tapAvoidTrigger = false;
        if(context.interaction is TapInteraction)
        {
            tapAttackTrigger = true;
        }
        else
        {
            holdAttack = true;
        }
    }
    /// <summary> 攻撃入力　離した時 </summary>
    /// <param name="context"></param>
    void OnAttackCancel(InputAction.CallbackContext context)
    {
        if(context.interaction is HoldInteraction)
        {
            holdAttack = false;
            DerivedAttackTrigger = false;
        }
    }


}
