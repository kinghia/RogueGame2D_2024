using System;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float MoveSpeed {get {return moveSpeed;}}
    public Vector2 moveInput;

    public bool isRun;

    Animator anim;
    Rigidbody2D rb;
    PlayerCombat playerCombat;
    

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        playerCombat = FindFirstObjectByType<PlayerCombat>();
    }

    void Update()
    {
        Movement();
    }

    void Movement()
    {
        if (!playerCombat.Attacking)
        {
            moveInput.x = Input.GetAxisRaw("Horizontal");
            moveInput.y = Input.GetAxisRaw("Vertical");
            moveInput.Normalize();

            if (moveInput.x > 0)
            {
                transform.rotation = Quaternion.Euler(0f, 0f, 0f); // nhìn sang phải          
            }
            else if (moveInput.x < 0)
            {
                transform.rotation = Quaternion.Euler(0f, 180f, 0f); // xoay sang trái
            }
        }
        else 
        {
            moveInput = Vector2.zero;
        }
        

        anim.SetFloat("Player_Run", moveInput.sqrMagnitude);
    }

    void FixedUpdate()
    {
        //rb.linearVelocity = moveInput * moveSpeed;
        rb.MovePosition(rb.position + moveInput * moveSpeed * Time.fixedDeltaTime);
    }
}
