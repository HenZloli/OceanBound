using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ChestSlot : MonoBehaviour, IDropHandler
{
    public Image background;

    public void OnDrop(PointerEventData eventData)
    {
        if (transform.childCount == 0) // slot trống
        {
            GameObject dragged = eventData.pointerDrag;
            if (dragged != null)
            {
                // Nhận cả InventoryItem hoặc ChestItem
                var invItem = dragged.GetComponent<InventoryItem>();
                var chestItem = dragged.GetComponent<ChestItem>();

                if (invItem != null || chestItem != null)
                {
                    dragged.GetComponent<MonoBehaviour>().GetComponent<RectTransform>().SetParent(transform);
                    if (invItem != null) invItem.parentAfterDrag = transform;
                    if (chestItem != null) chestItem.parentAfterDrag = transform;
                }
            }
        }
    }
}
