using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    Animator animator;
    PlayerController movement;
    Rigidbody2D rb;

    int groundID;
    int hangingID;
    int crouchID;
    int speedID;
    int fallID;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        movement = GetComponentInParent<PlayerController>();
        rb = GetComponentInParent<Rigidbody2D>();
    }

    void Start()
    {     
        groundID = Animator.StringToHash("isOnGround");
        hangingID = Animator.StringToHash("isHanging");
        crouchID = Animator.StringToHash("isCrouching");
        speedID = Animator.StringToHash("speed");
        fallID = Animator.StringToHash("verticalVelocity");
    }

    void Update()
    {
        animator.SetFloat(speedID, Mathf.Abs(movement.xVelocity));
        animator.SetBool(groundID, movement.isOnGround);
        animator.SetBool(hangingID, movement.isHanging);
        animator.SetBool(crouchID, movement.isCrouch);
        animator.SetFloat(fallID, rb.velocity.y);
    }

    public void StepAudio()
    {
        AudioManager.PlayFootstrpAudio();
    }

    public void CrouchStepAudio()
    {
        AudioManager.PlayCrouchFootstrpAudio();
    }
}
