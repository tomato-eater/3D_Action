using UnityEngine;

/// <summary>
/// 攻撃
/// </summary>
public class Player_Attack : IStateBase
{
    PlayerController controller;
    public Player_Attack(PlayerController player) => controller = player;

    float time;

    public void Start()
    {
        Debug.Log("こうげき");
        time = 0;
    }

    public void Update()
    {
        time += controller.ElapsedTime();
        if (time > 1.2f)
        {
            controller.ChangeState(controller.StateIdle);
        }
    }

    public void End()
    {

    }
}

/*
 攻撃の判定方法

1　武器のモデルにコライダーを割り当て、アニメーションで ON OFF する
    攻撃処理とアニメーションとのタイミングにずれがない
    
2　キャラクター前方にコライダーを出現させる　
    範囲攻撃等で活躍

3　武器の根元から先端にレイを飛ばす
    すり抜けが起きない

 */