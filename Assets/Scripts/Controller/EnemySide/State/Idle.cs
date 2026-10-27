using UnityEngine;

namespace Enemy
{
    /// <summary>
    /// Enemyアイドル
    /// </summary>
    public class Idle : State
    {
        public Idle(EnemyController controller) : base(controller) { }

        float time = 0;
        float timer = 0;

        public override void Start()
        {
            Debug.Log("アイドル");
            timer = 0;
            time = Random.Range(_controller.IdleTime.x, _controller.IdleTime.y);
        }

        public override void Update()
        {
            if (_controller.Scouting())
            {
                return;
            }

            timer += Time.deltaTime;
            if (timer >= time)
            {
                _controller.ChangeState(_controller.Loiter);
            }
        }
    }
}
