using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.Mathematics;

public class SelcetTools : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;
    public Item item;

    public Image imageShowItem;
    public GameObject panelShowItem;
    public TextMeshProUGUI DurabilityText;
    public TextMeshProUGUI MinrLvText;
    public TextMeshProUGUI NameToolText;

    void Start()
    {
        if (panelShowItem != null)
            panelShowItem.SetActive(false);

        if (DurabilityText != null)
            DurabilityText.text = "";

        if (MinrLvText != null)
            MinrLvText.text = "";

        if (NameToolText != null)
            NameToolText.text = "";
    }

    void Update()
    {
        SetSelectedTool(InventoryManager.instance.GetSelectedItem(false));
        SetSelectedItem(InventoryManager.instance.GetSelectedItem(false));
    }

    void SetSelectedTool(Item newItem)
    {
        if (newItem == null || newItem.type != ItemType.Tools)
        {
            spriteRenderer.sprite = null;
            imageShowItem.sprite = null;
            item = null;
            panelShowItem.SetActive(false);
            return;
        }

        item = newItem;

        if (item.type == ItemType.Tools)
            panelShowItem.SetActive(true);

        spriteRenderer.sprite = item.image;
        imageShowItem.sprite = item.image;
        MinrLvText.text = "MineLv: " + item.MiningLevel.ToString();
        NameToolText.text = item.itemName;
    }

    void SetSelectedItem(Item newItem)
    {
        if (newItem == null)
        {
            spriteRenderer.sprite = null;
            item = null;
            return;
        }
        item = newItem;
        spriteRenderer.sprite = item.image;
    }

    
    public void UpdateDurability(int durability)
    {
        if (item == null || item.type != ItemType.Tools) return;

        DurabilityText.text = durability + " / " + item.maxDurability;
    }
}
