using UnityEngine;
using TMPro;

public class AnvilCopper : blockBase
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
            textDisplay.text = "F: Anvil Copper";
        
       
    }

    void Update()
    {
        if (isPlayerInRange && Input.GetKeyDown(KeyCode.F))
        {
            CallUI.instance.AnvilUI();
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
            CallUI.instance.UI[2].SetActive(false);
        }
    }
}
