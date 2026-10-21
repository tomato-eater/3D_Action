using UnityEngine;

namespace Enemy
{
    /// <summary>
    /// Enemyアイドル
    /// </summary>
    public class Idle : EnemyState
    {
        public Idle(EnemyBehaviorController controller) : base(controller) { }

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
                controller.ChangeState(controller.Loiter);
            }
        }
    }
}
