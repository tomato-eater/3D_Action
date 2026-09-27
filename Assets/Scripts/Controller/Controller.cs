using UnityEngine;

/// <summary>
/// ステートテンプレート
/// </summary>
public interface IStateBase
{
    /// <summary>
    /// 切り替え直後
    /// </summary>
    void Start();

    /// <summary>
    /// 更新
    /// </summary>
    void Update();

    /// <summary>
    /// 切り替え直前
    /// </summary>
    void End();
}

/// <summary>
/// コントローラーテンプレート
/// </summary>
public class Controller : MonoBehaviour
{
    /// <summary>
    /// 現在のステート
    /// </summary>
    protected IStateBase state;

    protected virtual void Update()
    {
        state?.Update();
    }

    /// <summary>
    /// ステートの切り替え
    /// </summary>
    /// <param name="s">切り替え先のステート</param>
    public void ChangeState(IStateBase s)
    {
        state?.End();
        state = s;
        state?.Start();
    }
}
