using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class RecipeSlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public Recipe recipe;
    public Image icon;

    public void Init(Recipe r)
    {
        recipe = r;
        icon.sprite = recipe.resultItem.image;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        RecipeTooltipManager.instance.ShowTooltip(recipe, Input.mousePosition);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        RecipeTooltipManager.instance.HideTooltip();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        CraftingSystem.instance.Craft(recipe);
    }
}
