using UnityEngine;

[CreateAssetMenu(fileName = "StatusData", menuName = "Data/StatusData")]
public class StatusData : ScriptableObject
{
    [SerializeField] float maxHp;


    public float MaxHp => maxHp;
}
