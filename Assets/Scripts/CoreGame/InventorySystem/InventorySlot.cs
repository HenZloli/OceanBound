using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public enum SlotType
{
    None,
    Inventory,
    Input,
    Output,
    Helmet,
    Chestplate,
    Leggings,
    Boots,
    Trash
}

public class InventorySlot : MonoBehaviour, IDropHandler
{
    public Sprite[] SlotImage;
    public SlotType slotType;
    public Image image;
    public Color selectedColor, notSelectedColor;

    private void Awake()
    {
        Deselect();
    }

    void Start()
    {
        if (slotType == SlotType.Boots)
            image.sprite = SlotImage[0];
        if (slotType == SlotType.Leggings)
            image.sprite = SlotImage[1];
        if (slotType == SlotType.Chestplate)
            image.sprite = SlotImage[2];
        if (slotType == SlotType.Helmet)
            image.sprite = SlotImage[3];
        if (slotType == SlotType.Input)
            image.sprite = SlotImage[4];
        if (slotType == SlotType.Output)
            image.sprite = SlotImage[5];
        if (slotType == SlotType.Trash)
            image.sprite = SlotImage[6];
    }

    public void Select()
    {
        image.color = selectedColor;
    }

    public void Deselect()
    {
        image.color = notSelectedColor;
    }

    public void OnDrop(PointerEventData eventData)
    {
        InventoryItem inventoryItem = eventData.pointerDrag.GetComponent<InventoryItem>();
        if (inventoryItem == null) return;

      

        switch (slotType)
        {
            case SlotType.Inventory:
                if (transform.childCount == 0)
                    inventoryItem.parentAfterDrag = transform;
                break;

            case SlotType.Input:
                if (transform.childCount == 0) // chỉ cho quặng/gỗ...
                    inventoryItem.parentAfterDrag = transform;
                break;

            case SlotType.Output:
            if (inventoryItem.parentAfterDrag == transform)
                return;

            Debug.Log("Không thể thả item vào Output Slot!");
            // inventoryItem.transform.SetParent(inventoryItem.parentAfterDrag); // trả lại vị trí cũ
            // inventoryItem.transform.localPosition = Vector3.zero;
            break;

            case SlotType.Helmet:
                if (inventoryItem.item.armorType == ArmorType.Helmet && transform.childCount == 0)
                    inventoryItem.parentAfterDrag = transform;
                break;

            case SlotType.Chestplate:
                if (inventoryItem.item.armorType == ArmorType.Chestplate && transform.childCount == 0)
                    inventoryItem.parentAfterDrag = transform;
                break;

            case SlotType.Leggings:
                if (inventoryItem.item.armorType == ArmorType.Leggings && transform.childCount == 0)
                    inventoryItem.parentAfterDrag = transform;
                break;

            case SlotType.Boots:
                if (inventoryItem.item.armorType == ArmorType.Boots && transform.childCount == 0)
                    inventoryItem.parentAfterDrag = transform;
                break;

            case SlotType.Trash:
                Destroy(inventoryItem.gameObject);
                PlayerUseController.Instance.RemoveTool(inventoryItem.item);
                Debug.Log("Item đã bị xóa trong Trash slot!");
                break;

            case SlotType.None:
                Debug.Log("Slot này chưa được gán loại!");
                break;
        }
    }
}
