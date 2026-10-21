using System.Collections.Generic;
using UnityEngine;

namespace Playable
{
    /// <summary>
    /// Playableキャラクターのアクションベース
    /// </summary>
    public class ActionController : MonoBehaviour
    {
        [Header("Parameter")]
        [SerializeField, Tooltip("武器データ(今はプレハブを直指定)")] protected GameObject weaponData;

        [Header("Refer")]
        [SerializeField, Tooltip("AttackPoint")] protected Transform attackPoint;
        [SerializeField, Tooltip("行いたい挙動")] protected List<ActionBase> actions = new List<ActionBase>();
        [SerializeField, Tooltip("武器の位置")] protected List<Transform> weaponHolders = new List<Transform>();
        [Tooltip("装備中の武器")] protected List<GameObject> equippedWeapons = new List<GameObject>();
        [Tooltip("装備中の武器のアクセス先")] protected List<Weapon> weaponAccess = new List<Weapon>();

        [Header("Component")]
        [SerializeField] protected Animator animator;

        /// <summary>
        /// 読み取り専用　攻撃の基準点
        /// </summary>
        public Transform AttackPoint => attackPoint;
        /// <summary>
        /// 読み取り専用　アニメーター
        /// </summary>
        public Animator Animator => animator;

        public ActionBase Mode
        {
            get
            {
                return actions[0];
            }
        }

        /// <summary>
        /// アニメーション移行可能状況
        /// </summary>
        protected bool transferAnime = false;
        /// <summary>
        /// アニメーション移行が可能になったことを受け取る
        /// </summary>
        public void SetTransferAnime() => transferAnime = true;
        /// <summary>
        /// アニメーション移行が可能な事を伝える
        /// </summary>
        public bool GetTransferAnime
        {
            get
            {
                if (transferAnime)
                {
                    transferAnime = false;
                    return true;
                }
                return false;
            }
        }

        /// <summary>
        /// 装備の切り替え
        /// </summary>
        public void EquipWeapon()
        {
            //既に装備している武器を削除
            foreach (var weapon in equippedWeapons) if (weapon != null) Destroy(weapon);
            //装備数の調整
            while (weaponHolders.Count > equippedWeapons.Count) equippedWeapons.Add(null);
            while (weaponHolders.Count > weaponAccess.Count) weaponAccess.Add(null);
            //オブジェクトの生成・取得
            for (int i = 0; i < weaponHolders.Count; i++)
            {
                var current = Instantiate(weaponData, weaponHolders[i]);
                current.transform.localPosition = Vector3.zero;
                current.transform.localRotation = Quaternion.identity;

                equippedWeapons[i] = current;

                if (current.TryGetComponent<Weapon>(out var weapon))
                {
                    weaponAccess[i] = weapon;
                    weaponAccess[i].Set(this);
                }
            }
        }

        /// <summary>
        /// 攻撃の有効化(アニメーション管理)
        /// </summary>
        /// <param name="no">武器番号</param>
        public void OnAttack(int no)
        {
            var mode = Mode;
            if (mode == null) return;

            mode.TapAttackS(weaponAccess[no]);
            
        }
        /// <summary>
        /// 攻撃の無効化(アニメーション管理)
        /// </summary>
        /// <param name="no">武器番号</param>
        public void OffAttack(int no)
        {
            var mode = Mode;
            if (mode == null) return;

            mode.TapAttackE(weaponAccess[no]);
            
        }
    }
}