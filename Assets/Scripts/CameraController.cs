using UnityEngine;
using static UnityEditor.SceneView;
using static UnityEngine.GraphicsBuffer;

/// <summary>
/// カメラ制御
/// </summary>
public class CameraController : MonoBehaviour
{
    [Header("Parameter")]
    [SerializeField, Tooltip("通常時のカメラ速度 x_横軸 y_縦軸")]
    Vector2 moveSpeed;
    [SerializeField, Tooltip("ロックオン時のカメラ速度 x_横軸 y_縦軸")]
    Vector2 targetSpeed;
    [SerializeField, Tooltip("透けさせたくないレイヤー")] 
    LayerMask layerMask;
    [SerializeField, Tooltip("カメラの大きさ"), Range(0.01f, 0.5f)]
    float cameraMargin;
    [SerializeField, Tooltip("ジャンプ時等の縦軸のカメラの遅さ"), Range(0.01f, 1.0f)]
    float smoothY;
    [Space(10)]
    [SerializeField, Tooltip("通常時のカメラ稼働限界_縦軸 x_低さ y_高さ")] 
    Vector2 normalLimitY;  
    [SerializeField, Tooltip("通常時の注目点の高さ x_低いとき y_高いとき")]
    Vector2 normalOffsetY;
    [SerializeField, Tooltip("通常時のカメラの遠さ x_近いとき y_遠いとき")]
    Vector2 normalDisArea;
    [SerializeField, Tooltip("ロックオン時から通常状態への移行時間"), Range(0.01f, 1.0f)]
    float duration;
    [Space(10)]
    [SerializeField, Tooltip("ロックオン時の移動速度の遅さ"), Range(0.01f, 0.5f)]
    float smoothTime;
    [SerializeField, Tooltip("ロックオン時の回転速度の速さ"), Range(0.01f, 50f)]
    float rotSpeed;
    [SerializeField, Tooltip("ロックオン時の注目点の高さ"), Range(0.0f, 5.0f)] 
    float targetDirY;
    [SerializeField, Tooltip("ロックオン時のプレイヤーと敵の距離指数 x_近いとき y_遠いとき")]
    Vector2 targetDirRad;
    [SerializeField, Tooltip("ロックオン時のプレイヤーとカメラの遠さ x_近いとき y_遠いとき")]
    Vector2 targetDirZ;
    [SerializeField, Tooltip("ロックオン時のカメラ稼働限界_横軸 x_左側 y_右側")]
    Vector2 targetLimitX;
    [SerializeField, Tooltip("ロックオン時のカメラ稼働限界_縦軸 x_低さ y_高さ")]
    Vector2 targetLimitY;

    [Header("Refer"), Tooltip("プレイヤーコントローラー")]
    [SerializeField] PlayerController player;

    Transform MyTransform;
    float yaw;  //横軸
    float pitch;//縦軸
    float targetYaw;
    float targetPitch;
    Vector3 cameraVelocity;
    bool lockOn = false;
    float currentFollowY;
    float followVelocityY;

    float timer = 0;
    Vector3 lockPos;
    Quaternion lockRot;

    private void Start()
    {
        if(!player)
        {
            Debug.LogError("カメラで未割当有り");
        }

        MyTransform = transform;
        yaw = MyTransform.eulerAngles.y;
        pitch = MyTransform.eulerAngles.x;
        if (pitch > 180)
        {
            pitch -= 360.0f;
        }
    }

    //Updateの最後に通る
    private void LateUpdate()
    {
        Vector3 pos = Vector3.zero;
        Quaternion rot = Quaternion.identity;
        //通常
        if (!player.target)
        {
            UsuallyMode(ref pos, ref rot);
        }
        //ロックオン
        else
        {
            LockMode(ref pos, ref rot);
        }
        //最終結果を更新
        MyTransform.position = pos;
        MyTransform.rotation = rot;
    }

    /// <summary>
    /// カメラの移動
    /// </summary>
    /// <param name="yaw">横軸</param>
    /// <param name="pitch">縦軸</param>
    /// <param name="speed">速度</param>
    void CameraMove(ref float yaw, ref float pitch, Vector2 speed)
    {
        var move = SInputSystem.instance.CameraMove;
        yaw += move.x * speed.x * player.ElapsedTime();
        pitch += move.y * speed.y * player.ElapsedTime();
    }

    /// <summary>
    /// 透け防止
    /// </summary>
    /// <param name="pos">始点</param>
    /// <param name="dir">終点</param>
    /// <returns>最終的なカメラの位置</returns>
    Vector3 AntiSeeThrough(Vector3 pos, Vector3 dir)
    {
        RaycastHit hit;
        if (Physics.Linecast(pos, dir, out hit, layerMask))
        {
            dir = hit.point + hit.normal * cameraMargin;
        }
        return dir;
    }

    /// <summary>
    /// 通常時の挙動
    /// </summary>
    /// <param name="pos">カメラ位置</param>
    /// <param name="rot">カメラ角度</param>
    void UsuallyMode(ref Vector3 pos, ref Quaternion rot)
    {
        //ロックオンからの移行
        if (lockOn)
        {
            lockOn = false;
            //ロックオン時の変数をリセット
            targetYaw = 0;
            targetPitch = 0;
            //変更時の値を保管
            lockPos = MyTransform.position;
            lockRot = MyTransform.rotation;
            yaw = MyTransform.eulerAngles.y;
            pitch = MyTransform.eulerAngles.x;
            //調整
            if (pitch > 180.0f)
                pitch -= 360.0f;
            pitch = Mathf.Clamp(pitch, normalLimitY.x, normalLimitY.y);

            timer = duration;
        }

        //カメラの移動　補正
        CameraMove(ref yaw, ref pitch, moveSpeed);
        yaw = (yaw % 360 + 360) % 360;
        pitch = Mathf.Clamp(pitch, normalLimitY.x, normalLimitY.y);

        //カメラの向き
        rot = Quaternion.Euler(pitch, yaw, 0);
        //カメラの向きYの割合
        var ratio = Mathf.InverseLerp(normalLimitY.x, normalLimitY.y, pitch);
        //Yに応じて微調整
        var dis = -Vector3.Slerp(new Vector3(0, 0, normalDisArea.x), new Vector3(0, 0, normalDisArea.y), ratio);
        var y = Mathf.Lerp(normalOffsetY.x, normalOffsetY.y, ratio);

        currentFollowY = Mathf.SmoothDamp(currentFollowY, player.MyTransform.position.y, ref followVelocityY, smoothY);
        //注目点
        var target = new Vector3(player.MyTransform.position.x, currentFollowY, player.MyTransform.position.z) + new Vector3(0, y, 0);
        //カメラ位置
        pos = AntiSeeThrough(target, rot * dis + target);
        //ロックオンからの移行中
        if (timer > 0)
        {
            timer -= player.ElapsedTime();
            var t = 1.0f - (timer / duration);
            t = Mathf.SmoothStep(0, 1.0f, t);
            //動きをなめらかにする
            pos = Vector3.Lerp(lockPos, pos, t);
            rot = Quaternion.Slerp(lockRot, rot, t);
        }
    }


    /// <summary>
    /// ロックオン状態の挙動
    /// </summary>
    /// <param name="pos">カメラ位置</param>
    /// <param name="rot">カメラ角度</param>
    void LockMode(ref Vector3 pos, ref Quaternion rot)
    {
        lockOn = true;
        //カメラの移動　補正
        CameraMove(ref targetYaw, ref targetPitch, targetSpeed);
        targetYaw = Mathf.Clamp(targetYaw, targetLimitX.x, targetLimitX.y);
        targetPitch = Mathf.Clamp(targetPitch, targetLimitY.x, targetLimitY.y);

        //座標取得
        var pPos = player.MyTransform.position;
        var tPos = player.target.transform.position;

        currentFollowY = Mathf.SmoothDamp(currentFollowY, pPos.y, ref followVelocityY, smoothY);
        pPos.y = currentFollowY;

        //距離計算
        var disRatio = Mathf.InverseLerp(targetDirRad.x, targetDirRad.y, Vector3.Distance(pPos, tPos));
        var distance = Mathf.Lerp(-targetDirZ.x, -targetDirZ.y, disRatio);
        //敵からプレイヤーのベクトル計算
        var t_p = Vector3.Scale(tPos - pPos, new Vector3(1, 0, 1)).normalized;
        //完全に重なった場合、プレイヤーの背後にする
        if (t_p == Vector3.zero)
            t_p = -player.transform.forward;
        t_p.Normalize();

        //カメラの向き
        var offRot = Quaternion.Euler(targetPitch, targetYaw, 0);
        var direction = Quaternion.LookRotation(t_p) * offRot * Vector3.forward;
        //注目点
        var target = pPos + new Vector3(0, targetDirY, 0);
        //カメラ位置
        var proPos = AntiSeeThrough(target, target + direction * distance);
        // 現在位置から targetPosition へ、指定した時間をかけて滑らかに移動
        pos = Vector3.SmoothDamp(MyTransform.position, proPos, ref cameraVelocity, smoothTime);
        // 現在の回転から目標の回転へ滑らかに補間
        tPos.y += targetDirY;
        var look = (pPos + tPos) * 0.5f;
        rot = Quaternion.Slerp(MyTransform.rotation, Quaternion.LookRotation(look - MyTransform.position), rotSpeed * player.ElapsedTime());
    }
}
