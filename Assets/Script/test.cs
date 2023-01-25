using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test : MonoBehaviour
{
    private Rigidbody2D rb;
    private BoxCollider2D coll;


    [Header("狀態")]
   // public bool isCrouch;
    public bool isOnGround;
   // public bool isJump;
    public bool isHeadBlacked;
    public bool isHanging;

    //float xVelocity;


    [Header("環境檢測")]
    public float footOffset = 0.4f;
    public float headClearance = 0.5f;
    public float groundDistance = 0.2f;
    float playerHight;
    public float eyeHeight = 1.5f;
    public float grabDistance = 0.4f;
    public float reachOffset = 0.7f;

    [Header("跳躍參數")]
    public float hangingJumpForce = 15;

    public LayerMask groundLayer;

    bool jumpPressed; 
    bool crouchPressed;


    private void Start()
    {
        coll = GetComponent<BoxCollider2D>();
        rb = GetComponent<Rigidbody2D>();
        playerHight = coll.size.y;

    }
    void Update()
    {
        PhysicsCheck();
        crouchPressed = Input.GetButtonDown("Crouch");
        GroundMovment();
        MidAirMovement();

    }
    void PhysicsCheck()
    {
        RaycastHit2D leftCheck = Raycast(new Vector2(-footOffset, 0f), Vector2.down, groundDistance, groundLayer);
        RaycastHit2D RightCheck = Raycast(new Vector2(footOffset, 0f), Vector2.down, groundDistance, groundLayer);

        if (leftCheck || RightCheck)
            isOnGround = true;
        else isOnGround = false;

        RaycastHit2D headCheck = Raycast(new Vector2(0f, coll.size.y), Vector2.up, headClearance, groundLayer);

        if (headCheck)
            isHeadBlacked = true;
        else isHeadBlacked = false;

        float direction = transform.localScale.x;
        Vector2 graDir = new Vector2(direction, 0f);

        RaycastHit2D blockedCheck = Raycast(new Vector2(footOffset * direction, playerHight), graDir, groundDistance, groundLayer);
        RaycastHit2D wallCheck = Raycast(new Vector2(footOffset * direction, eyeHeight), graDir, grabDistance, groundLayer);
        RaycastHit2D lefdgeCheck = Raycast(new Vector2(reachOffset * direction, playerHight), Vector2.down, grabDistance, groundLayer);

        if (!isOnGround && rb.velocity.y < 0f && lefdgeCheck && wallCheck && !blockedCheck)
        {
            Vector3 pos = transform.position;

            pos.x += (wallCheck.distance - 0.05f) * direction;

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
    }

        RaycastHit2D Raycast(Vector2 offset, Vector2 rayDiraction, float length, LayerMask layer)
        {
            Vector2 POS = transform.position;

            RaycastHit2D hit = Physics2D.Raycast(POS + offset, rayDiraction, length, layer);

            Color color = hit ? Color.red : Color.green;

            Debug.DrawRay(POS + offset, rayDiraction * length, color);

            return hit;
        }    
}