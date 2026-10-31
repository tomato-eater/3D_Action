using UnityEngine;

namespace Enemy
{
    /// <summary>
    /// Enemy発見
    /// </summary>
    public class Discovery : State
    {
        public Discovery(EnemyController controller) : base(controller) { }

        /// <summary>
        /// 仮の変数
        /// </summary>
        float timer = 0;

        public override void Start()
        {
            Debug.Log("ハッケン");

            _controller.Agent.isStopped = true;
            _controller.Area.BattleStart();
            _controller.BattleChanger(true);

            timer = 0;
        }

        public override void Update()
        {
            ////指定したアニメーションが再生されているかの確認
            //if (!controller.animator.GetCurrentAnimatorStateInfo(0).IsName(アニメーションの名前))
            //{
            //    return;
            //}
            ////指定したアニメーションが指定した割合まで再生されたか
            //if(controller.animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f)
            //{
            //    controller.ChangeState()
            //}

            ///------仮の処理
            timer += Time.deltaTime;
            if (timer > 1)
            {

            }
        }

        public override void End()
        {
            _controller.Agent.isStopped = false;

        }
    }
}