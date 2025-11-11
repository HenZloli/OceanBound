using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class AnvilCore_Copper : MonoBehaviour
{
    [Header("UI Pane")]
    [SerializeField] private GameObject[] Pane; // UI của từng loại: Axe, Pickaxe,...

    [Header("Slots")]
    [SerializeField] private InventorySlot[] axeSlots;
    [SerializeField] private InventorySlot[] pickaxeSlots;
    [SerializeField] private InventorySlot[] helmetSlots;
    [SerializeField] private InventorySlot[] chestplateSlots;
    [SerializeField] private InventorySlot[] leggingSlots;
    [SerializeField] private InventorySlot[] bootsSlots;
    [SerializeField] private InventorySlot Output;

    [Header("Buttons")]
    [SerializeField] private Button btn_Craft;
    [SerializeField] private Button[] btn_selectUI;

    [Header("Database")]
    [SerializeField] private AnvilRecipeDatabase[] recipeDatabase;

    private bool isProcessing = false;
    private int currentPaneIndex = 0;

    void Start()
    {
        btn_Craft.onClick.AddListener(TryCraftActivePane);

        for (int i = 0; i < btn_selectUI.Length; i++)
        {
            int index = i; 
            btn_selectUI[i].onClick.AddListener(() => ChangeUI(index));
        }

        ChangeUI(0);
    }

    void ChangeUI(int index)
    {
        for (int i = 0; i < Pane.Length; i++)
            if (Pane[i] != null)
                Pane[i].SetActive(i == index);

        currentPaneIndex = index;
    }

    void TryCraftActivePane()
    {
        switch (currentPaneIndex)
        {
            case 0:
                TryToAnvil(axeSlots, recipeDatabase[0]);
                break;
            case 1:
                TryToAnvil(pickaxeSlots, recipeDatabase[1]);
                break;
            case 2:
                TryToAnvil(helmetSlots, recipeDatabase[2]);
                break;
            case 3:
                TryToAnvil(chestplateSlots, recipeDatabase[3]);
                break;
            case 4:
                TryToAnvil(leggingSlots, recipeDatabase[4]);
                break;
            case 5:
                TryToAnvil(bootsSlots, recipeDatabase[5]);
                break;
        }
    }

    void TryToAnvil(InventorySlot[] slots, AnvilRecipeDatabase dbR)
    {
        if (isProcessing || Output.transform.childCount > 0) return;

        // Lấy tất cả item thật sự có trong slot
        Item[] inputItems = slots
            .Select(s => s.GetComponentInChildren<InventoryItem>())
            .Where(i => i != null)
            .Select(i => i.item)
            .ToArray();

        if (inputItems.Length == 0) return;

        var recipe = dbR.GetRecipe(inputItems);
        if (recipe == null) return;

        isProcessing = true;

        
        foreach (var slot in slots)
            ClearSlot(slot);

        CreateOutput(recipe.Output);

        Invoke(nameof(ResetProcess), 0.5f);
    }

    void ResetProcess() => isProcessing = false;

    private void ClearSlot(InventorySlot slot)
    {
        InventoryItem child = slot.GetComponentInChildren<InventoryItem>();
        if (child != null)
        {
            child.count--;
            if (child.count <= 0)
                Destroy(child.gameObject);
            else
                child.RefreshCount();
        }
    }

    private void CreateOutput(Item outputItem)
    {
        if (Output.transform.childCount > 0) return;
        InventoryManager.instance.SpawnNewItem(outputItem, Output);
    }
}
