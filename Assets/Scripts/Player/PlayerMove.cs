using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    private Rigidbody2D RB;
    private SpriteRenderer SR;
    private Animator anim;
    private PlayerState playerState;

    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 3f;      // tốc độ đi bộ
    [SerializeField] private float runBoost = 2f;       // tốc độ cộng thêm khi chạy
    [SerializeField] private float acceleration = 10f;  // gia tốc khi tăng tốc
    [SerializeField] private float deceleration = 10f;  // giảm tốc khi dừng
    [SerializeField] private bool useMomentum = false;  // bật/tắt quán tính
    [SerializeField] private float momentumFactor = 0.9f; // độ trượt (0.95 = trượt nhiều)

    [SerializeField] private Sprite[] img;

    private Vector2 targetVelocity;
    private Vector2 currentVelocity;

    public bool canMove = true;

    void Start()
    {
        RB = GetComponent<Rigidbody2D>();
        SR = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        playerState = GetComponent<PlayerState>();
    }

    void Update()
    {
        HandleMovement();
        HandleAnimation();
    }

    void HandleMovement()
    {
        Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        input.Normalize();

        float speed = moveSpeed;
        bool isRuning = Input.GetKey(KeyCode.LeftShift) && playerState.CurrentStamina > 0;

        if (isRuning)
        {
            playerState.StaminaUse(2f);
            speed += runBoost;
        }
        else
        {
            speed = moveSpeed;
        }
        

        if (!canMove)
            speed = 0;

        targetVelocity = input * speed;

        if (useMomentum)
        {
            // nếu có input thì tăng tốc về target
            if (input.magnitude > 0)
            {
                currentVelocity = Vector2.Lerp(currentVelocity, targetVelocity, acceleration * Time.deltaTime);
            }
            else
            {
                // không có input thì trượt từ từ
                currentVelocity *= momentumFactor;
                if (currentVelocity.magnitude < 0.05f)
                    currentVelocity = Vector2.zero;
            }
        }
        else
        {
            // không dùng quán tính thì tăng/giảm tốc bình thường
            currentVelocity = Vector2.Lerp(currentVelocity, targetVelocity, (targetVelocity.magnitude > 0 ? acceleration : deceleration) * Time.deltaTime);
        }

        RB.linearVelocity = currentVelocity;

        if (input.x < 0) SR.flipX = true;
        else if (input.x > 0) SR.flipX = false;
    }

    void HandleAnimation()
    {
        if (anim == null) return;

        if (currentVelocity.magnitude > 0.1f)
        {
            if (Input.GetKey(KeyCode.LeftShift))
                anim.Play("player_1_run");
            else
                anim.Play("player_1_walk");
        }
        else
        {
            anim.Play("player_1_idle");
        }
    }
}
