using UnityEngine;

public class DoubleChest : blockBase
{
    private ChestManager currentChest;

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            currentChest = GetComponent<ChestManager>();
            Debug.Log("Player near chest");
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            currentChest = null;
            Debug.Log("Player left chest");
        }
    }

    void Update()
    {
        if (currentChest != null && Input.GetKeyDown(KeyCode.F))
        {
            currentChest.ToggleChest();
        }
    }
}
