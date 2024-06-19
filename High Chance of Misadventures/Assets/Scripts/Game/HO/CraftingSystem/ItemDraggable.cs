using UnityEngine;
using UnityEngine.EventSystems;

public class ItemDraggable : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [SerializeField] private CraftingMaterialSO materialData;

    public void SetMaterial(CraftingMaterialSO material)
    {
        materialData = material;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (materialData == null) return;

        CraftingSystem.Instance.OnBeginDragMaterial(materialData);
        CraftingSystem.Instance.OnUpdateBookDisplay(materialData);
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (materialData == null) return;

        CraftingSystem.Instance.OnDragMaterial(materialData, eventData.position);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (materialData == null) return;

        CraftingSystem.Instance.OnEndDragMaterial(materialData, eventData.position);
    }
}