using UnityEngine;

[CreateAssetMenu(fileName = "ChestData", menuName = "Inventory/ChestData")]
public class ChestData : ScriptableObject
{
    public string chestName = "New Chest";
    public int maxSlots = 12;
    public Item[] itemsInChest; // mảng item của chest

    private void OnEnable()
    {
        // Khởi tạo mảng nếu null
        if (itemsInChest == null || itemsInChest.Length != maxSlots)
            itemsInChest = new Item[maxSlots];
    }
}
