using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// 入力システム管理
/// </summary>
[RequireComponent(typeof(PlayerInput))]
public class SInputSystem : MonoBehaviour
{
    public static SInputSystem instance { get; private set; }

    [SerializeField] PlayerInput playerInput;
    InputActionMap playerMap;

    /// <summary>
    /// 移動入力
    /// </summary>
    public Vector2 MoveValue { get; private set; }
    /// <summary>
    /// カメラ入力
    /// </summary>
    public Vector2 CameraMove {  get; private set; }
    /// <summary>
    /// 攻撃入力
    /// </summary>
    public bool AttackButton {  get; private set; }
    /// <summary>
    /// ジャンプ入力
    /// </summary>
    public bool JumpButton {  get; private set; }
    /// <summary>
    /// ロック入力
    /// </summary>
    public bool TargetButton {  get; private set; }
    /// <summary>
    /// ロックオン対象変更
    /// </summary>
    public bool ChangeButtonT {  get; private set; }
    /// <summary>
    /// ロックオン場所変更
    /// </summary>
    public bool ChangeButtonP {  get; private set; }
    /// <summary>
    /// 防御入力
    /// </summary>
    public bool DefenseButton {  get; private set; }
    /// <summary>
    /// 回避入力
    /// </summary>
    public bool AvoidButton {  get; private set; }

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
    }
    /// <summary>
    /// ゲーム終了時、実行
    /// </summary>
    private void OnApplicationQuit()
    {
        instance = null;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (!playerInput)
        {
            if (TryGetComponent<PlayerInput>(out playerInput))
            {
                InputActionAsset asset = playerInput.actions;
                playerMap = asset.FindActionMap("Player");

            }
        }

        
    }

    // Update is called once per frame
    void Update()
    {
        MoveValue = playerInput.actions["Move"].ReadValue<Vector2>();
        CameraMove = playerInput.actions["Look"].ReadValue<Vector2>();

        AttackButton = playerInput.actions["Attack"].IsPressed();

        JumpButton = playerInput.actions["Jump"].IsPressed();

        TargetButton = playerInput.actions["TargetTrigger"].IsPressed();
        ChangeButtonT = playerInput.actions["ChangeTarget"].IsPressed();
        ChangeButtonP = playerInput.actions["ChangePoint"].IsPressed();
        
        DefenseButton = playerInput.actions["Defense"].IsPressed();
        AvoidButton = playerInput.actions["Avoid"].IsPressed();
    }
}
