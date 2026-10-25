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
            if (combo >= _controller.Action.Mode.MaxCombo)
            {
                combo = 0;
                _controller.ChangeState(_controller.Idle);
                return;
            }


            _controller.MoveVector = Vector3.zero;
            Vector3 rot = _controller.target ? _controller.target.transform.position - _controller.MyTransform.position : Vector3.zero;

            _controller.MoveAndRotate(rot, 1.5f);

            _controller.Action.Mode.TapAttack();

            combo++;
            endAni = false;
            time = 0;
        }

        public override void Update()
        {
            if (!endAni)
                endAni = _controller.Action.GetTransferAnime;

            if (endAni)
            {
                time += _controller.ElapsedTime();
                if (time > 0.1f)
                {
                    //回避入力検知
                    if (SGameManager.instance.ActivityMode == ActivityMode.Battle && _controller.ChangeAvoid())
                    {
                        combo = 0;
                        _controller.Action.MyAnimator.SetTrigger("EndAttack");
                        return;
                    }

                    //攻撃入力検知
                    if (combo < _controller.Action.Mode.MaxCombo && _controller.NextAction(out var comboAdd))
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
                    _ = SInputSystem.Instance.TapAttackTrigger & SInputSystem.Instance.UniqueTrigger & SInputSystem.Instance.HoldAttack;

                    combo = 0;
                    _controller.Action.MyAnimator.SetTrigger("EndAttack");
                    _controller.ChangeState(_controller.Idle);
                    return;
                }
            }
        }
    }

}
