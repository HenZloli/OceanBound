using UnityEngine;
using TMPro;
public class Bush : MonoBehaviour
{
    [Header("UI Display")]
    [SerializeField] private GameObject uiDisplay;
    [SerializeField] private TextMeshProUGUI textDisplay;

    [Header("Flint Flake Settings")]
    public GameObject[] dropItem;

    private bool isPlayerInRange = false;

    void Start()
    {
        if (uiDisplay != null)
            uiDisplay.SetActive(false);

        if (textDisplay != null)
            textDisplay.text = "F: Bush";
    }

    void Update()
    {
        if (isPlayerInRange && Input.GetKeyDown(KeyCode.F))
        {
            SpawnItem();
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

    void SpawnItem()
    {
        int rd_1 = Random.Range(1, 5);
        int rd_2 = Random.Range(1, 3);
        if (dropItem != null && dropItem.Length > 0)
        {
            for (int i = 0; i < rd_1; i++)
                Instantiate(dropItem[0], transform.position, Quaternion.identity);

            for (int i = 0; i < rd_2; i++)
                Instantiate(dropItem[1], transform.position, Quaternion.identity);

        }
    }
}
