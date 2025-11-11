using UnityEngine;

public class ChestManager : MonoBehaviour
{
    public string chestName = "Wooden Chest";
    public int maxSlots = 12;
    public Item[] itemsInChest; // dữ liệu item riêng của chest

    private bool isOpen = false;

    void Awake()
    {
        if (itemsInChest == null || itemsInChest.Length != maxSlots)
            itemsInChest = new Item[maxSlots]; // tạo mảng mới nếu chưa có
    }

    public void ToggleChest()
    {
        if (isOpen) CloseChest();
        else OpenChest();
    }

    public void OpenChest()
    {
        isOpen = true;
        ChestUI.instance.OpenChest(this);
    }

    public void CloseChest()
    {
        isOpen = false;
        ChestUI.instance.CloseChest();
    }
}
