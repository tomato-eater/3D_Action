using UnityEngine;

/// <summary>
/// ステートテンプレート
/// </summary>
public abstract class StateBase<Controller>
{
    /// <summary>
    /// コントローラー
    /// </summary>
    protected readonly Controller controller;
    /// <summary>
    /// 宣言
    /// </summary>
    /// <param name="t">コントローラー(this)</param>
    public StateBase(Controller t) => controller = t;

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
public class BehaviorControllerBase<Controller, State> : MonoBehaviour where State :StateBase<Controller>
{
    /// <summary>
    /// 現在のステート
    /// </summary>
    protected State state {  get; private set; }

    /// <summary>
    /// ステートの切り替え
    /// </summary>
    /// <param name="s">切り替え先のステート</param>
    public void ChangeState(State s)
    {
        state?.End();
        state = s;
        state?.Start();
    }
}
