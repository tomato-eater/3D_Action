using UnityEngine;

public class Enemy_Action : IStateBase
{

    EnemyController controller;
    public Enemy_Action(EnemyController enemy) => controller = enemy;


    public override void Start()
    {
        Debug.Log("ƒAƒNƒVƒ‡ƒ“");

    }

    public override void Update()
    {

    }

}
