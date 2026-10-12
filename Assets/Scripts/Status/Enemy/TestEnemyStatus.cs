using UnityEngine;

public class TestEnemyStatus : StatusBase
{
    EnemyBehaviorController controller;

    private void Start()
    {
        transform.parent.TryGetComponent<EnemyBehaviorController>(out controller);
    }


    public override void Damage(float damage)
    {
        Debug.Log(controller.name + ": " + damage);
    }
}
