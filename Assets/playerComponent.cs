using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class playerComponent : MonoBehaviour
{
    public float moveSpeed = 1f;
    public float jumpForce = 2.2f;
    public Transform groundCheck;
    public LayerMask groundLayer;
    public GameObject glowEffect;
    public float glowEffectDuration = 3f;

    public GameObject currentCrownEffect;
    public GameObject 무적임펙트;
    public bool isInvincible = false;

    private float originalJumpForce;
    private Coroutine jumpResetCoroutine;
    private Coroutine jumpBoostCoroutine;
    private Coroutine glowEffectCoroutine;
    private Coroutine speedBoostCoroutine;

    private Rigidbody2D rb;
    private Animator animator;
    private bool isGrounded;
    private int jumpCount;
    public int maxJumpCount = 1;

    // ✅ 점수 관련
    private float score;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        originalJumpForce = jumpForce;

        score = 1000f; // ✅ 시작 점수

        if (glowEffect != null)
            glowEffect.SetActive(false);

        if (currentCrownEffect != null)
            currentCrownEffect.SetActive(false);

        if (무적임펙트 != null)
            무적임펙트.SetActive(false);
    }

    private void Update()
    {
        float moveInput = Input.GetAxisRaw("Horizontal");
        bool isMoving = Mathf.Abs(moveInput) > 0;
        animator.SetFloat("Speed", isMoving ? 1f : 0f);

        rb.velocity = new Vector2(moveInput * moveSpeed, rb.velocity.y);

        isGrounded = Physics2D.OverlapCircle(groundCheck.position, 0.1f, groundLayer);
        if (isGrounded && rb.velocity.y <= 0)
        {
            jumpCount = 0;
        }

        animator.SetBool("IsJumping", !isGrounded && Mathf.Abs(rb.velocity.y) > 0.01f);

        if (Input.GetKeyDown(KeyCode.Space) && jumpCount < maxJumpCount)
        {
            rb.velocity = new Vector2(rb.velocity.x, 0f);
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            jumpCount++;
        }

        if (isMoving)
        {
            transform.localScale = new Vector3(Mathf.Sign(moveInput), 1, 1);
        }

        // ✅ 점수 감소
        score -= Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Respawn"))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        if (collision.CompareTag("Finish"))
        {
            int stage = SceneManager.GetActiveScene().buildIndex;
            HighScore.TrySet(stage, (int)score); // ✅ 점수 저장

            LevelObject levelObj = collision.GetComponent<LevelObject>();
            if (levelObj != null)
                levelObj.moveTonextLevel();
        }

        if (collision.CompareTag("DoubleJumpShoes"))
        {
            maxJumpCount = 2;
            Destroy(collision.gameObject);

            if (jumpResetCoroutine != null)
                StopCoroutine(jumpResetCoroutine);

            jumpResetCoroutine = StartCoroutine(ResetJumpAfterDelay(10f));
            EnableGlowEffect();
        }

        if (collision.CompareTag("JumpBoost"))
        {
            if (jumpBoostCoroutine != null)
                StopCoroutine(jumpBoostCoroutine);

            jumpBoostCoroutine = StartCoroutine(JumpBoostRoutine(5f, 2.3f));
            Destroy(collision.gameObject);
        }

        if (collision.CompareTag("SpeedItem"))
        {
            if (speedBoostCoroutine != null)
                StopCoroutine(speedBoostCoroutine);

            speedBoostCoroutine = StartCoroutine(SpeedBoostRoutine(3f, 4f));
            Destroy(collision.gameObject);
        }

        if (collision.CompareTag("InvincibilityItem"))
        {
            StartCoroutine(BecomeInvincible(4f));
            Destroy(collision.gameObject);
        }
    }

    private IEnumerator ResetJumpAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        maxJumpCount = 1;
    }

    private IEnumerator JumpBoostRoutine(float duration, float boostMultiplier)
    {
        jumpForce = originalJumpForce * boostMultiplier;
        yield return new WaitForSeconds(duration);
        jumpForce = originalJumpForce;
    }

    public IEnumerator SpeedBoostRoutine(float multiplier, float duration)
    {
        moveSpeed *= multiplier;
        EnableGlowEffect();
        yield return new WaitForSeconds(duration);
        moveSpeed /= multiplier;
    }

    public void Die()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void EnableGlowEffect()
    {
        if (glowEffect == null) return;

        glowEffect.SetActive(true);

        if (glowEffectCoroutine != null)
            StopCoroutine(glowEffectCoroutine);

        glowEffectCoroutine = StartCoroutine(DisableGlowEffectAfterDelay());
    }

    private IEnumerator DisableGlowEffectAfterDelay()
    {
        yield return new WaitForSeconds(glowEffectDuration);
        glowEffect.SetActive(false);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Enemy"))
        {
            Vector2 contactPoint = collision.contacts[0].point;
            Vector2 playerCenter = GetComponent<Collider2D>().bounds.center;

            if (playerCenter.y > contactPoint.y)
            {
                rb.velocity = new Vector2(rb.velocity.x, 0f);
                rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            }
            else
            {
                if (!isInvincible)
                {
                    Die();
                }
            }
        }
    }

    public IEnumerator BecomeInvincible(float duration)
    {
        isInvincible = true;

        if (무적임펙트 != null)
            무적임펙트.SetActive(true);

        yield return new WaitForSeconds(duration);

        isInvincible = false;

        if (무적임펙트 != null)
            무적임펙트.SetActive(false);
    }
}
