using Player;
using UnityEngine;
using System.Collections.Generic;

namespace Playable.Alpha
{
    /// <summary>
    /// AlphaÇÃï®óùçUåÇÉÇÅ[Éh
    /// </summary>
    public class Attacker : ActionBase
    {
        public override int MaxCombo => 3;

        public override float AttackDistance => 0.75f;

        public override float MaxCharge => 0;

        public override bool ChargeMove => throw new System.NotImplementedException();

        [SerializeField] GameObject SlashObject;
        List<GameObject> slashObjects = new List<GameObject>();


        public override void TapAttackS(Weapon weapon = null)
        {
            if (weapon == null) return;

            weapon.StartAttack();
        }
        public override void TapAttackE(Weapon weapon = null)
        {
            if (weapon == null) return;

            weapon.EndAttack();
        }


        public override bool HoldAttack(float time)
        {
            chargeTime = time;
            return false;
        }

        public override void ReleaseAttack()
        {
            chargeTime = 0;

            foreach (var slashObject in slashObjects)
            {
                if (slashObject.activeSelf) continue;
                slashObject.transform.position = transform.position;
                slashObject.transform.rotation = transform.rotation;
                slashObject.SetActive(true);
                return;
            }

            var slash = Instantiate(SlashObject, transform.position, Quaternion.identity);
            slash.transform.rotation = transform.rotation;
            if (slash.TryGetComponent<Shockwave>(out var shockwave))
                if (transform.parent.TryGetComponent<PlayerController>(out var player))
                    shockwave.Set(player);

            slashObjects.Add(slash);
        }


        public override void Skill()
        {
            throw new System.NotImplementedException();
        }
    }
}