using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Object/AnvilRecipe")]
public class AnvilRecipe : ScriptableObject
{
    public Item[] Input;
    public Item Output;
}

