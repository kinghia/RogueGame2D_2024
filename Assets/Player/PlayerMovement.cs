using System;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f; // Tốc độ di chuyển
    public float MoveSpeed {get {return moveSpeed;}}
    Rigidbody2D rb;
    public Vector2 moveInput;
    SpriteRenderer spriteRenderer;
    Animator anim;
    

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        // Nhận input từ bàn phím
        moveInput.x = Input.GetAxisRaw("Horizontal");
        moveInput.y = Input.GetAxisRaw("Vertical");  
        moveInput.Normalize();

        // Lật nhân vật nếu đi sang trái
        if (moveInput.x < 0)
        {
            spriteRenderer.flipX = true;
        }
        else if (moveInput.x > 0)
        {
            spriteRenderer.flipX = false;
        }

        anim.SetFloat("isRun", moveInput.sqrMagnitude);
    }

    void FixedUpdate()
    {
        // Di chuyển nhân vật
        //rb.linearVelocity = moveInput * moveSpeed;
        rb.MovePosition(rb.position + moveInput * moveSpeed * Time.fixedDeltaTime);
    }
}
