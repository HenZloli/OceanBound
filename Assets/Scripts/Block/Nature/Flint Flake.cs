using UnityEngine;
using TMPro;

public class FlintFlake : MonoBehaviour
{
    [Header("UI Display")]
    [SerializeField] private GameObject uiDisplay;
    [SerializeField] private TextMeshProUGUI textDisplay;

    [Header("Flint Flake Settings")]
    public Item flint_falake;

    private bool isPlayerInRange = false;

    void Start()
    {
        if (uiDisplay != null)
            uiDisplay.SetActive(false);

        if (textDisplay != null)
            textDisplay.text = "F: " + flint_falake.itemName;
    }

    void Update()
    {
        if (isPlayerInRange && Input.GetKeyDown(KeyCode.F))
        {
            textDisplay.text = $"Nhặt {flint_falake.itemName}";
            InventoryManager.instance.AddItem(flint_falake);
            Destroy(gameObject);
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
        }
    }
}
