using UnityEngine;

/// <summary>
/// 
/// </summary>
public class StatusBase : MonoBehaviour
{
    [SerializeField] StatusData status;

    /// <summary>
    /// åªç›ÇÃHP
    /// </summary>
    protected float hp;

    public float Hp => hp;

    protected virtual void Start()
    {
        hp = status.MaxHp;
    }

    public virtual void Damage(float damage)
    {
        Debug.Log(damage);

    }

}
