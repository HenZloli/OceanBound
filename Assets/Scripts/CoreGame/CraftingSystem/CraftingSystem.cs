using UnityEngine;

public class CraftingSystem : MonoBehaviour
{
    public static CraftingSystem instance;
    private void Awake() { instance = this; }

    public bool CanCraft(Recipe recipe)
    {
        foreach (var ingredient in recipe.ingredients)
        {
            if (!InventoryManager.instance.HasEnoughItem(ingredient.item, ingredient.amount))
                return false;
        }
        return true;
    }

    public bool Craft(Recipe recipe)
    {
        if (!CanCraft(recipe))
        {
            Debug.Log("Thiếu nguyên liệu để chế tạo!");
            return false;
        }

        // Nếu item là Tools và đã có rồi thì chặn luôn
        if (recipe.resultItem.type == ItemType.Tools && InventoryManager.instance.HasTool(recipe.resultItem))
        {
            Debug.Log("⚠ Bạn đã có " + recipe.resultItem.name + " rồi, không thể chế thêm!");
            return false;
        }

        // Trừ nguyên liệu
        foreach (var ingredient in recipe.ingredients)
        {
            InventoryManager.instance.RemoveItem(ingredient.item, ingredient.amount);
        }

        // Thêm item kết quả
        for (int i = 0; i < recipe.resultAmount; i++)
        {
            InventoryManager.instance.AddItem(recipe.resultItem);
        }

        Debug.Log("Chế tạo thành công: " + recipe.resultItem.name);
        return true;
    }


    
}
