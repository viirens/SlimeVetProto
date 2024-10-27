using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class CharacterController2D : MonoBehaviour
{
    Rigidbody2D rigidbody2d;
    [SerializeField] float speed = 5f;
    Vector2 motionVector;
    public Vector2 lastMotionVector;
    SpriteRenderer spriteRenderer; 
    public bool moving;

    // New variables for dash
    [SerializeField] float dashSpeed = 10f;
    [SerializeField] float dashTime = 0.2f;
    private bool isDashing;
    private float dashTimeCounter;

    void Awake()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>(); // Initialize SpriteRenderer
    }

    void Update()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        motionVector = new Vector2(horizontal, vertical);
        RotateSprite();

        moving = horizontal != 0 || vertical != 0;

        if (moving)
        {
            lastMotionVector = motionVector.normalized;
        }

        // Dash logic
        if (Input.GetKeyDown(KeyCode.LeftShift) && !isDashing)
        {
            if (moving)
            {
                isDashing = true;
                dashTimeCounter = dashTime;
            }
        }

        if (isDashing)
        {
            dashTimeCounter -= Time.deltaTime;
            if (dashTimeCounter <= 0)
            {
                isDashing = false;
            }
        }
    }

    void FixedUpdate()
    {
        Move();
    }

    void Move()
    {
        // Normalize the motion vector to ensure consistent speed
        Vector2 normalizedMotionVector = motionVector.normalized;
        float currentSpeed = speed;

        if (isDashing)
        {
            currentSpeed = dashSpeed;
        }

        rigidbody2d.velocity = normalizedMotionVector * currentSpeed;
    }

    void RotateSprite()
    {
        if (motionVector != Vector2.zero)
        {
            if (motionVector.x < 0) 
            {
                transform.rotation = Quaternion.Euler(new Vector3(0, 0, -90));
            }
            else if (motionVector.x > 0)
            {
                transform.rotation = Quaternion.Euler(new Vector3(0, 0, 90));
            }
            else if (motionVector.y < 0)
            {
                transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
            }
            else if (motionVector.y > 0)
            {
                transform.rotation = Quaternion.Euler(new Vector3(0, 0, 180));
            }
        }
    }
}
