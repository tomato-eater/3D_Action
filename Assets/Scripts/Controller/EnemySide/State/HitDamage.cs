using UnityEngine;

namespace Enemy
{
    /// <summary>
    /// Enemy攻撃を食らったら　よろける用
    /// </summary>
    public class HitDamage : State
    {
        public HitDamage(EnemyController controller) : base(controller) { }

        public override void Start()
        {
            Debug.Log("ヒット");
        }

        public override void Update()
        {
            throw new System.NotImplementedException();
        }


    }
}