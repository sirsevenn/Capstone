using UnityEngine;
using UnityEngine.EventSystems;

public class ItemDraggable : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [SerializeField] private CraftingMaterialSO material;

    public void SetMaterial(CraftingMaterialSO material)
    {
        this.material = material;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (material == null) return;

        CraftingSystem.Instance.OnBeginDragMaterial(material);
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (material == null) return;

        CraftingSystem.Instance.OnDragMaterial(material, eventData.position);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (material == null) return;

        CraftingSystem.Instance.OnEndDragMaterial(material, eventData.position);
    }
}