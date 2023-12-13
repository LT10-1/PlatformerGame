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
    private float detectionTimeCounter; // Bộ đếm thời gian sau khi phát hiện

    protected override void Start()
    {
        base.Start();
        invincible = true;
        detectionTimeCounter = waitTimeAfterDetection; // Khởi tạo bộ đếm thời gian
        
    }


    void Update()
    {
        playerDetection = Physics2D.Raycast(wallCheck.position, Vector2.right * facingDir, 25f, whatIsPlayer);

        if (playerDetection && !angryMode)
        {
            // Bắt đầu đếm ngược thời gian đợi khi phát hiện người chơi
            if (detectionTimeCounter > 0)
            {
                detectionTimeCounter -= Time.deltaTime;
                Holding(); // Gọi phương thức để enemy dừng lại
                enemyRenderer.color = Color.red; // Đặt màu của enemy thành màu đỏ
            }
            else
            {
                angryMode = true; // Chuyển sang angryMode sau khi thời gian đợi kết thúc
                detectionTimeCounter = waitTimeAfterDetection; // Đặt lại bộ đếm thời gian cho lần phát hiện tiếp theo
                enemyRenderer.color = Color.white; // Đặt lại màu của enemy thành màu gốc khi bắt đầu đuổi theo
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

    private void Holding()
    {
        rb.velocity = new Vector2(0, 0); // Dừng enemy lại
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
