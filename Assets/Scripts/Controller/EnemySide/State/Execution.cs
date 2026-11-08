using UnityEngine;

namespace Enemy
{
    /// <summary>
    /// Enemy実行
    /// </summary>
    public class Execution : State
    {
        public Execution(EnemyController controller) : base(controller) { }
        public override void Start()
        {
            var no = _controller.Action.SelectExecution(_controller.Agent);
        }
        public override void Update()
        {
            //戦闘から逃げられたら
            if (!_controller.Area.IsBattle)
            {
                _controller.Action.MyAnimator.Play("Move");
                _controller.ChangeState(_controller.Idle);
                return;
            }

            //行動中
            if (_controller.Action.Execution(_controller.transform.position, _controller.target.point))
            {
                _controller.ChangeState(_controller.Wait);
                return;
            }
        }
    }
}