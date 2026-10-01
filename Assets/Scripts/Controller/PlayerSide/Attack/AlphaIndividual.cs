using UnityEngine;

/// <summary>
/// アルファの攻撃・スキル
/// </summary>
public class AlphaIndividual : PlayableIndividualController
{
    public override void Set(PlayableBehaviorController player)
    {
        StateApproach = new Player_Approach(player);
    }
}
