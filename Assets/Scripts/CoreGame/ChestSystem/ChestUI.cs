using UnityEngine;

public class ChestUI : MonoBehaviour
{
    public static ChestUI instance;

    public GameObject chestPanel;
    public GameObject inventoryPanel;
    public ChestSlot[] chestSlots;

    private ChestManager currentChest;

    void Awake()
    {
        instance = this;
        chestPanel.SetActive(false);
    }

    public void OpenChest(ChestManager chest)
    {
        currentChest = chest;
        chestPanel.SetActive(true);

        // Clear UI cũ
        foreach (var slot in chestSlots)
            foreach (Transform child in slot.transform)
                Destroy(child.gameObject);

        // Load item từ data
        for (int i = 0; i < chestSlots.Length; i++)
        {
            if (i < chest.itemsInChest.Length && chest.itemsInChest[i] != null)
                SpawnItemUI(i, chest.itemsInChest[i]);
        }

        if (!inventoryPanel.activeSelf)
            inventoryPanel.SetActive(true);

        Debug.Log("Mở chest: " + chest.chestName);
    }

    public void CloseChest()
    {
        // Chỉ clear UI, dữ liệu vẫn giữ nguyên
        foreach (var slot in chestSlots)
            foreach (Transform child in slot.transform)
                Destroy(child.gameObject);

        chestPanel.SetActive(false);
        inventoryPanel.SetActive(false);
        currentChest = null;
    }

    private void SpawnItemUI(int slotIndex, Item item)
    {
        GameObject prefab = Resources.Load<GameObject>("ChestItemPrefab");
        if (prefab == null)
        {
            Debug.LogError("Không tìm thấy ChestItemPrefab trong Resources!");
            return;
        }

        GameObject newItem = Instantiate(prefab, chestSlots[slotIndex].transform);
        ChestItem chestItem = newItem.GetComponent<ChestItem>();
        chestItem.InitialiseItem(item);
    }

    // Add item trực tiếp vào data + spawn UI
    public void AddItemToChest(int slotIndex, Item item)
    {
        if (currentChest == null) return;
        if (slotIndex < 0 || slotIndex >= currentChest.itemsInChest.Length) return;

        currentChest.itemsInChest[slotIndex] = item;
        SpawnItemUI(slotIndex, item);
    }

    // Remove item trực tiếp từ data + clear UI
    public void RemoveItemFromChest(int slotIndex)
    {
        if (currentChest == null) return;
        if (slotIndex < 0 || slotIndex >= currentChest.itemsInChest.Length) return;

        currentChest.itemsInChest[slotIndex] = null;
        foreach (Transform child in chestSlots[slotIndex].transform)
            Destroy(child.gameObject);
    }
}
