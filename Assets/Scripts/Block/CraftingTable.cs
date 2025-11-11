using UnityEngine;
using TMPro;
using Unity.VisualScripting;

public class CraftingTable : blockBase
{
    [Header("UI Display")]
    [SerializeField] private GameObject uiDisplay;
    [SerializeField] private TextMeshProUGUI textDisplay;
    private bool isPlayerInRange = false;

    void Start()
    {
        if (uiDisplay != null)
            uiDisplay.SetActive(false);

        if (textDisplay != null)
            textDisplay.text = "F: Crafting Table";
        
       
    }

    void Update()
    {
        if (isPlayerInRange && Input.GetKeyDown(KeyCode.F))
        {
            CallUI.instance.CratingTaleUI();
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isPlayerInRange = true;
            uiDisplay.SetActive(true);
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isPlayerInRange = false;
            uiDisplay.SetActive(false);
            CallUI.instance.UI[1].SetActive(false);
        }
    }
}
