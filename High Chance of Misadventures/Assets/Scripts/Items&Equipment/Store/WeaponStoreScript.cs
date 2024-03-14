using System;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WeaponStoreScript : MonoBehaviour
{
    [SerializeField] private Image weaponImage;
    [SerializeField] private TMP_Text ATK_text;
    [SerializeField] private TMP_Dropdown specialSkilllDropdown;
    [SerializeField] private TMP_Text skillDescriptionText;
    [SerializeField] private Button upgradeButton;

    public void SetupWeaponStoreTemplate(Weapon weapon)
    {
        weaponImage.sprite = weapon.GetWeaponData().GetWeaponIcon();
        ATK_text.text = weapon.GetTotalATK().ToString();

        var specialSkillTypes = Enum.GetNames(typeof(ETempSkillTypes));
        specialSkilllDropdown.ClearOptions();
        specialSkilllDropdown.AddOptions(specialSkillTypes.OfType<string>().ToList());

        if (weapon.GetSpecialSkillData() != null)
        {
            specialSkilllDropdown.value = (int)weapon.GetSpecialSkillData().GetSkillType();
            skillDescriptionText.text = weapon.GetSpecialSkillData().GetSkillDescription();
        }
        else
        {
            specialSkilllDropdown.value = 0;
            skillDescriptionText.text = "";
        }

        upgradeButton.onClick.AddListener(() => StoreManager.Instance.OnUpgradeWeapon(weapon.GetWeaponData().GetWeaponName()));
        specialSkilllDropdown.onValueChanged.AddListener((int value) => {
            StoreManager.Instance.OnSwitchSpecialSkill(weapon.GetWeaponData().GetWeaponName(), value, this);
        });
    }

    public void UpdateDescription(TempSpecialSkillDataSO skillData)
    {
        skillDescriptionText.text = (skillData != null) ? skillData.GetSkillDescription() : "";
    }
}
