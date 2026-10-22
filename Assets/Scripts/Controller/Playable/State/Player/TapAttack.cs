using UnityEngine;

namespace Playable.Player
{
    /// <summary>
    /// Player単押し攻撃
    /// </summary>
    public class TapAttack : State
    {
        public TapAttack(PlayerController controller) : base(controller) { }

        uint combo = 0;

        bool endAni = false;

        float time;

        public override void Start()
        {
            if (combo >= controller.Action.Mode.MaxCombo)
            {
                combo = 0;
                controller.ChangeState(controller.Idle);
                return;
            }


            controller.moveVector = Vector3.zero;
            Vector3 rot = controller.target ? controller.target.transform.position - controller.MyTransform.position : Vector3.zero;

            controller.MoveAndRotate(rot, 1.5f);

            controller.Action.Mode.TapAttack();

            combo++;
            endAni = false;
            time = 0;
        }

        public override void Update()
        {
            if (!endAni)
                endAni = controller.Action.GetTransferAnime;

            if (endAni)
            {
                time += controller.ElapsedTime();
                if (time > 0.1f)
                {
                    //回避入力検知
                    if (SGameManager.instance.ActivityMode == ActivityMode.Battle && controller.ChangeAvoid())
                    {
                        combo = 0;
                        controller.Action.Animator.SetTrigger("EndAttack");
                        return;
                    }

                    //攻撃入力検知
                    if (combo < controller.Action.Mode.MaxCombo && controller.Attack(out var comboAdd))
                    {
                        if (!comboAdd)
                        {
                            combo = 0;
                        }
                        return;
                    }
                }
                if (time > 0.15f)
                {
                    //攻撃入力状況をリセット
                    bool tapAttackTrigger = SInputSystem.Instance.TapAttackTrigger;

                    combo = 0;
                    controller.Action.Animator.SetTrigger("EndAttack");
                    controller.ChangeState(controller.Idle);
                    return;
                }
            }
        }
    }

}
