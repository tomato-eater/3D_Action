using UnityEngine;
using UnityEngine.InputSystem.XR;

public class BattleArea : MonoBehaviour
{
    [SerializeField, Tooltip("Player")] PlayerController player;
    [SerializeField, Tooltip("—LŒø‹——£")] float activeRadius = 60.0f;

    [SerializeField, Tooltip("í“¬ƒGƒŠƒA”¼Œa")] float radius;
    [Tooltip("“G")] public EnemyController[] enemyControllers;

    /// <summary>
    /// •`‰æ”ÍˆÍ“à‚É“ü‚Á‚½
    /// </summary>
    bool activation = true;
    /// <summary>
    /// í“¬”ÍˆÍ“à‚É“ü‚Á‚½
    /// </summary>
    public bool Within { get; private set; } = false;
    /// <summary>
    /// í“¬‚É“ü‚Á‚½
    /// </summary>
    public bool IsBattle { get; private set; } = false;

    public Vector3 AreaPos { get; private set; }
    public float Radius => radius;
    public Transform PlayerPos { get; private set; }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        AreaPos = transform.position;
        PlayerPos = player.transform;
    }

    // Update is called once per frame
    void Update()
    {
        //Player‚Æ‚Ì‹——£‚ğ‘ª‚é
        var dis = Vector3.Distance(AreaPos, PlayerPos.position);
        //•`‰æó‘Ô‚ÌØ‚è‘Ö‚¦
        if (activation != dis <= activeRadius) 
        {
            ChangeActive(dis <= activeRadius);
        }
        //í“¬”ÍˆÍ“à‚©‚Ì”»’f
        dis = Vector2.Distance(new Vector2(AreaPos.x, AreaPos.z), new Vector2(PlayerPos.position.x, PlayerPos.position.z));
        if (Within != dis <= radius)
        {
            Within = !Within;
            if (!Within)
            {
                IsBattle = false;
                SGameManager.instance.StartExploreMode();
            }
        }
        if (SGameManager.instance.ActivityMode != ActivityMode.Battle && IsBattle)
        {
            SGameManager.instance.StartBattleMode();
        }
    }

    /// <summary>
    /// “G‚Ì—LŒøó‘Ô‚ÌØ‚è‘Ö‚¦
    /// </summary>
    /// <param name="active">XVŒã‚Ìó‘Ô</param>
    void ChangeActive(bool active)
    {
        activation = active;
        foreach (var enemy in enemyControllers)
            enemy.gameObject.SetActive(active);
    }

    public void BattleStart()
    {
        if (IsBattle) return;

        IsBattle = true;
        SGameManager.instance.StartBattleMode();
    }


    /// <summary>
    /// í“¬”ÍˆÍ‚Ì‰Â‹‰»
    /// </summary>
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, Radius);
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(transform.position, activeRadius);
    }
}
