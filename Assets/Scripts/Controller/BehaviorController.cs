using UnityEngine;

/// <summary>
/// ステートテンプレート
/// </summary>
public abstract class IStateBase
{
    /// <summary>
    /// 切り替え直後
    /// </summary>
    public virtual void Start() { }

    /// <summary>
    /// 更新
    /// </summary>
    public abstract void Update();

    /// <summary>
    /// 切り替え直前
    /// </summary>
    public virtual void End() { }
}

/// <summary>
/// コントローラーテンプレート
/// </summary>
public class BehaviorController : MonoBehaviour
{
    /// <summary>
    /// 現在のステート
    /// </summary>
    protected IStateBase state;

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
