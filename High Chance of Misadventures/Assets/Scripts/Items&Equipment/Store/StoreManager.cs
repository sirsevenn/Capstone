using System.Collections.Generic;
using UnityEngine;

public class StoreManager : MonoBehaviour
{
    #region Singleton
    public static StoreManager Instance;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        else
        {
            Instance = this;
        }
    }

    private void OnDestroy()
    {
        if (Instance != null && Instance == this)
        {
            Destroy(this.gameObject);
        }
    }
    #endregion

    [SerializeField] private GameObject armorInfoPanel;
    [SerializeField] private GameObject weaponsInfoPanel;
    [SerializeField] private GameObject itemsInfoPanel;

    // TEMP, should be classes not the data
    [SerializeField] private List<ArmorDataSO> tempArmorList;
    [SerializeField] private List<WeaponDataSO> tempWeaponsList;
    [SerializeField] private List<ItemDataSO> tempItemsList;


    private void InitializeStore()
    {

    }

    public void OnArmorButton()
    {

    }

    public void OnWeaponsButton()
    {

    }

    public void OnItemsButton()
    {

    }
}
