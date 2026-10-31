using UnityEngine;

namespace Enemy
{
    /// <summary>
    /// Enemy逃げられた
    /// </summary>
    public class Escaped : State
    {
        public Escaped(EnemyController controller) : base(controller) { }

        public override void Start()
        {
            

        }

        public override void Update()
        {
            throw new System.NotImplementedException();
        }



    }
}
