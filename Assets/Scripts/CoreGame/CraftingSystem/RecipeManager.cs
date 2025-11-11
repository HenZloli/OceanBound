using UnityEngine;

public class RecipeManager : MonoBehaviour
{
    public Transform recipePanel;        
    public GameObject recipeSlotPrefab;  
    public Recipe[] recipes;            

    void Start()
    {
        foreach (Recipe r in recipes)
        {
            GameObject slot = Instantiate(recipeSlotPrefab, recipePanel);
            slot.GetComponent<RecipeSlot>().Init(r);
        }
    }
}
