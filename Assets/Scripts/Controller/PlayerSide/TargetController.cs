using UnityEngine;

/// <summary>
/// ロックオン管理
/// </summary>
public class TargetController : MonoBehaviour
{
    [Header("Parameter")]
    [SerializeField] float scoutingRadius;
    [SerializeField] float angle;
    [SerializeField] LayerMask mask;

    [Header("Component")]
    [SerializeField] PlayableBehaviorController player;

    BattleArea battleArea;
    int individual;
    int part;

    private void Start()
    {
        TryGetComponent<PlayableBehaviorController>(out player);
    }

    void Update()
    {
        //ターゲットロックオン・オフ
        LockEnemy();
        //ターゲット変更
        if(player.target)
        {
            if(Vector3.Distance(transform.position, player.target.transform.position) > scoutingRadius) {
                player.target = null;
                battleArea = null;
                return;
            }

            ChangeEnemy();
        }
    }



    /// <summary>
    /// 敵のロックオン、オフ
    /// </summary>
    void LockEnemy()
    {
        if (SInputSystem.instance.TargetTrigger)
        {
            //既にターゲットがいる
            if (player.target != null)
            {
                player.target = null;
                battleArea = null;
                return;
            }
            //ターゲットがいない
            //敵の取得
            if (GetShortestCollider(out var collider))
            {
                //最短にいる敵を取得
                if (collider.transform.TryGetComponent<EnemyController>(out var controller))
                {
                    //ロックオン
                    part = 0;
                    player.target = controller.points[part];
                    //グループを取得
                    if (collider.transform.parent.TryGetComponent<BattleArea>(out battleArea))
                    {
                        //ロックオンした奴は何番目か
                        for (individual = 0; individual < battleArea.enemyControllers.Length; individual++)
                        {
                            //発見次第終了
                            if (controller == battleArea.enemyControllers[individual])
                                break;
                        }
                    }
                }
            }
        }
    }

    /// <summary>
    /// 索敵後、範囲内で最も近い敵を取得
    /// </summary>
    /// <param name="collider">最も近いコライダーが入る</param>
    /// <returns>取得できたか ? true : false</returns>
    bool GetShortestCollider(out Collider collider)
    {
        collider = null;
        float minDir = float.MaxValue;

        //範囲内で索敵
        Collider[] hit = Physics.OverlapSphere(player.MyTransform.position, scoutingRadius, mask);
        //居なければ終わり
        if (hit.Length == 0)
            return false;
        //該当した敵を洗い出す
        foreach (var enemy in hit)
        {
            //自身から敵へのベクトル、角度を調べる
            var dir = (enemy.transform.position - player.MyTransform.position).normalized;
            var eAngle = Vector3.Angle(player.CameraForward(), dir);
            //範囲内なら
            if (eAngle < angle * 0.5f)
            {
                //距離を計算　最短を選択
                var toDir = (enemy.transform.position - player.MyTransform.position).sqrMagnitude;
                if (toDir < minDir)
                {
                    minDir = toDir;
                    collider = enemy;
                }
            }
        }
        return true;
    }



    /// <summary>
    /// ターゲットの変更
    /// </summary>
    void ChangeEnemy()
    {
        //敵を変える
        if (SInputSystem.instance.ChangeTriggerT)
        {
            ChangeTarget();
        }

        //部位を変える
        if (SInputSystem.instance.ChangeTriggerP)
        {
            //部位が１つの場合は敵自体を変える
            if (battleArea.enemyControllers[individual].points.Length == 1)
            {
                ChangeTarget();
            }
            else
            {
                ChangePoint();
            }
        }
    }

    /// <summary>
    /// 標的の変更
    /// </summary>
    void ChangeTarget()
    {
        individual++;
        if (individual >= battleArea.enemyControllers.Length)
        {
            individual = 0;
        }
        part = 0;

        ChangeLock();
    }
    /// <summary>
    /// 部位の変更
    /// </summary>
    void ChangePoint()
    {
        part++;
        if(part >= battleArea.enemyControllers[individual].points.Length)
        {
            part = 0;
        }
        ChangeLock();
    }
    /// <summary>
    /// 変更を更新
    /// </summary>
    private void ChangeLock()
    {
        player.target = battleArea.enemyControllers[individual].points[part];
    }


    /// <summary>
    /// 索敵範囲の可視化
    /// </summary>
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, scoutingRadius);

        Vector3 leftBoundary = Quaternion.Euler(0, -angle * 0.5f, 0) * player.CameraForward();
        Vector3 rightBoundary = Quaternion.Euler(0, angle * 0.5f, 0) * player.CameraForward();

        Gizmos.DrawLine(transform.position, transform.position + leftBoundary * angle);
        Gizmos.DrawLine(transform.position, transform.position + rightBoundary * angle);
    }
}
