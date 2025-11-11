using TMPro;
using UnityEngine;

public class ArmorManager : MonoBehaviour
{
    public static ArmorManager instance;

    [Header("Armor Slots")]
    public InventorySlot helmetSlot;
    public InventorySlot chestSlot;
    public InventorySlot leggingsSlot;
    public InventorySlot bootsSlot;

    [Header("UI")]
    public TextMeshProUGUI armorValueText;

    private int totalArmorValue = 0;

    void Awake()
    {
        instance = this;
    }

    public void EquipArmor(InventoryItem item)
    {
        if (item == null || item.item == null) return;

        AddArmor(item.item.armorValue);
        Debug.Log($"[ArmorManager] Trang bị {item.item.name}, +{item.item.armorValue} giáp (Tổng: {totalArmorValue})");
    }

    public void UnequipArmor(InventoryItem item)
    {
        if (item == null || item.item == null) return;

        RemoveArmor(item.item.armorValue);
        Debug.Log($"[ArmorManager] Gỡ {item.item.name}, -{item.item.armorValue} giáp (Tổng: {totalArmorValue})");
    }

    private void AddArmor(int value)
    {
        totalArmorValue += value;
        UpdateArmorUI();
    }

    private void RemoveArmor(int value)
    {
        totalArmorValue -= value;
        if (totalArmorValue < 0) totalArmorValue = 0;
        UpdateArmorUI();
    }

    public int GetArmorValue() => totalArmorValue;

    private void UpdateArmorUI()
    {
        if (armorValueText != null)
            armorValueText.text = $"Armor: {totalArmorValue}";
    }
}
