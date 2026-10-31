using UnityEngine;

namespace Enemy
{
    /// <summary>
    /// Represents a wait state for an enemy.
    /// </summary>
    public class Wait : State
    {
        public Wait(EnemyController controller) : base(controller) { }

        public override void Start()
        {
            Debug.Log("待機");
        }

        public override void Update()
        {
            throw new System.NotImplementedException();
        }

    }
}

