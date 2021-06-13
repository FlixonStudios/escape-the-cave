using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    //Config
    [SerializeField] float playerBaseXMoveSpeed = 5.0f;
    [SerializeField] float playerBaseJumpSpeed = 50.0f;
    [SerializeField] float playerBaseClimbSpeed = 2.0f;
    [SerializeField] Vector2 deathKick = new Vector2(25f, 0f);
    [SerializeField] float delayBeforeOffGround = 0.2f;
    [SerializeField] float delayBeforeCanClimbAgain = 0.4f;

    //State
    bool isAlive = true;
    bool isOnGround = true;
    bool isClimbing = false;
    bool isJumping = false;
    
    // Cached component references
    Vector2 playerCurrentMoveSpeed = new Vector2(0, 0);
    Rigidbody2D myRigidbody2D;
    Animator myAnimator;
    CapsuleCollider2D myBody;
    BoxCollider2D myFeet;
    float myBaseRigidbodyGravity;
    float SUPER_SMALL_VALUE = 0.0001f;
    float offGroundToJumpDelayTimer = 0f;
    float jumpDelayTimer = 0f;
    

    //add transition from ladder
    //add transition from ground
    

    // Message then methods
    void Start()
    {
        myRigidbody2D = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();
        myBody = GetComponent<CapsuleCollider2D>();
        myFeet = GetComponent<BoxCollider2D>();
        myBaseRigidbodyGravity = myRigidbody2D.gravityScale;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isAlive) { return; }
        
        Run();       
        Jump();
        ClimbLadder();
        Idle();
        Die();
        StateCheckTimer();
        FlipSprite();
    }

    private void Run()
    {        
        if (InRunnableState())
        {
            float playerXMove = Input.GetAxis("Horizontal") * playerBaseXMoveSpeed;

            myRigidbody2D.velocity = new Vector2(playerXMove, myRigidbody2D.velocity.y);

            myAnimator.SetBool("isRunning", true);
        }
        
    }

    private bool InRunnableState()
    {
        return !isClimbing;
    }

    private void Idle()
    {
                
        bool playerHasHorizontalSpeed = Mathf.Abs(myRigidbody2D.velocity.x) > Mathf.Epsilon;

        if (!playerHasHorizontalSpeed)
        {
            myAnimator.SetBool("isRunning", false);
        }
    }

    
    private void ClimbLadder()
    {               
        if (InClimbableState())
        {

            float playerYMove = Input.GetAxis("Vertical") * playerBaseClimbSpeed;
            float playerXMove = Input.GetAxis("Horizontal") * playerBaseClimbSpeed;

            myRigidbody2D.velocity = new Vector2(playerXMove, playerYMove);

            ToggleClimbingOn();
            
            ToggleClimbingAnimationOn();
            
            ToggleGravityOff();
        }
        else
        {
            ToggleClimbingOff();
            ToggleClimbingAnimationOff();
            ToggleGravityOn();
        }
    }
    private bool InClimbableState()
    {
        //Don't need to check whether currently Climbing to return Climbable state
        //Player can climb regardless of whether he is currently climbing or jumping

        if (jumpDelayTimer < delayBeforeCanClimbAgain)
        {
            return false;
        }
        else if (myBody.IsTouchingLayers(LayerMask.GetMask("Ladder")))
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    private void ToggleClimbingAnimationOff()
    {
        myAnimator.SetBool("isClimbing", false);
    }
    private void ToggleGravityOn()
    {
        myRigidbody2D.gravityScale = myBaseRigidbodyGravity;
    }
    private void ToggleGravityOff()
    {
        myRigidbody2D.gravityScale = 0;
    }

    private void ToggleClimbingAnimationOn()
    {
        bool playerHasVerticalSpeed = Mathf.Abs(myRigidbody2D.velocity.y) > SUPER_SMALL_VALUE;
        bool playerHasHorizontalSpeed = Mathf.Abs(myRigidbody2D.velocity.x) > SUPER_SMALL_VALUE;

        bool playerHasClimbSpeed = (playerHasVerticalSpeed || playerHasHorizontalSpeed);

        if (playerHasClimbSpeed)
        {
            myAnimator.SetBool("isClimbing", playerHasClimbSpeed);
        }
        else
        {
            // TODO set climbing pause animation to play
        }        
    }

    private void ToggleClimbingOn()
    {
        isClimbing = true;
        ResetJumpDelayTimer();
    }
    private void ToggleClimbingOff()
    {
        isClimbing = false;
    }
    private void Jump()
    {
        if (InJumpableState())
        {
            if (Input.GetButtonDown("Jump"))
            {
                StartCountdownToNextJump();
                isJumping = true;
                Vector2 jumpVelocityToAdd = new Vector2(0, playerBaseJumpSpeed);
                myRigidbody2D.velocity += jumpVelocityToAdd;
            }
        }
    }
    private bool InJumpableState()
    {
        CheckIfOnGround();
        CheckIfClimbing();
        CheckIfJumping();
        if (isJumping)
        {
            return false;
        }
        else if (isOnGround || isClimbing)
        {            
            return true;
        }
        else
        {
            return false;
        }
    }

    private void CheckIfClimbing()
    {
        // Not necessary?
    }

    private void CheckIfOnGround()
    {
        if (myFeet.IsTouchingLayers(LayerMask.GetMask("Ground")))
        {
            isOnGround = true;
            ResetJumpDelayTimer();
        }
        else if (StillWithinBufferOffGroundToJump())
        {
            isOnGround = true;
        }
        else
        {
            isOnGround = false;
        }
    }
    private void CheckIfJumping()
    {
        if (jumpDelayTimer > delayBeforeCanClimbAgain)
        {
            isJumping = false;
        }
        else
        {
            isJumping = true;
        }
    }

    private void StartCountdownToNextJump()
    {
        jumpDelayTimer = 0f;
    }
    private bool StillWithinBufferOffGroundToJump()
    {     
        if (offGroundToJumpDelayTimer >= delayBeforeOffGround)
        {
            return false;
        }
        else
        {
            return true;
        }
    }
    private void ResetJumpDelayTimer()
    {
        offGroundToJumpDelayTimer = 0f;
    }
    private void Die()
    {
        if (myBody.IsTouchingLayers(LayerMask.GetMask("Enemy", "Hazard")))
        {
            myAnimator.SetTrigger("Death");
            GetComponent<Rigidbody2D>().velocity = deathKick;
            isAlive = false;

            FindObjectOfType<GameSession>().ProcessPlayerDeath();
        }
    }

    private void StateCheckTimer()
    {
        offGroundToJumpDelayTimer += Time.deltaTime;
        jumpDelayTimer += Time.deltaTime;
    }
    private void FlipSprite()
    {
        bool playerHasHorizontalSpeed = Mathf.Abs(myRigidbody2D.velocity.x) > SUPER_SMALL_VALUE;

        if (playerHasHorizontalSpeed)
        {
            transform.localScale = new Vector2(Mathf.Sign(myRigidbody2D.velocity.x), transform.localScale.y);
        }
    }
    private void StateCheck()
    {      

        //Debug.Log("OnClimb:" + isClimbing + "; OnGround:" + isOnGround + "; OnJump:" + isJumping);
    }
}
