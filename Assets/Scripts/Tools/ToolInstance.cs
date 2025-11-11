using UnityEngine;

public class ToolInstance : MonoBehaviour
{
    public Item itemData;
    public int currentDurability;
    public SelcetTools uiTool;

    private PlayerUseController controller;

  

    void Start()
    {
        if (uiTool == null)
            uiTool = FindAnyObjectByType<SelcetTools>();

        // Lấy tham chiếu controller
        controller = PlayerUseController.Instance;
    }

    private void Update()
    {
        if (uiTool != null)
            uiTool.UpdateDurability(currentDurability);
    }

    public void Init(Item data)
    {
        itemData = data;
        currentDurability = data.maxDurability;
    }

    /// <summary>
    /// Trả về true nếu tool đã hỏng
    /// </summary>
    public bool UseTool()
    {
        if (itemData.type != ItemType.Tools) return false;

        currentDurability--;
        Debug.Log(itemData.name + " durability: " + currentDurability);

        if (currentDurability <= 0)
        {
            Debug.Log(itemData.name + " đã hỏng!");

            // Gọi về controller thay vì Destroy trực tiếp
            if (controller != null)
                controller.RemoveTool(itemData);

            return true;
        }

        return false;
    }
}
