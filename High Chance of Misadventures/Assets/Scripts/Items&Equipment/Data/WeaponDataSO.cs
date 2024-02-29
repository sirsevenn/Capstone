using UnityEngine;

[CreateAssetMenu(fileName = "New Weapon", menuName = "ScriptableObjects/Inventory/Weapon")]
public class WeaponDataSO : ScriptableObject
{
    [SerializeField] private Sprite weaponIcon;
    [SerializeField] private string weaponName;
    [SerializeField] private int ATK;

    public Sprite GetWeaponIcon() => weaponIcon;

    public string GetWeaponName() => weaponName;

    public int GetAttackValue() => ATK;
}