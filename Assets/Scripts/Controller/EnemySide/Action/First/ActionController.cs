using UnityEngine;
using UnityEngine.AI;

namespace Enemy.First
{
    /// <summary>
    /// Firstのアクション
    /// </summary>
    public class ActionController : ActionControllerBase
    {
        NormalAttack _normalAttack = new();

        public override int SelectExecution(NavMeshAgent agent)
        {
            _agent = agent;
            _action = _normalAttack;

            _action.StartCall();
            return 0;
        }

        public override bool Execution(Vector3 nowPos, TargetPoint target)
        {
            return _action.Call(_agent, nowPos, target, MyAnimator);
        }
    }

    /// <summary>
    /// 通常攻撃・(接近して剣を振る)*1
    /// </summary>
    class NormalAttack : ActionBase
    {
        public override void StartCall()
        {
            _actionCount = 0;
            _callAnimator = false;
        }

        public override bool Call(NavMeshAgent agent, Vector3 nowPos, TargetPoint target, Animator animator)
        {
            switch (_actionCount)
            {
                case 0://接近
                    //初回の呼び出し
                    if (!_callAnimator)
                    {
                        PlayAnimator(animator, "Approach_1");

                        agent.speed = 20;
                        agent.acceleration = 50;
                        agent.isStopped = false;
                    }

                    //目的地
                    var targetPos = target.transform.position;
                    //目標ベクトル
                    var adjustment = Vector3.Scale(targetPos - nowPos, new Vector3(1, 0, 1)).normalized;

                    //微調整
                    var myPos = nowPos + adjustment * agent.radius;
                    targetPos -= adjustment * target.Radius;

                    //移動先の指定
                    agent.SetDestination(targetPos);

                    //十分な近さか
                    bool dis = Vector3.Distance(myPos, targetPos) <= 1.5f;

                    if(dis)
                    {
                        agent.isStopped = true;
                    }    
                    else
                    {
                        agent.isStopped = false;
                    }

                    //用意したアニメーターが再生仕切った && 十分な距離 of 速度が0 なら
                    if(_animeEvent)
                    {
                        EndAction();
                    }
                    break;

                case 1://剣を振った
                    //初回の呼び出し
                    if (!_callAnimator)
                    {
                        PlayAnimator(animator, "Attack_1");
                        agent.isStopped = true;
                    }
                    if (_animeEvent)
                    {
                        EndAction();
                    }
                    break;

                case 2://攻撃を終えた
                    PlayAnimator(animator, "EndAttack");
                    return true;

                default:
                    Debug.LogError("通る予定なし");
                    break;
            }
            return false;
        }
    }
}
