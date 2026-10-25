using UnityEngine;

namespace Playable.Player
{
    /// <summary>
    /// PlayerƒXƒLƒ‹
    /// </summary>
    public class Unique : State
    {
        public Unique(PlayerController controller) : base(controller) { }

        public override void Start()
        {
            _controller.Action.UniqueStart();
        }

        public override void Update()
        {
            if (_controller.Action.UniqueUpdate(_controller.ElapsedTime()))
            {
                _controller.ChangeState(_controller.Idle);
            }
            
        }

        public override void End()
        {
            
        }

    }
}
