using UnityEngine;

[System.Serializable]
public class Ingredient
{
    public Item item;   // SO Item của mày
    public int amount;  // số lượng cần
}

[CreateAssetMenu(fileName = "New Recipe", menuName = "Scriptable Object/Recipe")]
public class Recipe : ScriptableObject
{
    public Ingredient[] ingredients; // nguyên liệu
    public Item resultItem;          // kết quả
    public int resultAmount = 1;     // số lượng tạo ra
}
