using UnityEngine;

/// <summary>
/// ステートテンプレート
/// </summary>
public abstract class StateBase<TController>
{
    /// <summary>
    /// コントローラー
    /// </summary>
    protected TController _controller;
    /// <summary>
    /// 宣言
    /// </summary>
    /// <param name="t">コントローラー</param>
    public StateBase(TController t) => _controller = t;

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
public class BehaviorControllerBase<TController, TState> : MonoBehaviour where TState : StateBase<TController>
{
    /// <summary>
    /// 現在のステート
    /// </summary>
    protected TState State { get; private set; }

    protected virtual void Update()
    {
        State?.Update();
    }

    /// <summary>
    /// ステートの切り替え
    /// </summary>
    /// <param name="s">切り替え先のステート</param>
    public void ChangeState(TState s)
    {
        State?.End();
        State = s;
        State?.Start();
    }
}
