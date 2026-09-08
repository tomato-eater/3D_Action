using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
/// <summary>
/// 入力システム管理
/// </summary>
public class S_InputSystem : MonoBehaviour
{
    public static S_InputSystem instance { get; private set; }

    [SerializeField] PlayerInput playerInput;
    public Vector2 moveValue { get; private set; }


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
            if (TryGetComponent<PlayerInput>(out var pi))
            {
                playerInput = pi;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        moveValue = playerInput.actions["Move"].ReadValue<Vector2>();
    }
}
