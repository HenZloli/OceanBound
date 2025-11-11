using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Object/FurnaceRecipe")]
public class FurnaceRecipe : ScriptableObject
{
    public Item input;   // nguyên liệu
    public Item output;  // sản phẩm
    public float smeltTime = 5f; // thời gian nung riêng cho từng công thức
}