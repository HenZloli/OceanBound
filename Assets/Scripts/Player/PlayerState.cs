using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerState : MonoBehaviour
{
    [Header("UI Bars")]
    [SerializeField] private Image health_field;
    [SerializeField] private Image hunger_field;
    [SerializeField] private Image stamina_field;

    [SerializeField] private TextMeshProUGUI health_text;
    [SerializeField] private TextMeshProUGUI hunger_text;
    [SerializeField] private TextMeshProUGUI stamina_text;

    [Header("Base Stats")]
    [SerializeField] private int base_Max_Health = 10;
    [SerializeField] private float base_Max_Hunger = 10f;
    [SerializeField] private float base_Max_Stamina = 10f;

    [Header("Per Level Increase")]
    [SerializeField] private int health_per_level = 1;
    [SerializeField] private float hunger_per_level = 2f;
    [SerializeField] private float stamina_per_level = 1f;

    [Header("Current Level")]
    [SerializeField] private int level = 1;

    [Header("Stamina Settings")]
    [SerializeField] private float staminaRegenRate = 2f;  
    [SerializeField] private float regenDelay = 1.5f;       

    private float regenTimer = 0f; 
    private float staminaUpdateTimer = 0f;

    [SerializeField] private int currentHealth;
    [SerializeField] private float currentHunger;
    [SerializeField] private float currentStamina;

    public int Level => level;
    public int MaxHealth => base_Max_Health + (level - 1) * health_per_level;
    public float MaxHunger => base_Max_Hunger + (level - 1) * hunger_per_level;
    public float MaxStamina => base_Max_Stamina + (level - 1) * stamina_per_level;

    public float CurrentStamina => currentStamina;
    public float CurrentHunger => currentHunger;
    public int CurrentHealth => currentHealth;

    void Start()
    {
        currentHealth = MaxHealth;
        currentHunger = MaxHunger;
        currentStamina = MaxStamina;
        UpdateStates();
    }

    void Update()
    {
        
        if (currentStamina < MaxStamina)
            RegenStamina();

       
        if (Input.GetKeyDown(KeyCode.U))
            LevelUp();

        if (Input.GetKeyDown(KeyCode.H))
        {
            int dmg = Random.Range(3, 10);
            TakeDamage(dmg);
        }
    }

    public void LevelUp()
    {
        level++;
        currentHealth = MaxHealth;
        currentHunger = MaxHunger;
        currentStamina = MaxStamina;
        UpdateStates();
        Debug.Log($"🎉 Lên cấp {level}! Máu tối đa mới: {MaxHealth}");
    }

    public void TakeDamage(int amount)
    {
        int armor = ArmorManager.instance != null ? ArmorManager.instance.GetArmorValue() : 0;
        float reduction = Mathf.Clamp01(armor / 25f);
        int finalDamage = Mathf.RoundToInt(amount * (1f - reduction));

        currentHealth -= finalDamage;
        currentHealth = Mathf.Clamp(currentHealth, 0, MaxHealth);
        UpdateStates();

        Debug.Log($"Nhận {finalDamage} sát thương (Armor {armor}, giảm {reduction * 100f:0}% từ {amount})");

        if (currentHealth <= 0)
            Debug.Log("☠️ Player chết rồi!");
    }

    public void StaminaUse(float amountPerSecond)
    {
        currentStamina -= amountPerSecond * Time.deltaTime;
        currentStamina = Mathf.Clamp(currentStamina, 0, MaxStamina);
        regenTimer = regenDelay; 
        UpdateStates();
    }

    private void RegenStamina()
    {
        if (regenTimer > 0)
        {
            regenTimer -= Time.deltaTime;
            return;
        }

        
        if (currentStamina < MaxStamina && currentHunger > 0)
        {
            
            float hungerFactor = currentHunger / MaxHunger;
            float actualRegenRate = staminaRegenRate * Mathf.Lerp(0.3f, 1f, hungerFactor);

            
            currentStamina += actualRegenRate * Time.deltaTime;

           
            float hungerUse = 0.15f * actualRegenRate * Time.deltaTime; 
            currentHunger -= hungerUse;

           
            currentStamina = Mathf.Clamp(currentStamina, 0, MaxStamina);
            currentHunger = Mathf.Clamp(currentHunger, 0, MaxHunger);

            
            staminaUpdateTimer += Time.deltaTime;
            if (staminaUpdateTimer >= 0.1f)
            {
                UpdateStates();
                staminaUpdateTimer = 0f;
            }
        }
    }


    public void UpdateStates()
    {
        if (health_field)
            health_field.fillAmount = (float)currentHealth / MaxHealth;

        if (hunger_field)
            hunger_field.fillAmount = currentHunger / MaxHunger;

        if (stamina_field)
            stamina_field.fillAmount = currentStamina / MaxStamina;

        UpdateTexts();
    }

    private void UpdateTexts()
    {
        if (health_text) health_text.text = $"{currentHealth}/{MaxHealth}";
        if (hunger_text) hunger_text.text = $"{(int)currentHunger}/{(int)MaxHunger}";
        if (stamina_text) stamina_text.text = $"{(int)currentStamina}/{(int)MaxStamina}";
    }
}
