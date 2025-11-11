using UnityEngine;

public class CallUI : MonoBehaviour
{
    private PlayerMove playerMove;
    public static CallUI instance;

    [Header("UI")]
    public GameObject[] UI;

    [Header("UI Cannot be turned off")]
    public GameObject[] UInotFalse;

    [Header("Pos UI")]
    public Transform[] PosUI;

    [Header("Recipe ToolTip Show")]
    public GameObject RecieToolTipShow;

    //=========Vector============//
    private Vector2[] originalPos;
    private Vector2[] HiddenPos;

    private bool isOpen = false;

    /* UI Cannot be turned off
        0 = Inventory : y = 3000
        1 = Furnace_1 : y = 4000
    */

    /* normal UI
        0 = miniCarting
    */

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        if (playerMove == null)
            playerMove = FindAnyObjectByType<PlayerMove>();

        originalPos = new Vector2[UInotFalse.Length];
        HiddenPos = new Vector2[UInotFalse.Length];

        // lưu vị trí gốc duy nhất một lần
        for (int i = 0; i < UInotFalse.Length; i++)
        {
            originalPos[i] = PosUI[i].position;
        }

        // ẩn lúc khởi động
        SetHidden(0, 3000);
        SetHidden(1, 4000);
    }





    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
            InventoryUI();
    }




    public void InventoryUI()
    {
        if (isOpen)
        {
            // ẩn 
            SetHidden(1, 4000); // ẩn lò khi ấn mở inven
            SetHidden(0, 3000);

            RecieToolTipShow.SetActive(false);
            UI[0].SetActive(false);
            UI[2].SetActive(false);
            playerMove.canMove = true;
        }
        else
        {
            // hiện
            SetVisible(0);
            UI[0].SetActive(true);
            playerMove.canMove = false;
        }
        isOpen = !isOpen;
    }

    public void CratingTaleUI()
    {
        if (isOpen)
        {
            // ẩn 
            SetHidden(0, 3000);
            RecieToolTipShow.SetActive(false);
            UI[1].SetActive(false);
            playerMove.canMove = true;
        }
        else
        {
            // hiện
            SetVisible(0);
            UI[1].SetActive(true);
            playerMove.canMove = false;
        }
        isOpen = !isOpen;
    }
    public void AnvilUI()
    {
        if (isOpen)
        {
            // ẩn 
            SetHidden(0, 3000);
            UI[2].SetActive(false);
            playerMove.canMove = true;
        }
        else
        {
            // hiện
            SetVisible(0);
            UI[2].SetActive(true);
            playerMove.canMove = false;
        }
        isOpen = !isOpen;
    }



    /// <summary>
    ///  cannot be turnd off
    /// </summary>
    public void Furnace_1()
    {
        if (isOpen)
        {
            // ẩn 
            SetHidden(1, 4000);
            SetHidden(0, 3000);
            playerMove.canMove = true;
        }
        else
        {
            // hiện
            SetVisible(1);
            SetVisible(0);
            playerMove.canMove = false;
        }
        isOpen = !isOpen;
    }




    //========UI Cannot be turned off========//

    public void SetHidden(int i, float y)
    {
        HiddenPos[i] = new Vector2(originalPos[i].x, y);
        UInotFalse[i].transform.position = HiddenPos[i];
    }

    public void SetVisible(int i)
    {
        UInotFalse[i].transform.position = originalPos[i];
    }

    
}