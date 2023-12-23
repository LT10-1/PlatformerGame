using Unity.VisualScripting;
using UnityEngine;


public class Ene_Rino : Enemy
{


    [Header("Rino Specific")]
    [SerializeField] private float speedAngry;
    [SerializeField] private float shockTime;
    [SerializeField] private float shockTimeCounter;


    
    
    
    private bool hitWall;

    [Header("Visuals")]
    [SerializeField] private SpriteRenderer enemyRenderer;

    [SerializeField] private float waitTimeAfterDetection = 1.5f; // Thời gian đợi sau khi phát hiện người chơi
    [SerializeField] private float detectionTimeCounter; // Bộ đếm thời gian sau khi phát hiện



    protected override void Start()
    {
        base.Start();
        invincible = false;
        detectionTimeCounter = waitTimeAfterDetection; // Khởi tạo bộ đếm thời gian

    }


    void Update()
    {
        if (isDead) return;
        CollisionCheck();
        


        if(canMove == true)
            invincible = false;
        if (angryMode == true)
            invincible = true;


        if (!playerDetection)
        {
            detectionTimeCounter = waitTimeAfterDetection;

        }

        // Kiểm tra nếu người chơi đã được phát hiện và enemy chưa ở trong chế độ tức giận
        if (playerDetection && !angryMode)
        {

            detectionTimeCounter -= Time.deltaTime; // Giảm thời gian phát hiện dựa trên thời gian thực


            rb.velocity = new Vector2(0, 0); // Dừng enemy lại



            enemyRenderer.color = Color.red; // Thay đổi màu của enemy thành màu đỏ để biểu thị rằng nó đang trong trạng thái cảnh giác

            if (detectionTimeCounter <= 0) // nếu thời gian phát hiện đã hết
            {

                angryMode = true; // Kích hoạt chế độ tức giận, cho phép enemy bắt đầu đuổi theo người chơi


                detectionTimeCounter = waitTimeAfterDetection; // Đặt lại thời gian chờ sau khi phát hiện để sử dụng cho lần tiếp theo


                enemyRenderer.color = Color.white;
            }
        }


        else if (!angryMode)
        {
            
            enemyRenderer.color = Color.white;
            WalkAround();
        }
        else //angrymode = true
        {
            enemyRenderer.color = Color.red;
            rb.velocity = new Vector2(speedAngry * facingDir, rb.velocity.y);

            if (!groundDetected)
            {
                Flip();
                angryMode = false;
            }




            if (wallDetected && invincible)
            {
                hitWall = true;
                invincible = false;
                canMove = false;
                enemyRenderer.color = Color.white;
                shockTimeCounter = shockTime;
                Flip();

            }
            if (hitWall)
            {
                enemyRenderer.color = Color.white;
                invincible = false;
                rb.velocity = new Vector2(0, 0);
            }


            if (shockTimeCounter <= 0 && !invincible)
            {
                hitWall = false;
                canMove = true;
                
                invincible = false;
                Flip();
                
                angryMode = false;
            }
        }

        shockTimeCounter -= Time.deltaTime;



        AnimatorController();

    }



    private void AnimatorController()
    {
        anim.SetBool("hitWall", hitWall);
        anim.SetFloat("xVelocity", rb.velocity.x);
    }

    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();

        
    }

}
