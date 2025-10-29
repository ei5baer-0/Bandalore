using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public Rigidbody2D rb; // Referenz auf den Rigidbody des Spielers

    [SerializeField] private float dashSpeed = 20f; // Geschwindigkeit des Dashes
    [SerializeField] private float dashDuration = 0.1f; // Dauer des Dashes
    [SerializeField] private float dashCooldown = 1f; // Abklingzeit des Dashes
    [SerializeField] private float speed = 8f; // Bewegungsgeschwindigkeit des Spielers
    [SerializeField] private float jumpforce = 5f; //Sprungkraft für den Spieler
    [SerializeField] private SpriteRenderer spriteRenderer; //SpriteRenderer verknuepfen
    [SerializeField] private Animator animator; //AnimatorController verknuepfen

    private float horizontal; // Horizontale Eingabe fuer die Bewegung
    private float dashTimer = 0f; // Timer fuer die Dauer des Dashes
    private float dashCooldownTimer = 0f; // Timer fuer die Abklingzeit des Dashes
    private float playerheight;

    private bool isDashing = false; // Gibt an, ob der Spieler gerade dashed

    void Update()
    {
        FlipCharacterX();
        DashCooldownTimer();
    }

    private void FixedUpdate()
    {
        // Bewege den Spieler horizontal basierend auf der horizontalen Eingabe
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
            isDashing = true;
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

            dashCooldownTimer = dashCooldown;
        }
        /*else ()
        {

        } */
    }

    public void Move(InputAction.CallbackContext context)
    {
        horizontal = context.ReadValue<Vector2>().x;
        animator.SetFloat("SpeedY", Mathf.Abs(horizontal));
    }

    public void Jump(InputAction.CallbackContext context)
    {

        if (context.performed && GetIsGrounded())
        {
            rb.AddForce(Vector2.up * jumpforce, ForceMode2D.Impulse);
        }
    }

    public bool GetIsGrounded()
    {
        return Physics2D.Raycast(transform.position, Vector2.down, 4f, LayerMask.GetMask("Ground"));

    }

    private void DashCooldownTimer()
    {
        // Reduziere den Timer fuer die Dash-Abklingzeit
        if (dashCooldownTimer > 0f)
        {
            dashCooldownTimer -= Time.deltaTime;
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
}
