using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;

/// <summary>
/// 入力システム管理
/// </summary>
[RequireComponent(typeof(PlayerInput))]
public class SInputSystem : MonoBehaviour
{
    public static SInputSystem Instance { get; private set; }
    //プレイヤーインプット
    PlayerInput _playerInput;

    //移動入力アクション
    InputAction _moveAction;
    //カメラ入力アクション
    InputAction _lookAction;

    //ジャンプアクション
    InputAction _jumpAction;

    //ロックオンアクション
    InputAction _targetAction;
    //ロックオン切り替え(個体)アクション
    InputAction _targetTAction;
    //ロックオン切り替え(部位)アクション
    InputAction _targetPAction;

    //防御アクション
    InputAction _defenseAction;
    //走りアクション
    InputAction _runAction;


    /// <summary> 攻撃入力の受付状態 </summary>
    bool _eventActive = false;


    /// <summary>
    /// 移動入力
    /// </summary>
    public Vector2 MoveValue => _moveAction.ReadValue<Vector2>();
    /// <summary>
    /// カメラ入力
    /// </summary>
    public Vector2 CameraMove => _lookAction.ReadValue<Vector2>();

    /// <summary>
    /// ジャンプ入力した
    /// </summary>
    public bool JumpTrigger => _jumpAction.triggered;
    /// <summary>
    /// ジャンプ入力中
    /// </summary>
    public bool JumpPress => _jumpAction.IsPressed();

    /// <summary>
    /// ロックオン・オフ変更
    /// </summary>
    public bool TargetTrigger => _targetAction.triggered;
    /// <summary>
    /// ロックオン対象変更
    /// </summary>
    public bool ChangeTriggerT => _targetTAction.triggered;
    /// <summary>
    /// ロックオン場所変更
    /// </summary>
    public bool ChangeTriggerP => _targetPAction.triggered;

    /// <summary>
    /// 防御入力中
    /// </summary>
    public bool DefenseButton => _defenseAction.IsPressed();

    /// <summary>
    /// 走り入力中
    /// </summary>
    public bool RunButton => _runAction.IsPressed();



    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        TryGetComponent<PlayerInput>(out _playerInput);

        _moveAction = _playerInput.actions["Move"];
        _lookAction = _playerInput.actions["Look"];
        _jumpAction = _playerInput.actions["Jump"];
        _targetAction = _playerInput.actions["TargetTrigger"];
        _targetTAction = _playerInput.actions["ChangeTarget"];
        _targetPAction = _playerInput.actions["ChangePoint"];
        _defenseAction = _playerInput.actions["Defense"];
        _runAction = _playerInput.actions["Run"];

        SwitchMap("Player");
    }
    /// <summary> ゲーム終了時、実行 </summary>
    private void OnApplicationQuit()
    {
        Instance = null;
    }


    /// <summary>
    /// マップの切り替え
    /// </summary>
    /// <param name="map">切り替え先</param>
    void SwitchMap(string map)
    {
        _playerInput.SwitchCurrentActionMap(map);

        var attackAction = _playerInput.actions["Attack"];
        var voidAction = _playerInput.actions["Avoid"];
        var unique = _playerInput.actions["Unique"];
        if (map == "Player")
        {
            if (!_eventActive)
            {
                _eventActive = true;
                attackAction.performed += OnAttackPerform;
                attackAction.canceled += OnAttackCancel;

                voidAction.performed += OnAvoidTap;

                unique.performed += OnUniquePerform;
                unique.canceled += OnUniqueCancel;
            }
            else
            {
                _eventActive = false;
                attackAction.performed -= OnAttackPerform;
                attackAction.canceled -= OnAttackCancel;

                voidAction.performed -= OnAvoidTap;

                unique.performed -= OnUniquePerform;
                unique.canceled -= OnUniqueCancel;
            }
        }
    }


    //----------------------------------------------Attack--Avoid--Skill



    /// <summary> 攻撃タップトリガー状況 </summary>
    bool _tapAttackTrigger = false;
    /// <summary> 攻撃ホールド状況 </summary>
    bool _holdAttack = false;
    /// <summary> 回避発動トリガー </summary>
    bool _tapAvoidTrigger = false;
    /// <summary>
    /// 固有技発動トリガー
    /// </summary>
    bool _uniqueTrigger = false;

    /// <summary> 攻撃タップトリガー状況取得 </summary>
    public bool TapAttackTrigger
    {
        get
        {
            if (_tapAttackTrigger)
            {
                _tapAttackTrigger = false;
                return true;
            }
            return false;
        }
    }
   
    /// <summary> 攻撃ホールド状況取得 </summary>
    public bool HoldAttack
    {
        get
        {
            if (_holdAttack && !DerivedAttackTrigger)
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
    
    /// <summary> 回避発動トリガー状況取得 </summary>
    public bool TapAvoidTrigger
    {
        get
        {
            if (_tapAvoidTrigger)
            {
                _tapAvoidTrigger = false;
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
        if (SGameManager.instance.ActivityMode != ActivityMode.Battle || !avoidReception) return;

        _tapAttackTrigger = false;
        _holdAttack = false;
        _uniqueTrigger = false;

        if (context.interaction is TapInteraction)
        {
            _tapAvoidTrigger = true;
        }
    }

    /// <summary> 攻撃入力　押した時 </summary>
    /// <param name="context"></param>
    void OnAttackPerform(InputAction.CallbackContext context)
    {
        _tapAvoidTrigger = false;
        _uniqueTrigger = false;

        if (context.interaction is TapInteraction)
        {
            _tapAttackTrigger = true;
        }
        else
        {
            _holdAttack = true;
        }
    }
    /// <summary> 攻撃入力　離した時 </summary>
    /// <param name="context"></param>
    void OnAttackCancel(InputAction.CallbackContext context)
    {
        if (context.interaction is HoldInteraction)
        {
            _holdAttack = false;
            DerivedAttackTrigger = false;
        }
    }

    /// <summary>
    /// 固有技発動トリガー状況取得
    /// </summary>
    public bool UniqueTrigger
    {
        get
        {
            if (_uniqueTrigger)
            {
                _uniqueTrigger = false;
                return true;
            }
            return false;
        }
    }

    /// <summary>
    /// 固有技入力解除状況取得
    /// </summary>
    public bool UniqueCancel { get; private set; } = true;

    /// <summary>
    /// 固有技入力
    /// </summary>
    /// <param name="context"></param>
    void OnUniquePerform(InputAction.CallbackContext context)
    {
        //ボタンが離されていない状況で押下したら
        if (!UniqueCancel) return;

        _tapAvoidTrigger = false;
        _tapAttackTrigger = false;
        _holdAttack = false;

        _uniqueTrigger = true;
        UniqueCancel = false;
    }
    /// <summary>
    /// 固有技解除
    /// </summary>
    /// <param name="context"></param>
    void OnUniqueCancel(InputAction.CallbackContext context)
    {
        UniqueCancel = true;
    }
}
