using UnityEngine;

/// <summary>
/// 待機
/// </summary>
public class Enemy_Idle : IStateBase
{

    EnemyController controller;
    public Enemy_Idle(EnemyController enemy) => controller = enemy;

    float time = 0;
    float timer = 0;

    public override void Start()
    {
        Debug.Log("アイドル");
        timer = 0;
        time = Random.Range(controller.IdleTime.x, controller.IdleTime.y);
    }

    public override void Update()
    {
        if (controller.Scouting())
        {
            return;
        }

        timer += Time.deltaTime;
        if (timer >= time) 
        {
            controller.ChangeState(controller.StateLoiter);
        }
    }
}
