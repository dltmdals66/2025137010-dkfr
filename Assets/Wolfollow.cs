using UnityEngine;
using UnityEngine.SceneManagement;

public class WolfFollow : MonoBehaviour
{
    public Transform target;
    public float moveSpeed = 1.2f;
    public float jumpForce = 2.2f;
    public float stopDistance = 0f;

    public LayerMask groundLayer;
    public Transform groundCheck;
    public float groundCheckRadius = 0.2f;

    public Transform wallCheck;
    public float wallCheckDistance = 0.1f;

    private Rigidbody2D rb;
    private bool isGrounded;
    private bool facingRight = true;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (target == null) return;

        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

        float distance = Vector2.Distance(transform.position, target.position);

        if (distance > stopDistance)
        {
            Vector2 direction = (target.position - transform.position).normalized;

            Vector2 wallDir = new Vector2(Mathf.Sign(direction.x), 0f);
            bool isWallAhead = Physics2D.Raycast(wallCheck.position, wallDir, wallCheckDistance, groundLayer);

            rb.velocity = new Vector2(direction.x * moveSpeed, rb.velocity.y);

            if (isWallAhead && isGrounded)
            {
                rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            }

            if ((direction.x > 0 && !facingRight) || (direction.x < 0 && facingRight))
            {
                Flip();
            }
        }
    }

    void Flip()
    {
        facingRight = !facingRight;
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("Player Dead!");
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (wallCheck != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(wallCheck.position, wallCheck.position + Vector3.right * wallCheckDistance);
        }

        if (groundCheck != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
        }
    }
}