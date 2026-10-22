using UnityEngine;
using System.Collections.Generic;

namespace Playable.Alpha
{
    /// <summary>
    /// Alphaの物理攻撃モード
    /// </summary>
    public class Attacker : ActionBase
    {
        public override int MaxCombo => 3;

        public override float AttackDistance => 0.75f;

        public override float MaxHoldTime => 0;

        public override bool ChargeMove => false;

        [SerializeField] GameObject SlashObject;
        List<GameObject> slashObjects = new List<GameObject>();


        public override bool HoldAttack(float time)
        {
            //ホールドの必要がない
            MyAnimator.SetTrigger("HoldAttack");
            return true;
        }

        public override void ReleaseAttack()
        {
        }

        /// <summary>
        /// ホールド攻撃の斬撃を出す
        /// </summary>
        public void HoldSlash()
        {
            //未使用斬撃があれば再利用
            foreach (var slashObject in slashObjects)
            {
                if (slashObject.activeSelf) continue;
                slashObject.transform.position = transform.position;
                slashObject.transform.rotation = transform.rotation;
                slashObject.SetActive(true);
                return;
            }
            //無ければ新しく作成、登録
            var slash = Instantiate(SlashObject, transform.position, Quaternion.identity);
            slash.transform.rotation = transform.rotation;
            if (slash.TryGetComponent<Shockwave>(out var shockwave))
            {
                if (transform.parent.TryGetComponent<Player.PlayerController>(out var player))
                {
                    shockwave.Set(player);
                    Debug.Log("npcを追加した場合、ここが問題になる");
                }
            }

            slashObjects.Add(slash);
        }

        public override void HoldSkill(float time)
        {
            throw new System.NotImplementedException();
        }

        public override void ReleaseSkill()
        {
            throw new System.NotImplementedException();
        }

    }
}