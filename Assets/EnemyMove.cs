using UnityEngine;

public class EnemyMove : MonoBehaviour
{
    public float moveSpeed = 0.6f;
    public Transform leftLimit;
    public Transform rightLimit;

    private Rigidbody2D rb;
    private bool movingRight = true;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        Move();
    }

    void Move()
    {
        float moveDirection = movingRight ? 1f : -1f;
        rb.velocity = new Vector2(moveDirection * moveSpeed, rb.velocity.y);

        // 🟨 이 부분이 핵심 (왼쪽이 기본 방향일 경우)
        transform.localScale = new Vector3(movingRight ? -1f : 1f, 1f, 1f);

        if (movingRight && transform.position.x >= rightLimit.position.x)
        {
            movingRight = false;
        }
        else if (!movingRight && transform.position.x <= leftLimit.position.x)
        {
            movingRight = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.GetComponent<playerComponent>().Die();
        }
    }
}