using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Object/FurnaceRecipeDatabase")]
public class FurnaceRecipeDatabase : ScriptableObject
{
    public FurnaceRecipe[] recipes;

    public FurnaceRecipe GetRecipe(Item input)
    {
        foreach (var recipe in recipes)
        {
            if (recipe.input == input)
                return recipe;
        }
        return null;
    }
}
