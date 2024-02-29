using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ArmorStoreScript : MonoBehaviour
{
    [SerializeField] private Image armorImage;
    [SerializeField] private TMP_Text armorName;


    // TODO: replace SO to class
    public void InitializeArmorStoreTemplate(ArmorDataSO data)
    {
        armorImage.sprite = data.GetArmorIcon();
        armorName.text = data.GetArmorName();
    }
}
