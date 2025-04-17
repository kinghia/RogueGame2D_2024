using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerMovement : MonoBehaviour
{
    [Header("Move")]
    public float moveSpeed = 5f;
    [SerializeField] Vector2 moveInput;

    public bool isRun;
    public bool isFacingRight;

    [Header("Dash")]
    [SerializeField] float dashSpeed = 10f;
    [SerializeField] float dashDuration = 1f;
    [SerializeField] float dashCooldown = 1f;
    bool isDashing;
    bool canDash = true;
    

    Animator anim;
    Rigidbody2D rb;
    PlayerCombat playerCombat;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        playerCombat = FindFirstObjectByType<PlayerCombat>();

        canDash = true;
    }

    void Update()
    {
        Movement();
    }

    void Movement()
    {
        if(isDashing) return;
        
        if (!playerCombat.Attacking)
        {
            moveInput.x = Input.GetAxisRaw("Horizontal");
            moveInput.y = Input.GetAxisRaw("Vertical");
            moveInput.Normalize();

            if (moveInput.x > 0)
            {
                transform.rotation = Quaternion.Euler(0f, 0f, 0f);   
                isFacingRight = true;     
            }
            else if (moveInput.x < 0)
            {
                transform.rotation = Quaternion.Euler(0f, 180f, 0f);
                isFacingRight = false;
            }
        }
        else 
        {
            moveInput = Vector2.zero;
        }

        anim.SetFloat("Player_Run", moveInput.sqrMagnitude);

        if (Input.GetKeyDown(KeyCode.C) && canDash)
        {
            StartCoroutine(Dash());
            
            anim.SetTrigger("Dashing");
        }

    }

    void FixedUpdate()
    {
        if(isDashing) return;
        
        //rb.linearVelocity = new Vector2(moveInput.x * moveSpeed, moveInput.y * moveSpeed * Time.fixedDeltaTime);
        rb.MovePosition(rb.position + moveInput * moveSpeed * Time.fixedDeltaTime);
    }

    IEnumerator Dash()
    {
        canDash = false;
        isDashing = true;
        rb.linearVelocity = new Vector2(moveInput.x * dashSpeed, moveInput.y * dashSpeed);
        yield return new WaitForSeconds(dashDuration);
        isDashing = false;

        yield return new WaitForSeconds(dashCooldown);
        canDash = true;
    }
}
