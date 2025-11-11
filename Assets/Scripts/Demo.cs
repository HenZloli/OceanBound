using UnityEngine;


public class Demo : MonoBehaviour
{
    public InventoryManager inventoryManager;

    public Item[] items;
    public Recipe recipeToCraft;

    void Selcet(int id)
    {
        bool result = inventoryManager.AddItem(items[id]);
        if (result == true)
            Debug.Log("ADD ITEM");
        else
            Debug.Log("ITEM NOT ADD");
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            int rd = Random.Range(0, items.Length);
            Selcet(rd);
        }
        
        
    }
    
    //     if (Input.GetKeyDown(KeyCode.P))
    //     {
    //         Item receivedItem = inventoryManager.GetSelectedItem(false);
    //         if (receivedItem != null)
    //             Debug.Log("da chon " + receivedItem);
    //         else
    //             Debug.Log("ko co item de chon");

    //     }
    //     else if (Input.GetKeyDown(KeyCode.U))
    //     {
    //         Item receivedItem = inventoryManager.GetSelectedItem(true);
    //         if (receivedItem != null)
    //             Debug.Log("dang su dung " + receivedItem);
    //         else
    //             Debug.Log("ko co item de chon");
    //     }
    // }
}
