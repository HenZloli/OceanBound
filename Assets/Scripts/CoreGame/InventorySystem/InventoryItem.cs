using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class InventoryItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerClickHandler
{
    [Header("Item Info")]
    public Item item;
    public int count = 1;

    [Header("UI")]
    public Image image;
    public TextMeshProUGUI countText;

    [HideInInspector] public Transform parentAfterDrag;
    private Canvas parentCanvas;

    

    void Awake()
    {
        image = GetComponent<Image>();
        countText = GetComponentInChildren<TextMeshProUGUI>();
        parentCanvas = GetComponentInParent<Canvas>();
    }

    public void InitialiseItem(Item newItem, int newCount = 1)
    {
        item = newItem;
        count = newCount;
        image.sprite = item.image;
        image.enabled = true;
        RefreshCount();
    }

    public void RefreshCount()
    {
        countText.text = count > 1 ? count.ToString() : "";
    }

    public void SetCount(int newCount)
    {
        count = Mathf.Max(0, newCount);
        RefreshCount();
        if (count <= 0)
        {
            Destroy(gameObject);
        }
    }

    public void AddCount(int amount)
    {
        count += amount;
        RefreshCount();
    }

    // --- DRAG ---
    public void OnBeginDrag(PointerEventData eventData)
    {
        image.raycastTarget = false;
        parentAfterDrag = transform.parent;
        transform.SetParent(parentCanvas.transform, true);
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        image.raycastTarget = true;

        GameObject hitObj = eventData.pointerCurrentRaycast.gameObject;
        InventorySlot targetSlot = hitObj ? hitObj.GetComponentInParent<InventorySlot>() : null;

        if (targetSlot != null)
        {
            InventoryItem targetItem = targetSlot.GetComponentInChildren<InventoryItem>();

            // Nếu slot trống -> chuyển item
            if (targetItem == null)
            {
                transform.SetParent(targetSlot.transform);
                parentAfterDrag = targetSlot.transform;
                transform.localPosition = Vector3.zero;
            }
            // Nếu trùng loại & stackable -> gộp stack
            else if (targetItem.item == this.item && item.Stackable)
            {
                int maxStack = InventoryManager.instance.MAX_STACK_ITEM;
                int spaceLeft = maxStack - targetItem.count;

                if (spaceLeft > 0)
                {
                    int toMove = Mathf.Min(spaceLeft, this.count);
                    targetItem.AddCount(toMove);
                    SetCount(this.count - toMove);
                }
                else
                {
                    SwapWith(targetItem);
                }
            }
            else // khác loại → đổi chỗ
            {
                SwapWith(targetItem);
            }
        }
        else
        {
            transform.SetParent(parentAfterDrag);
            transform.localPosition = Vector3.zero;
        }
    }

    // --- RIGHT CLICK TO SPLIT STACK ---
    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right && item.Stackable && count > 1)
        {
            SplitStack();
        }
    }

    private void SplitStack()
    {
        // tìm slot trống
        InventorySlot emptySlot = InventoryManager.instance.GetEmptySlot();
        if (emptySlot == null) return;

        int half = count / 2;
        SetCount(count - half);

        // spawn item mới trong slot trống
        GameObject newItemObj = Instantiate(gameObject, emptySlot.transform);
        InventoryItem newItem = newItemObj.GetComponent<InventoryItem>();
        newItem.InitialiseItem(item, half);
        newItem.parentAfterDrag = emptySlot.transform;
        newItem.transform.localPosition = Vector3.zero;
    }

    private void SwapWith(InventoryItem other)
    {
        Transform otherParent = other.transform.parent;

        other.transform.SetParent(parentAfterDrag);
        other.transform.localPosition = Vector3.zero;

        transform.SetParent(otherParent);
        transform.localPosition = Vector3.zero;

        parentAfterDrag = otherParent;
    }
}
