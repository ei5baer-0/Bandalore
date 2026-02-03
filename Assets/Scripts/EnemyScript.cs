using UnityEngine;
using System.Collections;

public class EnemyScript : MonoBehaviour
{
    private enum State
    {
        FireSpirit, BlackHole, BubbleMan
    }

    [Header("References")]
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Transform player;

    [Header("EnemyType")]
    [SerializeField] private State type;

    [Header("FireSpirit")]
    [SerializeField] private float speed = 2f;

    [Header("BubbleMan")]
    [SerializeField] private Transform shootPoint;
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private float shootInterval = 2f;
    [SerializeField] private float shootDistance = 10f;

    private float enemyHalfHeight;
    private float enemyWidth;
    private int direction = 1;
    private Coroutine shootingCoroutine;

    void Start()
    {
        enemyHalfHeight = spriteRenderer.bounds.extents.y;
        enemyWidth = spriteRenderer.bounds.extents.x;
        spriteRenderer.flipX = true;
    }

    void Update()
    {
        switch (type)
        {
            case State.FireSpirit:
                FireSpirit();
                break;

            case State.BlackHole:
                BlackHole();
                break;

            case State.BubbleMan:
                BubbleMan();
                break;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerController player = collision.gameObject.GetComponent<PlayerController>();

            if (player != null)
            {
                player.TakeDamage();
                player.ApplyKnockback(transform.position);
            }
        }
    }

    private void FireSpirit()
    {
        transform.Translate(Vector2.right * direction * speed * Time.deltaTime);

        if (!IsOnEdge())
        {
            FlipDirection();
        }
    }

    private void BlackHole()
    {
        return;
    }

    private void BubbleMan()
    {
        if (shootingCoroutine == null)
            StartShooting();
    }

    private bool IsOnEdge()
    {
        int _groundMask = LayerMask.GetMask("Ground");

        Vector2 center = (Vector2)transform.position;
        Vector2 left = center + Vector2.left * enemyWidth;
        Vector2 right = center + Vector2.right * enemyWidth;

        bool _hitLeft = Physics2D.Raycast(left, Vector2.down, enemyHalfHeight + 0.1f, _groundMask);
        bool _hitRight = Physics2D.Raycast(right, Vector2.down, enemyHalfHeight + 0.1f, _groundMask);

        return _hitLeft && _hitRight;
    }

    private void FlipDirection()
    {
        direction *= -1;
        Vector3 scale = transform.localScale;
        scale.x = Mathf.Abs(scale.x) * direction;
        transform.localScale = scale;
    }

    void OnEnableShooting()
    {
        StartShooting();
    }

    void OnDisableShooting()
    {
        StopShooting();
    }

    public void StopShooting()
    {
        if (shootingCoroutine != null)
        {
            StopCoroutine(shootingCoroutine);
            shootingCoroutine = null;
        }
    }

    public void StartShooting()
    {
        if (shootingCoroutine == null)
            shootingCoroutine = StartCoroutine(ShootRoutine());
    }

    private IEnumerator ShootRoutine()
    {
        while (true)
        {
            if (player != null)
            {
                float distanceToPlayer = Vector2.Distance(shootPoint.position, player.position);

                if (distanceToPlayer <= shootDistance)
                {
                    Shoot();
                }
            }

            yield return new WaitForSeconds(shootInterval);
        }
    }

    private void Shoot()
    {
        Vector2 directionToPlayer = (player.position - shootPoint.position).normalized;

        GameObject proj = Instantiate(projectilePrefab, shootPoint.position, Quaternion.identity);
        Projectile projectile = proj.GetComponent<Projectile>();
        projectile.Initialize(directionToPlayer);
    }

}
