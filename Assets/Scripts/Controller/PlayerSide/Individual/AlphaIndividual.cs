using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// アルファの攻撃・スキル
/// </summary>
public class AlphaIndividual : PlayableIndividualController
{
    public override float AttackDistance => 0.75f;

    public override int MaxCombo => 3;

    public override float MaxCharge => 0;

    public override bool ChargingMove => false;

    public override bool AutoChange => true;

    [SerializeField] GameObject SlashObject;

    List<GameObject> slashObjects = new List<GameObject>();

    public override void OnHold(bool hold, float holdTime)
    {
        Debug.Log("holdB");
    }

    public override void Release(float holdTime)
    {
        Debug.Log("holdE");
    }

    /// <summary>
    /// アニメーターで呼び出し
    /// </summary>
    public void Slash()
    {
        for (int i = 0; i < slashObjects.Count; i++)
        {
            if (slashObjects[i].activeSelf) continue;

            slashObjects[i].transform.position = transform.position;
            slashObjects[i].transform.rotation = controller.transform.rotation;
            slashObjects[i].SetActive(true);
            return;
        }

        var slash = Instantiate(SlashObject, transform.position, Quaternion.identity);
        slash.transform.rotation = controller.transform.rotation;
        if (slash.TryGetComponent<Shockwave>(out var shockwave))
            shockwave.Set(controller);

        slashObjects.Add(slash);
    }
}
