using System.Collections.Generic;
using UnityEngine;

namespace Playable
{
    /// <summary>
    /// Playableキャラクターのアクションベース
    /// </summary>
    public abstract class ActionControllerBase : MonoBehaviour
    {
        [Header("Parameter")]
        [SerializeField, Tooltip("武器データ(今はプレハブを直指定)")] GameObject _weaponData;

        [Header("Refer")]
        [SerializeField, Tooltip("AttackPoint")] Transform _attackPoint;
        [SerializeField, Tooltip("行いたい挙動")] List<ActionBase> _actions = new ();
        [SerializeField, Tooltip("武器の位置")] List<Transform> _weaponHolders = new ();
        [Tooltip("装備中の武器")] protected List<GameObject> _equippedWeapons = new ();
        [Tooltip("装備中の武器のアクセス先")] protected List<Weapon> _weaponAccess = new ();

        [Header("Component")]
        [SerializeField] Animator _animator;

        protected bool _endUnique;

        /// <summary>
        /// 読み取り専用　攻撃の基準点
        /// </summary>
        public Transform AttackPoint => _attackPoint;
        /// <summary>
        /// 読み取り専用　アニメーター
        /// </summary>
        public Animator MyAnimator => _animator;

        public ActionBase Mode
        {
            get
            {
                return _actions[0];
            }
        }

        /// <summary>
        /// アニメーション移行可能状況
        /// </summary>
        protected bool _transferAnime = false;
        /// <summary>
        /// アニメーション移行が可能になったことを受け取る
        /// </summary>
        public void SetTransferAnime() => _transferAnime = true;
        /// <summary>
        /// アニメーション移行が可能な事を伝える
        /// </summary>
        public bool GetTransferAnime
        {
            get
            {
                if (_transferAnime)
                {
                    _transferAnime = false;
                    return true;
                }
                return false;
            }
        }

        private void Start()
        {
            foreach (var action in _actions)
                action.Set(_animator);
        }

        /// <summary>
        /// 装備の切り替え
        /// </summary>
        public void EquipWeapon()
        {
            //既に装備している武器を削除
            foreach (var weapon in _equippedWeapons) if (weapon != null) Destroy(weapon);
            //装備数の調整
            while (_weaponHolders.Count > _equippedWeapons.Count) _equippedWeapons.Add(null);
            while (_weaponHolders.Count > _weaponAccess.Count) _weaponAccess.Add(null);
            //オブジェクトの生成・取得
            for (int i = 0; i < _weaponHolders.Count; i++)
            {
                var current = Instantiate(_weaponData, _weaponHolders[i]);
                //座標と方向を念のためリセット
                current.transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);

                _equippedWeapons[i] = current;

                if (current.TryGetComponent<Weapon>(out var weapon))
                {
                    _weaponAccess[i] = weapon;
                    _weaponAccess[i].Set(this);
                }
            }
        }

        /// <summary>
        /// 攻撃の有効化(アニメーションで呼び出し)
        /// </summary>
        /// <param name="no">武器番号</param>
        public void OnWeapon(int no)
        {
            var weapon = _weaponAccess[no];
            if (weapon == null) return;

            weapon.StartAttack();
        }
        /// <summary>
        /// 攻撃の無効化(アニメーションで呼び出し)
        /// </summary>
        /// <param name="no">武器番号</param>
        public void OffWeapon(int no)
        {
            var weapon = _weaponAccess[no];
            if (weapon == null) return;

            weapon.EndAttack();
        }
        /// <summary>
        /// ホールド攻撃(アニメーションで呼び出し)
        /// </summary>
        public void HoldAttack()
        {
            var mode = Mode;
            if (mode == null) return;

            mode.ReleaseAttack();
        }


        /// <summary>
        /// 固有技の開始
        /// </summary>
        public abstract void UniqueStart();

        /// <summary>
        /// 固有技の更新
        /// </summary>
        /// <param name="time">経過時間</param>
        /// <returns>自動で切り替わるか ? 替わる : 替わらない</returns>
        public abstract bool UniqueUpdate(float time);

        /// <summary>
        /// 固有技の終了
        /// </summary>
        public abstract void UniqueEnd();

        /// <summary>
        /// 固有技中に攻撃を喰らった
        /// </summary>
        /// <returns>ダメージを無効化するか ? する : しない</returns>
        public abstract bool Hit();

        public void EndUnique() => _endUnique = true;
    }
}