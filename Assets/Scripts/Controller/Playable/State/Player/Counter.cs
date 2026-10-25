using UnityEngine;

namespace Playable.Player
{
    /// <summary>
    /// Playerカウンター
    /// </summary>
    public class Counter : State
    {
        public Counter(PlayerController controller) : base(controller) { }

        public override void Start()
        {
            Debug.Log("a");

        }

        public override void Update()
        {

        }

    }

}