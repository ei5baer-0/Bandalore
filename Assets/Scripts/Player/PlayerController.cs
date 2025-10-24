using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{

    public Rigidbody2D rb; // Referenz auf den Rigidbody des Spielers

    public float dashSpeed = 20f; // Geschwindigkeit des Dashes
    public float dashDuration = 0.1f; // Dauer des Dashes
    public float dashCooldown = 1f; // Abklingzeit des Dashes
    public float speed = 8f; // Bewegungsgeschwindigkeit des Spielers
    public float jumpforce = 5f; //Sprungkraft für den Spieler

    private float horizontal; // Horizontale Eingabe fuer die Bewegung
    private float dashTimer = 0f; // Timer fuer die Dauer des Dashes
    private float dashCooldownTimer = 0f; // Timer fuer die Abklingzeit des Dashes
    
    private bool isDead = false; // Gibt an, ob der Spieler tot ist
    private bool isDashing = false; // Gibt an, ob der Spieler gerade dashed

    void Update()
    {
        if (isDead) return;

        // Reduziere den Timer fuer die Dash-Abklingzeit
        if (dashCooldownTimer > 0f)
        {
            dashCooldownTimer -= Time.deltaTime;
        }
        
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
    // Wenn der Spieler tot oder am Angreifen ist, wird nichts ausgefuehrt
        if (isDead) return;

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
        if (isDead) return;

        horizontal = context.ReadValue<Vector2>().x;
    }
    
    public void Jump(InputAction.CallbackContext context)
    {

        if (context.performed && !isDead && GetIsGrounded())
        {
            rb.AddForce(Vector2.up * jumpforce, ForceMode2D.Impulse);
        }
    }

    public bool GetIsGrounded()
    {
        return Physics2D.Raycast(transform.position, Vector2.down, 0.8f, LayerMask.GetMask("Ground"));
        
    }

}
