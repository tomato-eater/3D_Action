using System.Security.Cryptography.X509Certificates;
using UnityEngine;

/// <summary>
/// 発見した
/// </summary>
public class Enemy_Discovery : IStateBase
{
    EnemyController controller;
    public Enemy_Discovery(EnemyController enemy) => controller = enemy;

    /// <summary>
    /// 仮の変数
    /// </summary>
    float timer = 0;

    public void Start()
    {
        Debug.Log("ハッケン");

        controller.Agent.isStopped = true;
        controller.Area.BattleStart();

        timer = 0;
    }

    public void Update()
    {
        ////指定したアニメーションが再生されているかの確認
        //if (!controller.animator.GetCurrentAnimatorStateInfo(0).IsName(アニメーションの名前))
        //{
        //    return;
        //}
        ////指定したアニメーションが指定した割合まで再生されたか
        //if(controller.animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f)
        //{
        //    controller.ChangeState()
        //}

        ///------仮の処理
        timer += Time.deltaTime;
        if (timer > 1)
        {

        }
    }

    public void End()
    {
        controller.Agent.isStopped = false;

    }
}
