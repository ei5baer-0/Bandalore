using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Rigidbody2D rigidBody2D;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Animator animator;
    [SerializeField] private Countdown countdown;

    [Header("Variables")]
    [SerializeField] private float dashSpeed = 20f;         // Geschwindigkeit des Dashes
    [SerializeField] private float dashDuration = 0.1f;     // Dauer des Dashes
    [SerializeField] private float dashCooldown = 1f;       // Abklingzeit des Dashes
    [SerializeField] private float playerSpeed = 8f;        // Bewegungsgeschwindigkeit des Spielers
    [SerializeField] private float jumpForce = 8f;          // Sprungkraft fuer Spieler
    [SerializeField] private float coyoteBuffer = 0.1f;     // Variable fuer Coyote Jump
    [SerializeField] private float jumpBuffer = 0.1f;       // Variable fuer Jump Buffer

    private float playerHalfHeight;
    private float playerWidth;
    private float horizontal;                               // -1 == rechts | 1 == links
    private float dashCooldownTimer;                        // Timer fuer die Abklingzeit des Dashes
    private float coyoteTimer;                              // Timer fuer Coyote Buffer
    private float jumpBufferTimer;                          
    private float wasDashingTimer;
    private float dashTime;                                 // Variable fuer Dash Laenge wird berechnet

    private bool isDashing = false;
    private bool start = false;

    void Start()
    {
        playerHalfHeight = spriteRenderer.bounds.extents.y;
        playerWidth = spriteRenderer.bounds.extents.x;

        UpdateAnimClipLenght();
    }

    void Update()
    {
        start = countdown.gameStart;
        FlipCharacterX();
        DashCooldownTimer();
        WasDashingTimer();
        IsGrounded();
        DoJump();
        Falling();
    }

    private void FixedUpdate()
    {
        // Bewege den Spieler horizontal
        if (!isDashing && start == true)
        {
            rigidBody2D.linearVelocity = new Vector2(horizontal * playerSpeed, rigidBody2D.linearVelocity.y);
        }
    }

    public void Dash(InputAction.CallbackContext context)
    {
        // Wenn die Dash-Aktion ausgeloest wird und die Abklingzeit abgelaufen ist, starte das Dash
        if (context.performed && dashCooldownTimer <= 0f && start == true)
        {
            StartCoroutine(DoDash());
        }
    }

    // Methode, die den Dash ausfuehrt
    IEnumerator DoDash()
    {
        if (horizontal != 0f && start == true)
        {
            wasDashingTimer = dashTime;
            isDashing = true;
            animator.SetBool("IsDashing", true);
            float _dashTimer = dashDuration;

            Vector2 dashDirection = new Vector2(horizontal, 0f).normalized;

            while (_dashTimer > 0f)
            {
                rigidBody2D.linearVelocity = dashDirection * dashSpeed;
                _dashTimer -= Time.deltaTime;
                yield return null;
            }

            rigidBody2D.linearVelocity = new Vector2(horizontal * playerSpeed, rigidBody2D.linearVelocity.y);
            isDashing = false;
            animator.SetBool("IsDashing", false);

            dashCooldownTimer = dashCooldown;
        }
    }

    public void Move(InputAction.CallbackContext context)
    {
        if (start == true)
        {
            horizontal = context.ReadValue<Vector2>().x;
            animator.SetFloat("Horizontal", Mathf.Abs(horizontal));
        }
    }

    public void Jump(InputAction.CallbackContext context)
    {
        if (context.performed && start == true)
        {
            jumpBufferTimer = jumpBuffer;
        }
    }

    private void DoJump()
    {
        if (jumpBufferTimer > 0)
        {
            jumpBufferTimer -= Time.deltaTime;
        }

        if (jumpBufferTimer > 0 && coyoteTimer > 0)
        {
            rigidBody2D.linearVelocity = new Vector2(rigidBody2D.linearVelocity.x, 0f);
            rigidBody2D.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);

            jumpBufferTimer = 0f;
            coyoteTimer = 0f;
        }
    }

    private bool GetIsGrounded()
    {
        int _groundMask = LayerMask.GetMask("Ground");

        // 3 Punkte werden brechnet, ganz links, mitte und ganz rechts
        Vector2 center = (Vector2)transform.position;
        Vector2 left = center + Vector2.left * playerWidth / 2.5f;
        Vector2 right = center + Vector2.right * playerWidth / 2.5f;

        // Rays werden nach unten geworfen an den 3 vorherigen Berechneten Punkten und gecheckt ob die Maske "Ground" getroffen wurde
        bool _hitCenter = Physics2D.Raycast(center, Vector2.down, playerHalfHeight + 0.1f, _groundMask);
        bool _hitLeft = Physics2D.Raycast(left, Vector2.down, playerHalfHeight + 0.1f, _groundMask);
        bool _hitRight = Physics2D.Raycast(right, Vector2.down, playerHalfHeight + 0.1f, _groundMask);

        //Debug.DrawRay(center, Vector2.down * (playerHalfHeight + 0.1f), Color.green);
        //Debug.DrawRay(left, Vector2.down * (playerHalfHeight + 0.1f), Color.red);
        //Debug.DrawRay(right, Vector2.down * (playerHalfHeight + 0.1f), Color.blue);

        // Gebe wieder ob einer von den dreien "Ground" getreffen hat
        return _hitCenter || _hitLeft || _hitRight;
    }

    private void IsGrounded()
    {
        if (GetIsGrounded())
        {
            coyoteTimer = coyoteBuffer;
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
        // Timer für Dash Cooldown
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
        if (rigidBody2D.linearVelocity.y <= 0 && !GetIsGrounded())
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

    private void UpdateAnimClipLenght()
    {
        foreach (AnimationClip clip in animator.runtimeAnimatorController.animationClips)
        {
            if (clip.name == "Dash")
            {
                dashTime = clip.length;
                break;
            }
        }
    }
}
