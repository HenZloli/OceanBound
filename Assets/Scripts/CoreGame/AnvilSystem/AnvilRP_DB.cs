using UnityEngine;
using System.Linq;

[CreateAssetMenu(menuName = "Scriptable Object/AnvilRecipeDatabase")]
public class AnvilRecipeDatabase : ScriptableObject
{
    public AnvilRecipe[] recipes;

    public AnvilRecipe GetRecipe(Item[] inputs)
    {
        foreach (var recipe in recipes)
        {
            // Nếu khác số lượng thì bỏ qua
            if (recipe.Input.Length != inputs.Length)
                continue;

            // Kiểm tra xem tất cả item của recipe có trong input hay không (bất kể thứ tự)
            bool allMatch = recipe.Input.All(req => inputs.Contains(req));

            if (allMatch)
                return recipe;
        }

        return null;
    }
}
