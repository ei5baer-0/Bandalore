using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public Rigidbody2D rb;

    [SerializeField] private float dashSpeed = 20f; // Geschwindigkeit des Dashes
    [SerializeField] private float dashDuration = 0.1f; // Dauer des Dashes
    [SerializeField] private float dashCooldown = 1f; // Abklingzeit des Dashes
    [SerializeField] private float speed = 8f; // Bewegungsgeschwindigkeit des Spielers
    [SerializeField] private float jumpforce = 5f; //Sprungkraft fuer den Spieler
    [SerializeField] private float coyoteOffset = 0.1f;
    [SerializeField] private float jumpBuffer = 0.1f;
    [SerializeField] private float wasDashing = 5f; //Variable für Timer

    [SerializeField] private SpriteRenderer spriteRenderer; //SpriteRenderer verknuepfen
    [SerializeField] private Animator animator; //AnimatorController verknuepfen

    private float horizontal; // Horizontale Eingabe fuer die Bewegung
    private float dashTimer; // Timer fuer die Dauer des Dashes
    private float dashCooldownTimer; // Timer fuer die Abklingzeit des Dashes
    private float playerHalfHeight;
    private float playerWidth;
    private float coyoteTimer;
    private float jumpBufferTimer;
    private float wasDashingTimer; //Timer

    private bool isDashing = false; // Gibt an, ob der Spieler gerade dashed

    void Start()
    {
        playerHalfHeight = spriteRenderer.bounds.extents.y;
        playerWidth = spriteRenderer.bounds.extents.x;
    }

    void Update()
    {
        FlipCharacterX();
        DashCooldownTimer();
        WasDashingTimer();
        IsGrounded();
        JumpBuffer();
        Falling();
    }

    private void FixedUpdate()
    {
        // Bewege den Spieler horizontal
        if (!isDashing)
        {
            rb.linearVelocity = new Vector2(horizontal * speed, rb.linearVelocity.y);
        }
    }

    public void Dash(InputAction.CallbackContext context)
    {
        // Wenn die Dash-Aktion ausgeloest wird und die Abklingzeit abgelaufen ist, starte das Dash
        if (context.performed && dashCooldownTimer <= 0f)
        {
            StartCoroutine(DoDash());
        }
    }

    // Methode, die den Dash ausfuehrt
    IEnumerator DoDash()
    {
        if (horizontal != 0f)
        {
            wasDashingTimer = wasDashing;
            isDashing = true;
            animator.SetBool("IsDashing", true);
            dashTimer = dashDuration;

            Vector2 dashDirection = new Vector2(horizontal, 0f).normalized;

            while (dashTimer > 0f)
            {
                rb.linearVelocity = dashDirection * dashSpeed;
                dashTimer -= Time.deltaTime;
                yield return null;
            }

            rb.linearVelocity = new Vector2(horizontal * speed, rb.linearVelocity.y);
            isDashing = false;
            animator.SetBool("IsDashing", false);

            dashCooldownTimer = dashCooldown;
        }
    }

    public void Move(InputAction.CallbackContext context)
    {
        horizontal = context.ReadValue<Vector2>().x;
        animator.SetFloat("Horizontal", Mathf.Abs(horizontal));
    }

    public void Jump(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            jumpBufferTimer = jumpBuffer;
        }
    }

    private void JumpBuffer()
    {
        if (jumpBufferTimer > 0)
        {
            jumpBufferTimer -= Time.deltaTime;
        }

        if (jumpBufferTimer > 0 && coyoteTimer > 0)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0f);
            rb.AddForce(Vector2.up * jumpforce, ForceMode2D.Impulse);

            jumpBufferTimer = 0f;
            coyoteTimer = 0f;
        }
    }

    public bool GetIsGrounded()
    {
        int groundMask = LayerMask.GetMask("Ground");

        Vector2 center = (Vector2)transform.position;
        Vector2 left = center + Vector2.left * playerWidth / 2.5f;
        Vector2 right = center + Vector2.right * playerWidth / 2.5f;

        bool hitCenter = Physics2D.Raycast(center, Vector2.down, playerHalfHeight + 0.1f, groundMask);
        bool hitLeft = Physics2D.Raycast(left, Vector2.down, playerHalfHeight + 0.1f, groundMask);
        bool hitRight = Physics2D.Raycast(right, Vector2.down, playerHalfHeight + 0.1f, groundMask);

        //Debug.DrawRay(center, Vector2.down * (playerHalfHeight + 0.1f), Color.green);
        //Debug.DrawRay(left, Vector2.down * (playerHalfHeight + 0.1f), Color.red);
        //Debug.DrawRay(right, Vector2.down * (playerHalfHeight + 0.1f), Color.blue);

        return hitCenter || hitLeft || hitRight;
    }

    private void IsGrounded()
    {
        if (GetIsGrounded())
        {
            coyoteTimer = coyoteOffset;
            animator.SetBool("IsGrounded", true);
        }
        else
        {
            coyoteTimer -= Time.deltaTime;
            animator.SetBool("IsGrounded", false);
        }
    }

    private void DashCooldownTimer()
    {
        // Reduziere den Timer fuer die Dash-Abklingzeit
        if (dashCooldownTimer > 0f)
        {
            dashCooldownTimer -= Time.deltaTime;
        }
    }

    private void WasDashingTimer()
    {
        if (wasDashingTimer > 0f)
        {
            animator.SetBool("WasDashing", true);
            wasDashingTimer -= Time.deltaTime;
        }
        else
        {
            animator.SetBool("WasDashing", false);
        }
    }

    private void FlipCharacterX()
    {
        if (horizontal == -1)
        {
            //Bewegung nach Rechts
            spriteRenderer.flipX = true;
        }
        else if (horizontal == 1)
        {
            //Bewegung nach Links
            spriteRenderer.flipX = false;
        }
    }

    private void Falling()
    {
        if (rb.linearVelocity.y <= 0 && !GetIsGrounded())
        {
            animator.SetBool("IsRising", false);
            animator.SetBool("IsDecending", true);
        }
        else 
        {
            animator.SetBool("IsRising", true);
            animator.SetBool("IsDecending", false);
        }
    }

}
