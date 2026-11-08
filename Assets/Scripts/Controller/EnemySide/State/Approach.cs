using UnityEngine;

namespace Enemy
{
    /// <summary>
    /// Enemy接近(通常攻撃)
    /// </summary>
    public class Approach : State
    {
        public Approach(EnemyController controller) : base(controller) { }

        public override void Start()
        {
            
        }

        public override void Update()
        {
            throw new System.NotImplementedException();
        }
    }
}
