using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class ChestItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public Item item;

    [Header("UI")]
    [HideInInspector] public Image image;
    public TextMeshProUGUI countText;

    [HideInInspector] public int count = 1;
    [HideInInspector] public Transform parentAfterDrag;

    void Awake()
    {
        image = GetComponent<Image>();
        countText = GetComponentInChildren<TextMeshProUGUI>();
    }

    public void InitialiseItem(Item newItem)
    {
        item = newItem;
        if (newItem.image != null)
        {
            image.sprite = newItem.image;
            image.enabled = true;
            RefreshCount();
        }
        else
        {
            Debug.LogWarning(newItem.name + " chưa có sprite!");
        }
    }

    public void RefreshCount()
    {
        countText.text = count.ToString();
        countText.gameObject.SetActive(count > 1);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        image.raycastTarget = false;
        parentAfterDrag = transform.parent;
        transform.SetParent(transform.root, true);
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        image.raycastTarget = true;
        transform.SetParent(parentAfterDrag);
        transform.localPosition = Vector3.zero;
    }
}
