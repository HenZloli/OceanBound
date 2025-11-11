using System;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(menuName = "Scriptable Object/Item")]
public class Item : ScriptableObject
{
    public string ItemID;
    public string itemName;
    [Header("Only Gameplay")]
    public GameObject block_prefab;
    public ItemType type;
    public float burnTime;

    [Header("Only Tools")]
    public ActionType actionType;
    public int MiningLevel;
    public int maxDurability;
    public int damageMine;

    [Header("Only Armor")]
    public bool isArmor;
    public ArmorType armorType;
    public int armorValue;


    [Header("Only UI")]
    public bool Stackable = true;

    [Header("Both")]
    public Sprite image;
}

public enum ItemType
{
    None,
    Item,
    Block,
    Tools,
    Food,
    Armor
}

public enum ActionType
{
    None,
    AXE,
    PICKAXE,
    HOE
}

public enum ArmorType
{
    None,
    Helmet,
    Chestplate,
    Leggings,
    Boots
}