using UnityEngine;

/// <summary>
/// 回避
/// </summary>
public class Player_Avoid : IStateBase
{
    PlayableBehaviorController controller;
    public Player_Avoid(PlayableBehaviorController player) => controller = player;

    /// <summary>
    /// 開始地点
    /// </summary>
    Vector3 startPos;

    /// <summary>
    /// 移動先
    /// </summary>
    Vector3 targetPos;

    /// <summary>
    /// 回避時間経過割合
    /// </summary>
    float timer;

    /// <summary>
    /// 回避時間経過割合
    /// </summary>
    public float judgeJustAvoid { get; private set; } = 1;

    public override void Start()
    {
        timer = 0;
        judgeJustAvoid = 0.01f;
        startPos = controller.MyTransform.position;

        //移動方向と量を求める
        var moveValue = SInputSystem.instance.MoveValue;
        if(moveValue != Vector2.zero)
        {
            var forward = controller.CameraForward();
            targetPos = (forward * moveValue.y + controller.cameraTrans.right * moveValue.x).normalized * controller.CharData.AvoidDistance;
        }
        else
        {
            targetPos = -controller.MyTransform.forward * controller.CharData.AvoidDistance; ;
        }
        targetPos = startPos + targetPos;


        Debug.Log("かいひ");


    }


    public override void Update()
    {
        timer += controller.ElapsedTime();
        
        var progress = Mathf.Clamp01(timer / controller.CharData.AvoidTime);

        //移動割合計算
        judgeJustAvoid = 1f - Mathf.Pow(1f - progress, 3f);

        //割合から移動先の位置を取得
        Vector3 idealPos = Vector3.Lerp(startPos, targetPos, judgeJustAvoid);

        //差分を計算
        Vector3 moveVelocity = idealPos - controller.MyTransform.position;
        //移動
        controller.Controller.Move(moveVelocity);

        //移動しきったら戻す
        if (timer >= controller.CharData.AvoidTime)
        {
            controller.ChangeState(controller.StateIdle);
            return;
        }

        //移動速度が0ならキャンセル
        var moveSpeed = new Vector3(controller.Controller.velocity.x, 0, controller.Controller.velocity.z).magnitude;
        if (moveSpeed == 0)
        {
            controller.ChangeState(controller.StateIdle);
            return;
        };
    }


    public override void End()
    {
        judgeJustAvoid = 1;
    }
}
