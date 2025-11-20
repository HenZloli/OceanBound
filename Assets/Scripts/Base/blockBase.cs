using UnityEngine;
using Unity.Cinemachine;

public abstract class blockBase : MonoBehaviour
{
    private CinemachineImpulseSource impulse;
    [Header("Block Settings")]

    public string BlockID;
    public string blockName = "";
    public int maxHardness;
    public GameObject dropItem;
    public int dropAmount = 1;
    public ActionType requiredTool;

    public int LevelBlock;

    protected int currentHardness;
    private SpriteRenderer sr;
    private Color originalColor;

    protected virtual void Awake()
    {
        currentHardness = maxHardness;
        sr = GetComponent<SpriteRenderer>();
        impulse = GetComponent<CinemachineImpulseSource>();
        if (sr != null)
            originalColor = sr.color;
    }

    public virtual void TakeDamage(int damage, Item tool)
    {
        if (tool == null || tool.actionType != requiredTool)
        {
            return;
        }

        currentHardness -= damage;

        Shake(0.06f, 0.04f);

        Debug.Log(blockName + " còn " + currentHardness + " HP");

        if (sr != null)
            StartCoroutine(HitFlash());

        if (currentHardness <= 0)
        {
            Shake(0.15f, 0.13f);
            BreakBlock(tool);
        }
    }

    private System.Collections.IEnumerator HitFlash()
    {
        sr.color = Color.red;
        yield return new WaitForSeconds(0.1f); // thời gian nhấp nháy
        sr.color = originalColor;
    }

    protected virtual void BreakBlock(Item tool)
    {
        Debug.Log(blockName + " đã bị phá!");

        if (dropItem != null && tool.MiningLevel >= LevelBlock)
        {
            for (int i = 0; i < dropAmount; i++)
            {
                Instantiate(dropItem, transform.position, Quaternion.identity);
            }
        }
        else
        {
            // Debug.Log("Cấp độ của tool không đủ để nhận phần thưởng từ block này");
            TipGameInfo.instance.TipMessage("Cấp Độ Tool quá thấp để lấy được tài nguyên");
        }

        Destroy(gameObject);
    }
    //DAX FIX 
    protected void Shake(float x, float y)
    {
        if (impulse != null)
            impulse.GenerateImpulse(new Vector3(x, y, 0));
    }
}
