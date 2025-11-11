using Unity.VisualScripting;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager instance;
    public Item[] startItem;
    public int MAX_STACK_ITEM = 10;
    public InventorySlot[] inventorySlots;
    public GameObject inventoryItemPrefab;



    int selectedSlot = -1;

    void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        ChangeSelectedSlot(0);
        foreach (var item in startItem)
        {
            AddItem(item);
        }
    }

    private void Update()
    {
        if (Input.inputString != null)
        {
            bool isNumber = int.TryParse(Input.inputString, out int number);
            if (isNumber && number > 0 && number < 10)
            {
                ChangeSelectedSlot(number - 1);
            }
        }
    }

    void ChangeSelectedSlot(int newValue)
    {
        if (selectedSlot >= 0)
        {
            inventorySlots[selectedSlot].Deselect();
        }

        inventorySlots[newValue].Select();
        selectedSlot = newValue;

        // 👇 gọi equip khi đổi slot
        InventoryItem itemInSlot = inventorySlots[selectedSlot].GetComponentInChildren<InventoryItem>();
        if (itemInSlot != null)
        {
            PlayerUseController puc = FindAnyObjectByType<PlayerUseController>();
            puc.EquipItem(itemInSlot.item);
        }
        else
        {
            // không có item ở slot thì clear tool
            PlayerUseController puc = FindAnyObjectByType<PlayerUseController>();
            puc.EquipItem(null);
        }
    }

    public bool AddItem(Item item)
    {
        // Kiểm tra nếu slot có sẵn item giống thì cộng dồn
        for (int i = 0; i < inventorySlots.Length; i++)
        {
            InventorySlot slot = inventorySlots[i];

            if (slot.slotType != SlotType.Inventory)
                continue; // bỏ qua slot không phải Inventory

            InventoryItem itemInSlot = slot.GetComponentInChildren<InventoryItem>();
            if (itemInSlot != null && itemInSlot.item == item &&
                itemInSlot.count < MAX_STACK_ITEM &&
                itemInSlot.item.Stackable == true)
            {
                itemInSlot.count++;
                itemInSlot.RefreshCount();
                return true;
            }
        }

        // Nếu không cộng dồn được thì tìm slot trống trong Inventory
        for (int i = 0; i < inventorySlots.Length; i++)
        {
            InventorySlot slot = inventorySlots[i];

            if (slot.slotType != SlotType.Inventory)
                continue;

            InventoryItem itemSlot = slot.GetComponentInChildren<InventoryItem>();
            if (itemSlot == null)
            {
                SpawnNewItem(item, slot);
                return true;
            }
        }

        return false;
    }

    public void SpawnNewItem(Item item, InventorySlot slot)
    {
        GameObject newItemGo = Instantiate(inventoryItemPrefab, slot.transform);
        InventoryItem inventoryItem = newItemGo.GetComponent<InventoryItem>();
        inventoryItem.InitialiseItem(item);
    }

    public Item GetSelectedItem(bool use)
    {
        InventorySlot slot = inventorySlots[selectedSlot];
        InventoryItem itemInSlot = slot.GetComponentInChildren<InventoryItem>();
        if (itemInSlot != null)
        {
            Item item = itemInSlot.item;
            if (use == true)
            {
                itemInSlot.count--;
                if (itemInSlot.count <= 0)
                {
                    Destroy(itemInSlot.gameObject);
                }
                else
                {
                    itemInSlot.RefreshCount();
                }
            }
            return item;
        }
        return null;
    }

    #region Crating Item
    // kiem tra so luong item
    public int GetItemCount(Item targetItem)
    {
        int total = 0;
        foreach (var slot in inventorySlots)
        {
            InventoryItem itemInSlot = slot.GetComponentInChildren<InventoryItem>();
            if (itemInSlot != null && itemInSlot.item == targetItem)
            {
                total += itemInSlot.count;
            }
        }
        return total;
    }

    // check có đủ số lượng để craft
    public bool HasEnoughItem(Item targetItem, int requiredAmount)
    {
        return GetItemCount(targetItem) >= requiredAmount;
    }

    // Trừ item trong inventory
    public bool RemoveItem(Item targetItem, int amount)
    {
        int remaining = amount;

        foreach (var slot in inventorySlots)
        {
            InventoryItem itemInSlot = slot.GetComponentInChildren<InventoryItem>();
            if (itemInSlot != null && itemInSlot.item == targetItem)
            {
                if (itemInSlot.count > remaining)
                {
                    // slot này đủ, chỉ trừ bớt
                    itemInSlot.count -= remaining;
                    itemInSlot.RefreshCount();
                    return true;
                }
                else
                {
                    // slot này không đủ, trừ hết và xóa object
                    remaining -= itemInSlot.count;
                    Destroy(itemInSlot.gameObject);

                    // nếu mày có hàm clear slot thì gọi ở đây
                    // slot.ClearSlot();

                    if (remaining <= 0)
                        return true; // đã trừ đủ
                }
            }
        }

        // Nếu chạy hết vòng for mà vẫn còn thiếu
        return false;
    }

    public bool HasTool(Item tool)
    {
        foreach (var slot in inventorySlots)
        {
            InventoryItem invItem = slot.GetComponentInChildren<InventoryItem>();
            if (invItem != null && invItem.item == tool)
                return true;
        }
        return false;
    }


    #endregion
    

    public InventorySlot GetEmptySlot()
    {
        foreach (var slot in inventorySlots)
        {
            if (slot.transform.childCount == 0 && slot.slotType == SlotType.Inventory)
                return slot;
        }
        return null;
    }


}
