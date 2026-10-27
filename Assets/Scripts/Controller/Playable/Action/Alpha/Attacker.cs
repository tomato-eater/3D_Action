using System.Collections.Generic;
using UnityEngine;

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
        private List<GameObject> _slashObjects = new ();


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
            foreach (var slashObject in _slashObjects)
            {
                if (slashObject.activeSelf) continue;

                slashObject.transform.SetPositionAndRotation(transform.position, transform.rotation);
                slashObject.SetActive(true);
                return;
            }
            //無ければ新しく作成、登録
            var slash = Instantiate(SlashObject, transform.position, Quaternion.identity);
            slash.transform.rotation = transform.rotation;
            if (slash.TryGetComponent<Shockwave>(out var shockwave))
            {
                if (TryGetComponent<ActionControllerBase>(out var action))
                {
                    shockwave.Set(action.ElapsedTime);
                    Debug.Log("npcを追加した場合、ここが問題になる");
                }
            }

            _slashObjects.Add(slash);
        }

    }
}