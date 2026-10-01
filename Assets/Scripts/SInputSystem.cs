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
    InputAction avoidAction;

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
    /// 回避入力
    /// </summary>
    public bool AvoidTrigger => avoidAction.triggered;
    /// <summary>
    /// 走り入力中
    /// </summary>
    public bool RunButton => avoidAction.IsPressed();

    /// <summary>
    /// 攻撃入力の受付状態
    /// </summary>
    bool attackActive = false;
    /// <summary>
    /// 攻撃タップ入力
    /// </summary>
    bool tapAttackTrigger = false;
    /// <summary>
    /// 攻撃タップ入力受付
    /// </summary>
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
    /// <summary>
    /// 攻撃ホールド状態
    /// </summary>
    public bool HoldAttack { get; private set; } = false;
    /// <summary>
    /// 攻撃未入力
    /// </summary>
    public bool DerivedAttackTrigger { get; private set; } = true;

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
        avoidAction    = playerInput.actions["Avoid"];

        SwitchMap("Player");
    }
    /// <summary>
    /// ゲーム終了時、実行
    /// </summary>
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
        if (map == "Player")
        {
            if (!attackActive)
            {
                attackActive = true;
                attackAction.performed += OnAttackPerform;
                attackAction.canceled += OnAttackCancel;
            }
            else
            {
                attackActive = false;
            }
        }
    }


    void OnAttackPerform(InputAction.CallbackContext context)
    {
        if(context.interaction is TapInteraction)
        {
            tapAttackTrigger = true;
        }
        else
        {
            HoldAttack = true;
            DerivedAttackTrigger = false;
        }
    }

    void OnAttackCancel(InputAction.CallbackContext context)
    {
        if(context.interaction is HoldInteraction)
        {
            HoldAttack = false;
            DerivedAttackTrigger = true;
        }
    }

}
