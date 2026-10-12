using UnityEngine;

public class Enemy_Action : StateBase
{

    EnemyBehaviorController controller;
    public Enemy_Action(EnemyBehaviorController enemy) => controller = enemy;


    public override void Start()
    {
        Debug.Log("ƒAƒNƒVƒ‡ƒ“");

    }

    public override void Update()
    {

    }

}
