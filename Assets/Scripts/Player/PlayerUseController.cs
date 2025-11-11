using System.Collections.Generic;
using UnityEngine;

public class PlayerUseController : MonoBehaviour
{
    public static PlayerUseController Instance;

    private ToolInstance equippedTool;
    public GameObject ToolInstanceObj;

    // Lưu trữ các tool đã tạo
    private Dictionary<Item, ToolInstance> toolInstances = new Dictionary<Item, ToolInstance>();

    private void Awake()
    {
        Instance = this;
    }

    public void EquipItem(Item item)
    {
        if (item == null || item.type != ItemType.Tools) return;

        // Tắt tool hiện tại
        if (equippedTool != null)
            equippedTool.gameObject.SetActive(false);

        // Nếu đã tồn tại tool instance thì bật lại
        if (toolInstances.TryGetValue(item, out ToolInstance existingTool))
        {
            equippedTool = existingTool;
            if (equippedTool != null)
            {
                equippedTool.gameObject.SetActive(true);
                Debug.Log("Đã equip lại tool: " + item.name);
                return;
            }
        }

        // Nếu chưa có thì tạo mới
        GameObject toolObj = new GameObject(item.name + "_Instance");
        equippedTool = toolObj.AddComponent<ToolInstance>();
        equippedTool.Init(item);
        toolObj.transform.SetParent(ToolInstanceObj.transform);

        // Thêm vào dictionary
        toolInstances[item] = equippedTool;

        Debug.Log("Đã equip tool mới: " + item.name);
    }
    

    public void TryUseTool(blockBase block)
    {
        if (equippedTool == null)
        {
            Debug.Log("Chưa có tool nào được equip");
            return;
        }

        Debug.Log($"Tool: {equippedTool.itemData.name} | Action: {equippedTool.itemData.actionType} | Level: {equippedTool.itemData.MiningLevel}");
        Debug.Log($"Block: {block.blockName} | Required: {block.requiredTool} | Level: {block.LevelBlock}");

        bool broken = equippedTool.UseTool();
        if (!broken)
        {
            block.TakeDamage(equippedTool.itemData.damageMine, equippedTool.itemData);
        }
        else
        {
            // Nếu tool hỏng thì remove trong inventory slot
            InventoryManager.instance.GetSelectedItem(true);
        }
    }

    /// <summary>
    /// Xóa tool khi durability = 0
    /// </summary>
    public void RemoveTool(Item item)
    {
        if (toolInstances.ContainsKey(item))
        {
            ToolInstance tool = toolInstances[item];
            if (tool != null)
                Destroy(tool.gameObject);

            toolInstances.Remove(item);

            if (equippedTool != null && equippedTool.itemData == item)
                equippedTool = null;

            Debug.Log(item.name + " đã bị remove khỏi dictionary");
        }
    }
}
