using UnityEngine;

public class BuildingSystem : MonoBehaviour
{
    [Header("Layers & Prefab")]
    public LayerMask groundLayer;
    public LayerMask blockLayer;
    public GameObject highlightPrefab;

    [Header("Grid & Player")]
    public Transform player;
    public float maxPlaceDistance = 5f;

    [Header("Time Dealay")]
    public float actionDelay = 0.2f;
    private float nextActionTime;

    [Header("Item Change Delay")]
    public GameObject[] itemChange;

    private GameObject currentHighlight;

    void Update()
    {
        Item item = InventoryManager.instance.GetSelectedItem(false);

        if (item != null)
        {
            Highlight(item);

            if (Input.GetMouseButtonDown(1))
                Build(item);


            if (Input.GetMouseButtonDown(0))
                DestroyBlock(item);


            ///case game

            if (Input.GetMouseButtonDown(0) && item.ItemID == "flint_flake_item")
                // dùng flint_flake_item để có cơ hội nhận flint từ rock
                ItemChange(item);

            if (Input.GetMouseButtonDown(0) && item.ItemID == "wood_log")
                // đặt wood_log lên chopping_block
                ItemChange(item);
            
            if (Input.GetMouseButtonDown(0) && item.ItemID == "flint_hatchet")
                // dùng flint_hatchet chặt gỗ trong chopping_block
                ItemChange(item);
            
                
        }
        else
        {
            if (currentHighlight != null)
                currentHighlight.SetActive(false);
        }
    }

    private Vector2 SnapToGrid(Vector2 pos, float gridSize)
    {
        float x = Mathf.Round(pos.x / gridSize) * gridSize;
        float y = Mathf.Round(pos.y / gridSize) * gridSize;
        return new Vector2(x, y);
    }

    private void Highlight(Item currentItem)
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        if (currentItem.type == ItemType.Block)
        {
            mousePos = SnapToGrid(mousePos, 1f);
        } else
        {
            mousePos = SnapToGrid(mousePos, 0.1f);
        }



        Collider2D groundHit = Physics2D.OverlapPoint(mousePos, groundLayer);
        Collider2D blockHit = Physics2D.OverlapPoint(mousePos, blockLayer);
        float distance = Vector2.Distance(player.position, mousePos);

        if (groundHit == null || distance > maxPlaceDistance)
        {
            if (currentHighlight != null) currentHighlight.SetActive(false);
            return;
        }

        if (currentHighlight == null)
        {
            currentHighlight = Instantiate(highlightPrefab, mousePos, Quaternion.identity);
        }
        else
        {
            currentHighlight.SetActive(true);
            currentHighlight.transform.position = mousePos;
        }

        SpriteRenderer sr = currentHighlight.GetComponent<SpriteRenderer>();
        if (blockHit != null || distance > maxPlaceDistance)
            sr.color = Color.red;
        else
            sr.color = Color.green;
    }

    private void Build(Item currentItem)
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos = SnapToGrid(mousePos, 1f);

        float distance = Vector2.Distance(player.position, mousePos);
        if (distance > maxPlaceDistance) return; // quá xa

        Collider2D groundHit = Physics2D.OverlapPoint(mousePos, groundLayer);
        Collider2D blockHit = Physics2D.OverlapPoint(mousePos, blockLayer);

        if (groundHit == null || blockHit != null) return;

        if (currentItem.type == ItemType.Block)
        {
            InventoryManager.instance.GetSelectedItem(true); // giảm số lượng
            GameObject blockPrefab = currentItem.block_prefab;
            Instantiate(blockPrefab, mousePos, Quaternion.identity);
        }
    }

    private void DestroyBlock(Item currentItem)
    {
        if (currentItem == null) return;

        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos = SnapToGrid(mousePos, 0.1f);

        float distance = Vector2.Distance(player.position, mousePos);
        if (distance > maxPlaceDistance) return;

        Collider2D blockHit = Physics2D.OverlapPoint(mousePos, blockLayer);

        if (blockHit != null && currentItem.type == ItemType.Tools)
        {
            blockBase block = blockHit.GetComponent<blockBase>();
            if (block != null)
            {

                if (block.requiredTool == currentItem.actionType /*&& currentItem.MiningLevel >= block.LevelBlock*/ && Time.time >= nextActionTime)
                {
                    nextActionTime = Time.time + actionDelay;
                    PlayerUseController puc = player.GetComponent<PlayerUseController>();
                    puc.TryUseTool(block);
                }
                else if(Time.time < nextActionTime)
                {
                    Debug.Log("Hành động quá nhanh, hãy chậm lại");
                }
                else
                {
                    TipGameInfo.instance.TipMessage("Cần công cụ phù hợp để phá block này");
                }

            }
        }
    }
    private void ItemChange(Item currentItem)
    {
        if (currentItem == null) return;

        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos = SnapToGrid(mousePos, 0.1f);

        float distance = Vector2.Distance(player.position, mousePos);
        if (distance > maxPlaceDistance) return; // quá xa

        Collider2D blockHit = Physics2D.OverlapPoint(mousePos, blockLayer);
        if (blockHit == null) return;

        blockBase block = blockHit.GetComponent<blockBase>();
        if (block == null) return;




        /// case Flint
        float chance = Random.Range(0, 100);

        if (blockHit != null && currentItem.ItemID == "flint_flake_item" && itemChange.Length > 0 && block.BlockID== "rock_block")
        {
            InventoryManager.instance.GetSelectedItem(true);
            if (chance > 50)
                DropItemChange(0);
        }




        /// case Chopping
        Chopping chopping = blockHit.GetComponent<Chopping>();
        if (block.BlockID == "chopping_block" && chopping != null)
        {
            // đặt gỗ vào chopping 
            if (currentItem.ItemID == "wood_log")
            {
                if (!chopping.isWoodLogInChopping) // chưa có gỗ
                {
                    InventoryManager.instance.GetSelectedItem(true);
                    chopping.isWoodLogInChopping = true;

                    if (chopping.ItemSprite != null)
                        chopping.ItemSprite.sprite = currentItem.image;

                    Debug.Log("Đã đặt WoodLog lên Chopping");
                }
                else
                {
                    Debug.Log("Chopping đã có WoodLog, không thể đặt thêm");
                }
                return;
            }
            // chặt gỗ trong chopping
            if (currentItem.ItemID == "flint_hatchet" && chopping.isWoodLogInChopping)
            {
                for (int i = 0; i < 4; i++)
                    DropItemChange(1);

                chopping.isWoodLogInChopping = false;
                if (chopping.ItemSprite != null)
                    chopping.ItemSprite.sprite = null;
                Debug.Log("Chặt gỗ trong chopping -> ra 4 khúc");
                return;
            }
        }
    }

    void DropItemChange(int index)
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos = SnapToGrid(mousePos, 0.1f);
        Instantiate(itemChange[index], mousePos, Quaternion.identity);
    }


    void OnDrawGizmos()
    {
        if (player != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(player.position, maxPlaceDistance);
        }
    }
}
