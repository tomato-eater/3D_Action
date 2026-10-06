using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Playableキャラの攻撃やスキルの基盤
/// </summary>
public abstract class PlayableIndividualController : MonoBehaviour
{
    protected PlayableBehaviorController controller;
    /// <summary>
    /// コントローラー、各ステートの登録
    /// </summary>
    /// <param name="player"></param>
    public virtual void Set(PlayableBehaviorController player)
    {
        controller = player;
        EquipWeapon();
    }
    /// <summary>
    /// 攻撃周りのアニメーションが終了したか
    /// </summary>
    protected  bool endAnimator = false;
    /// <summary>
    /// 終了したことを受け取る要
    /// </summary>
    public virtual void SetEndAnimator() => endAnimator = true;
    /// <summary>
    /// 終了したことを伝える要
    /// </summary>
    public virtual bool EndAnimator
    {
        get
        {
            if (endAnimator)
            {
                endAnimator = false;
                return true;
            }
            return false;
        }
    }

    [SerializeField] GameObject weaponPre;
    [Header("武器の基準ホルダー")]
    [SerializeField] List<Transform> weaponHolder = new List<Transform>();
    /// <summary>
    /// 現在装備してる武器
    /// </summary>
    protected List<GameObject> currentWeaponObjects = new List<GameObject>();
    /// <summary>
    /// 武器のアクセス先
    /// </summary>
    protected List<Weapon> weapons = new List<Weapon>();
    /// <summary>
    /// 装備の切り替え
    /// </summary>
    public virtual void EquipWeapon()
    {
        //既に装備している武器を削除
        foreach (var weapon in currentWeaponObjects) if (weapon != null) Destroy(weapon);
        //装備数の調整
        while (weaponHolder.Count > currentWeaponObjects.Count) currentWeaponObjects.Add(null);
        while (weaponHolder.Count > weapons.Count) weapons.Add(null);

        for(int i = 0; i < weaponHolder.Count; i++)
        {
            var current = Instantiate(weaponPre, weaponHolder[i]);
            current.transform.localPosition = Vector3.zero;
            current.transform.localRotation = Quaternion.identity;

            currentWeaponObjects[i] = current;

            if(current.TryGetComponent<Weapon>(out var weapon))
            {
                weapons[i] = weapon;
            }
        }
    }
    /// <summary>
    /// 攻撃の有効化
    /// </summary>
    /// <param name="no">武器の指定</param>
    public virtual void OnAttack(int no)
    {
        if (weapons.Count <= no) return;
        weapons[no].StartAttack();
    }
    /// <summary>
    /// 攻撃の無効化
    /// </summary>
    /// <param name="no">武器の指定</param>
    public virtual void OffAttack(int no)
    {
        if (weapons.Count <= no) return;
        weapons[no].EndAttack();
    }

    /// <summary>
    /// 攻撃時の許容距離    0の場合そもそも接近しない
    /// </summary>
    public abstract float AttackDistance {  get; }
    /// <summary>
    /// 最大コンボ
    /// </summary>
    public abstract int MaxCombo {  get; }
    /// <summary>
    /// 長押しの最大時間
    /// </summary>
    public abstract float MaxCharge { get; }
    /// <summary>
    /// 長押し時、動けるか
    /// </summary>
    public abstract bool ChargingMove {  get; }
    /// <summary>
    /// 自動切り替え
    /// </summary>
    public abstract bool AutoChange {  get; }
    /// <summary>
    /// 長押し時に実行
    /// </summary>
    /// <param name="hold">長押し開始</param>
    /// <param name="holdTime">押してる時間</param>
    public abstract void OnHold(bool hold, float holdTime);
    /// <summary>
    /// 長押し後離した
    /// </summary>
    /// <param name="holdTime">長押ししてた時間</param>
    public abstract void Release(float holdTime);
}
