using UnityEngine;


public enum ActivityMode
{
    Explore, // 探索モード
    Battle   // 戦闘モード
}

public class SGameManager : MonoBehaviour
{
    public static SGameManager instance;

    public ActivityMode ActivityMode {  get; private set; } = ActivityMode.Explore;

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

    void Start()
    {

    }


    /// <summary>
    /// 探索に入った
    /// </summary>
    public void StartExploreMode()
    {
        if (ActivityMode == ActivityMode.Explore) return;

        ActivityMode = ActivityMode.Explore;
    }

    /// <summary>
    /// 戦闘に入った
    /// </summary>
    public void StartBattleMode()
    {
        if (ActivityMode == ActivityMode.Battle) return;

        ActivityMode = ActivityMode.Battle;
    }


}
