using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private  Rigidbody2D rb;
    private BoxCollider2D coll;

    [Header("移動參數")]
    public float speed = 8f;
    public float CrouchSpeedDivsor = 3f;

    [Header("跳躍參數")]
    public float jumpForce = 6.3f;
    public float jumpHoldForce = 1.9f;
    public float jumpHoldDuration = 0.1f;
    public float crouchJumpBoost = 2.5f;
    public float hangingJumpForce = 15;

    float jumptime;

    [Header("狀態")]
    public bool isCrouch;
    public bool isOnGround;
    public bool isJump;
    public bool isHeadBlacked;
    public bool isHanging;

    public float xVelocity;

    //不會 ok
    [Header("環境檢測")]
    public float footOffset = 0.4f;
    public float headClearance = 0.5f;
    public float groundDistance = 0.2f;
    float playerHight;
    public float eyeHeight = 1.5f;
    public float grabDistance = 0.4f;
    public float reachOffset = 0.7f;

    public LayerMask groundLayer;

    //按鍵設置
    bool jumpPressed;
    bool jumpHeld;
    bool crouchHeld;
    bool crouchPressed;

    //碰撞體尺寸
    Vector2 colliderStandSize;
    Vector2 colliderStandOffset;
    Vector2 colliderCrouchSize;
    Vector2 colliderCrouchOffset;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        coll = GetComponent<BoxCollider2D>();

        playerHight = coll.size.y;

        colliderStandSize = coll.size;
        colliderStandOffset = coll.offset;
        colliderCrouchSize = new Vector2(coll.size.x, coll.size.y / 2f);
        colliderCrouchOffset = new Vector2(coll.offset.x, coll.offset.y / 2f);
    }
    private void Update()
    {
        if (GameManager.GameOver())
            return;

        jumpPressed = Input.GetButton("Jump");
        jumpHeld = Input.GetButton("Jump");
        crouchHeld = Input.GetButton("Crouch");
        crouchPressed = Input.GetButtonDown("Crouch");  
    }
    private void FixedUpdate()
    {
        if (GameManager.GameOver())
            return;

        PhysicsCheck();
        GroundMovment();
        MidAirMovement();

        FilpDirction();
    }

    //不會 
    void PhysicsCheck()
    {       
        //左右腳射線
        RaycastHit2D leftCheck = Raycast(new Vector2(-footOffset, 0f), Vector2.down, groundDistance,groundLayer);
        RaycastHit2D RightCheck = Raycast(new Vector2(footOffset, 0f), Vector2.down, groundDistance, groundLayer);

        if (leftCheck||RightCheck)
             isOnGround = true;
        else isOnGround = false;

        //頭頂射線
        RaycastHit2D headCheck = Raycast(new Vector2( 0f,coll.size.y), Vector2.up, headClearance, groundLayer);

        if (headCheck)
            isHeadBlacked = true;
        else isHeadBlacked = false;

        //懸掛
        float direction = transform.localScale.x;
        Vector2 graDir = new Vector2(direction, 0f);

        RaycastHit2D blockedCheck = Raycast(new Vector2(footOffset * direction, playerHight), graDir, groundDistance, groundLayer);
        RaycastHit2D wallCheck = Raycast(new Vector2(footOffset * direction, eyeHeight), graDir, grabDistance, groundLayer);
        RaycastHit2D lefdgeCheck = Raycast(new Vector2(reachOffset * direction, playerHight), Vector2.down, grabDistance, groundLayer);

        if (!isOnGround && rb.velocity.y < 0f && lefdgeCheck && wallCheck && !blockedCheck)
        {
            Vector3 pos = transform.position;

            pos.x += (wallCheck.distance-0.05f) * direction;

            pos.y -= lefdgeCheck.distance;

            transform.position = pos;

            rb.bodyType = RigidbodyType2D.Static;
            isHanging = true;
        }
    }

    void GroundMovment()
    {
        if (isHanging)
            return;

        if (crouchHeld && !isCrouch && isOnGround)
            CrouchUp();
        else if (!crouchHeld && isCrouch && !isHeadBlacked)
            StandUp();
        else if (!isOnGround && isCrouch)
            StandUp();

        xVelocity = Input.GetAxis("Horizontal");

        //不會 ok
        if (isCrouch)
            xVelocity /= CrouchSpeedDivsor;

        rb.velocity = new Vector2(xVelocity * speed, rb.velocity.y);
    }
    
    //3月27 不太懂  跳
    void MidAirMovement()
    {
        if (isHanging)
        {
            if (jumpPressed)
            {
                rb.bodyType = RigidbodyType2D.Dynamic;
                rb.velocity = new Vector2(rb.velocity.x, hangingJumpForce);
                isHanging = false;
            }
            if (crouchPressed)
            {
                rb.bodyType = RigidbodyType2D.Dynamic;

                isHanging = false;
            }
        }

        if (jumpPressed && isOnGround && !isJump&& !isHeadBlacked)
        {
            if (isCrouch )
            {
                StandUp();
                rb.AddForce(new Vector2(0f, crouchJumpBoost), ForceMode2D.Impulse);
            }
            isOnGround = false;
            isJump = true;

            jumptime = Time.time + jumpHoldDuration;

            rb.AddForce(new Vector2(0f, jumptime), ForceMode2D.Impulse);
            //rb.velocity = new Vector2(rb.velocity.x, jumpForce);

            AudioManager.PlayerJumpAudio();
        }

        else if (isJump)
        {
            if(jumpHeld)
                rb.AddForce(new Vector2(0f, jumpHoldForce), ForceMode2D.Impulse);

                //rb.velocity = new Vector2(rb.velocity.x, jumopHoldForce);
            if (jumptime < Time.time)
                isJump = false;
        }

    }

    void FilpDirction() {
        if (xVelocity < 0)
        {
            transform.localScale = new Vector3(-1, 1,1);
        }
        if (xVelocity > 0)
        {
            transform.localScale = new Vector3(1, 1,1);
        }
    }
     
    void CrouchUp()
    {
        isCrouch = true;
        coll.size = colliderCrouchSize;
        coll.offset = colliderCrouchOffset;
    }
     
    void StandUp()
    {
        isCrouch = false;
        coll.size = colliderStandSize;
        coll.offset = colliderStandOffset;
    }

    RaycastHit2D Raycast(Vector2 offset,Vector2 rayDiraction, float length,LayerMask layer)
    {
        Vector2 POS = transform.position;

        RaycastHit2D hit = Physics2D.Raycast(POS + offset, rayDiraction, length, layer);

        Color color = hit ? Color.red : Color.green;

        Debug.DrawRay(POS + offset, rayDiraction * length, color);

        return hit;
    }
}
