﻿using Unity.VisualScripting;
using UnityEngine;


public class Ene_Rino : Enemy
{


    [Header("Rino Specific")]
    [SerializeField] private float speedAngry;
    [SerializeField] private float shockTime;
    [SerializeField] private float shockTimeCounter;


    [SerializeField] private LayerMask whatIsPlayer;
    private RaycastHit2D playerDetection;
    private bool angryMode;
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

        CollisionCheck();
        playerDetection = Physics2D.Raycast(wallCheck.position, Vector2.right * facingDir, 25f, whatIsPlayer);

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


                enemyRenderer.color = Color.white;  // Đặt lại màu của enemy về màu ban đầu
            }
        }


        else if (!angryMode)
        {
            
            enemyRenderer.color = Color.white;
            WalkAround();
        }
        else //angrymode = true
        {

            if (!groundDetected)
            {
                Flip();
                angryMode = false;
            }

            rb.velocity = new Vector2(speedAngry * facingDir, rb.velocity.y);



            if (wallDetected && invincible)
            {
                hitWall = true;
                invincible = false;
                canMove = false;
                
                shockTimeCounter = shockTime;
                Flip();

            }
            if (hitWall)
            {
                rb.velocity = new Vector2(0, 0);
            }


            if (shockTimeCounter <= 0 && !invincible)
            {
                hitWall = false;
                canMove = true;
                
                invincible = true;
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

        Gizmos.DrawLine(wallCheck.position, new Vector2(wallCheck.position.x + playerDetection.distance * facingDir, wallCheck.position.y));
    }

}
