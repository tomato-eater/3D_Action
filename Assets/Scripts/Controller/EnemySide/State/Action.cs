using UnityEngine;

namespace Enemy
{
    /// <summary>
    /// Enemy行動
    /// </summary>
    public class Action : State
    {
        public Action(EnemyController controller) : base(controller) { }
        public override void Start()
        {
            Debug.Log("状態選択");
        }
        public override void Update()
        {
            throw new System.NotImplementedException();
        }
    }
}