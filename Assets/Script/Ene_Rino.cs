using UnityEngine;


public class Ene_Rino : Enemy
{
    [Header("Move Info")]
    [SerializeField] private float speed;
    [SerializeField] private float speedAngry;
    [SerializeField] private float idleTime = 1;
    [SerializeField] private float idleTimeCounter;

    [SerializeField] private float shockTime;
    private float shockTimeCounter;


    [SerializeField] private LayerMask whatIsPlayer;
    private RaycastHit2D playerDetection;
    private bool angryMode;

    [Header("Visuals")]
    [SerializeField] private SpriteRenderer enemyRenderer;

    [SerializeField] private float waitTimeAfterDetection = 3f; // Thời gian đợi sau khi phát hiện người chơi
    [SerializeField] private float detectionTimeCounter; // Bộ đếm thời gian sau khi phát hiện


    protected override void Start()
    {
        base.Start();
        invincible = true;
        detectionTimeCounter = waitTimeAfterDetection; // Khởi tạo bộ đếm thời gian
        
    }


    void Update()
    {
        playerDetection = Physics2D.Raycast(wallCheck.position, Vector2.right * facingDir, 25f, whatIsPlayer);

        // Kiểm tra nếu người chơi đã được phát hiện và enemy chưa ở trong chế độ tức giận
        if (playerDetection && !angryMode)
        {
            // Nếu thời gian phát hiện còn lại lớn hơn 0, tiếp tục đếm ngược
            if (detectionTimeCounter > 0)
            {
                // Giảm thời gian phát hiện dựa trên thời gian thực
                detectionTimeCounter -= Time.deltaTime;

                
                rb.velocity = new Vector2(0, 0); // Dừng enemy lại

                // Thay đổi màu của enemy thành màu đỏ để biểu thị rằng nó đang trong trạng thái cảnh giác
                enemyRenderer.color = Color.red;
            }
            else // Ngược lại, nếu thời gian phát hiện đã hết
            {
                // Kích hoạt chế độ tức giận, cho phép enemy bắt đầu đuổi theo người chơi
                angryMode = true;

                // Đặt lại thời gian chờ sau khi phát hiện để sử dụng cho lần tiếp theo
                detectionTimeCounter = waitTimeAfterDetection;

                // Đặt lại màu của enemy về màu ban đầu
                enemyRenderer.color = Color.white;
            }
        }

        else if (!angryMode)
        {
            enemyRenderer.color = Color.white;
            if (idleTimeCounter <= 0)
                rb.velocity = new Vector2(speed * facingDir, rb.velocity.y);
            else
                rb.velocity = new Vector2(0, 0);

            idleTimeCounter -= Time.deltaTime;

            if (wallDetected || !groundDetected)
            {
                idleTimeCounter = idleTime;
                Flip();
            }
        }
        else //angrymode = true
        {
            rb.velocity = new Vector2(speedAngry * facingDir, rb.velocity.y);

            if (wallDetected && invincible)
            {
                invincible = false;
                transform.localScale = new Vector2(1, -1);
                shockTimeCounter = shockTime;
                Flip();

            }


            if (shockTimeCounter <= 0 && !invincible)
            {
                transform.localScale = new Vector2(1, 1);
                invincible = true;
                Flip();
                angryMode = false;
            }
            shockTimeCounter -= Time.deltaTime;
        }



        CollisionCheck();
        AnimatorController();

    }

    

    private void AnimatorController()
    {
        anim.SetBool("invincible", invincible);
        anim.SetFloat("xVelocity", rb.velocity.x);
    }

    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();

        Gizmos.DrawLine(wallCheck.position, new Vector2(wallCheck.position.x + playerDetection.distance * facingDir, wallCheck.position.y));
    }

}
