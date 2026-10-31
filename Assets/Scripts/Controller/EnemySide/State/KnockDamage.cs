using UnityEngine;

namespace Enemy
{
    /// <summary>
    /// Enemy攻撃を食らったら　吹っ飛ぶ用
    /// </summary>
    public class KnockDamage : State
    {
        public KnockDamage(EnemyController controller) : base(controller) { }

        public override void Start()
        {
            

        }

        public override void Update()
        {
            throw new System.NotImplementedException();
        }

    }

}
