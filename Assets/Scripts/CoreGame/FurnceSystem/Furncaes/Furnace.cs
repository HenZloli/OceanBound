using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class FurnaceCore : MonoBehaviour
{
    public FurnaceBlock furnaceBlock;
    [Header("UI")]
    public InventorySlot inputSlot;
    public InventorySlot fuelSlot;
    public InventorySlot outputSlot;

    public Image FieldResult;   // thanh progress nung
    public Image FuelBar;       // thanh nhiên liệu (giống Minecraft)

    public Sprite FurnaceOn;
    public Sprite FurnaceOff;

    public FurnaceRecipeDatabase recipeDatabase;

    private bool isBurning = false;
    private float burnTimer = 0f;    // thời gian nhiên liệu còn lại
    private float burnTimeMax = 0f;  // max thời gian nhiên liệu hiện tại
    private float progress = 0f;     // tiến độ nung hiện tại
    private FurnaceRecipe currentRecipe;

    void Start()
    {
        if (FieldResult != null) FieldResult.fillAmount = 0f;
        if (FuelBar != null) FuelBar.fillAmount = 0f;
    }

    void Update()
    {
        if (burnTimer > 0)
        {
            burnTimer -= Time.deltaTime;
            isBurning = true;

            // kiểm tra còn recipe & input hợp lệ không
            InventoryItem inputItem = inputSlot.GetComponentInChildren<InventoryItem>();
            if (currentRecipe != null && inputItem != null && inputItem.item == currentRecipe.input)
            {
                progress += Time.deltaTime / currentRecipe.smeltTime;
                FieldResult.fillAmount = progress;

                if (progress >= 1f) // nung xong
                {
                    CompleteSmelt();
                    progress = 0f;
                }
            }
            else
            {
                // không có input hợp lệ -> reset progress
                progress = 0f;
                FieldResult.fillAmount = 0f;
            }

            // update fuel bar
            if (FuelBar != null)
                FuelBar.fillAmount = burnTimer / burnTimeMax;
        }
        else
        {
            isBurning = false;
            if (FurnaceBlock.instance != null) //check ở đây vì khi obj chưa khởi tạo thì sẽ bị
            {
                FurnaceBlock.instance.FurnaceOff(); // đổi ngoại hình
            }
            

            // nếu hết nhiên liệu mà còn progress -> giảm dần
            if (progress > 0f)
            {
                progress -= Time.deltaTime * 0.5f; // tốc độ nguội (tùy chỉnh)
                if (progress < 0f) progress = 0f;
                FieldResult.fillAmount = progress;
            }

            TryStartSmelting();
        }

        if (isBurning)
        {
            FurnaceBlock.instance.FurnaceOn(); // khi nung đồ sẽ đổi ảnh
        }
    }


    void TryStartSmelting()
    {
        InventoryItem inputItem = inputSlot.GetComponentInChildren<InventoryItem>();
        InventoryItem fuelItem = fuelSlot.GetComponentInChildren<InventoryItem>();

        if (inputItem != null && fuelItem != null)
        {
            FurnaceRecipe recipe = recipeDatabase.GetRecipe(inputItem.item);
            if (recipe != null)
            {
                currentRecipe = recipe;

                // set fuel (mỗi item fuel có burnTime riêng)
                burnTimeMax = fuelItem.item.burnTime;
                burnTimer = burnTimeMax;
                
                // update fuel bar
                if (FuelBar != null)
                    FuelBar.fillAmount = 1f;

                // trừ fuel
                fuelItem.count--;
                fuelItem.RefreshCount();
                if (fuelItem.count <= 0) Destroy(fuelItem.gameObject);
            }
        }
    }

    void CompleteSmelt()
    {
        InventoryItem inputItem = inputSlot.GetComponentInChildren<InventoryItem>();
        if (inputItem == null)
        {
            progress = 0f;
            FieldResult.fillAmount = 0f;
            return;
        }

        // trừ input
        inputItem.count--;
        inputItem.RefreshCount();
        if (inputItem.count <= 0) Destroy(inputItem.gameObject);

        // spawn output
        if (outputSlot.transform.childCount == 0)
        {
            InventoryManager.instance.SpawnNewItem(currentRecipe.output, outputSlot);
        }
        else
        {
            InventoryItem outputItem = outputSlot.GetComponentInChildren<InventoryItem>();
            if (outputItem.item == currentRecipe.output && outputItem.count < InventoryManager.instance.MAX_STACK_ITEM)
            {
                outputItem.count++;
                outputItem.RefreshCount();
            }
        }

        // nếu sau khi nung mà input ko còn thì reset progress
        if (inputSlot.transform.childCount == 0)
        {
            progress = 0f;
            FieldResult.fillAmount = 0f;
        }
    }

}
