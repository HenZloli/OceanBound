using UnityEngine;
using TMPro;


[RequireComponent(typeof(SpriteRenderer))]
public class FurnaceBlock : blockBase
{
    public static FurnaceBlock instance;
    public SpriteRenderer spriteRenderer;
    [Header("UI Display")]
    [SerializeField] private GameObject uiDisplay;
    [SerializeField] private TextMeshProUGUI textDisplay;

    [Header("On Off")]

    public Sprite F_on;
    public Sprite F_off;
    public GameObject lightEffect;

    


    [Header("Flint Flake Settings")]

    private bool isPlayerInRange = false;

    protected override void Awake()
    {
        instance = this;
        base.Awake();
    }
    void Start()
    {
        if (uiDisplay != null)
            uiDisplay.SetActive(false);

        if (textDisplay != null)
            textDisplay.text = "F: Furnace";
        spriteRenderer = GetComponent<SpriteRenderer>();
       
    }

    void Update()
    {
        if (isPlayerInRange && Input.GetKeyDown(KeyCode.F))
        {
            CallUI.instance.Furnace_1();
        }
    }

    public void FurnaceOn()
    {
        spriteRenderer.sprite = F_on;
        lightEffect.SetActive(true);
    }
    public void FurnaceOff()
    {
        spriteRenderer.sprite = F_off;
        lightEffect.SetActive(false);
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
