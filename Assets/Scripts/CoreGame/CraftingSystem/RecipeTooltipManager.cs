using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class RecipeTooltipManager : MonoBehaviour
{
    public static RecipeTooltipManager instance;

    public GameObject tooltipObj;
    public TextMeshProUGUI recipeNameText;
    public Transform ingredientListParent;
    public GameObject ingredientUIPrefab; // prefab có Icon + Text

    private bool isShowing = false;
    private Vector3 offset = new Vector3(20, -50, 0); // vị trí offset so với chuột

    private void Awake()
    {
        instance = this;
        HideTooltip();
    }

    private void Update()
    {
        if (isShowing)
        {
            // Tooltip follow chuột
            tooltipObj.transform.position = Input.mousePosition + offset;
        }
    }

    public void ShowTooltip(Recipe recipe, Vector3 position)
    {
        tooltipObj.SetActive(true);
        isShowing = true;

        recipeNameText.text = recipe.resultItem.itemName;

        // Clear cũ
        foreach (Transform child in ingredientListParent)
            Destroy(child.gameObject);

        // Add nguyên liệu mới
        foreach (var ing in recipe.ingredients)
        {
            GameObject obj = Instantiate(ingredientUIPrefab, ingredientListParent);
            Image icon = obj.transform.Find("Icon").GetComponent<Image>();
            TextMeshProUGUI text = obj.transform.Find("Text").GetComponent<TextMeshProUGUI>();

            icon.sprite = ing.item.image;

            int currentAmount = InventoryManager.instance.GetItemCount(ing.item);
            text.text = $"{ing.item.itemName} {currentAmount}/{ing.amount}";
            text.color = currentAmount >= ing.amount ? Color.green : Color.red;
        }

        // Đặt vị trí lần đầu (có offset để tránh đè pointer)
        tooltipObj.transform.position = position + offset;
    }

    public void HideTooltip()
    {
        tooltipObj.SetActive(false);
        isShowing = false;
    }
}
