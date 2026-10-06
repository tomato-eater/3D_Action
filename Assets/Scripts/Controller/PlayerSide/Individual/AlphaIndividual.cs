using UnityEngine;

/// <summary>
/// アルファの攻撃・スキル
/// </summary>
public class AlphaIndividual : PlayableIndividualController
{
    public override float AttackDistance => 1.0f;

    public override int MaxCombo => 3;

    public override float MaxCharge => 0;

    public override bool ChargingMove => false;

    public override bool AutoChange => true;


    public override void OnHold(bool hold, float holdTime)
    {
        Debug.Log("holdB");
    }

    public override void Release(float holdTime)
    {
        Debug.Log("holdE");
    }
}
