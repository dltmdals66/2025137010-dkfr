using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class playerComponent : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 1f;
    public float jumpForce = 2.2f;
    public Transform groundCheck;
    public LayerMask groundLayer;

    [Header("Effects")]
    public GameObject glowEffect;
    public float glowEffectDuration = 3f;
    public GameObject currentCrownEffect;
    public GameObject 무적임펙트;

    [Header("State Flags")]
    public bool isInvincible = false;
    private bool isGiant = false;           // 🔵 거대화 플래그

    [Header("Jump Control")]
    private float originalJumpForce;
    private int jumpCount;
    public int maxJumpCount = 1;
    private Coroutine jumpResetCoroutine;
    private Coroutine jumpBoostCoroutine;

    [Header("Boost Control")]
    private Coroutine glowEffectCoroutine;
    private Coroutine speedBoostCoroutine;

    [Header("Components")]
    private Rigidbody2D rb;
    private Animator animator;
    private bool isGrounded;

    // ✅ 점수 관련
    private float score;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        originalJumpForce = jumpForce;

        score = 1000f; // 시작 점수

        if (glowEffect != null)
            glowEffect.SetActive(false);

        if (currentCrownEffect != null)
            currentCrownEffect.SetActive(false);

        if (무적임펙트 != null)
            무적임펙트.SetActive(false);
    }

    private void Update()
    {
        // 이동 & 애니메이션
        float moveInput = Input.GetAxisRaw("Horizontal");
        bool isMoving = Mathf.Abs(moveInput) > 0;
        animator.SetFloat("Speed", isMoving ? 1f : 0f);
        rb.velocity = new Vector2(moveInput * moveSpeed, rb.velocity.y);
        if (isMoving)
            transform.localScale = new Vector3(Mathf.Sign(moveInput), 1, 1);

        // 점프 체크
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, 0.1f, groundLayer);
        if (isGrounded && rb.velocity.y <= 0)
            jumpCount = 0;
        animator.SetBool("IsJumping", !isGrounded && Mathf.Abs(rb.velocity.y) > 0.01f);

        if (Input.GetKeyDown(KeyCode.Space) && jumpCount < maxJumpCount)
        {
            rb.velocity = new Vector2(rb.velocity.x, 0f);
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            jumpCount++;
        }

        // 점수 감소
        score -= Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // ─── Item 태그 처리 ────────────────────────
        if (collision.CompareTag("Item"))
        {
            isGiant = true;                                              // 거대화 시작
            score += collision.GetComponent<ItemObject>().GetPoint();    // ItemObject에서 point 가져와 더하기
            Destroy(collision.gameObject);                               // 아이템 제거
            return;                                                      // 이후 로직 스킵
        }

        // ─── Respawn 태그 처리 ─────────────────────
        if (collision.CompareTag("Respawn"))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        // ─── Finish 태그 처리 ──────────────────────
        if (collision.CompareTag("Finish"))
        {
            StageResultSaver.SaveStage(
                SceneManager.GetActiveScene().buildIndex,
                (int)score
            );

            LevelObject levelObj = collision.GetComponent<LevelObject>();
            if (levelObj != null)
                levelObj.moveTonextLevel();
        }

        // ─── DoubleJumpShoes 태그 처리 ───────────────
        if (collision.CompareTag("DoubleJumpShoes"))
        {
            maxJumpCount = 2;
            Destroy(collision.gameObject);

            if (jumpResetCoroutine != null)
                StopCoroutine(jumpResetCoroutine);

            jumpResetCoroutine = StartCoroutine(ResetJumpAfterDelay(10f));
            EnableGlowEffect();
        }

        // ─── JumpBoost 태그 처리 ────────────────────
        if (collision.CompareTag("JumpBoost"))
        {
            if (jumpBoostCoroutine != null)
                StopCoroutine(jumpBoostCoroutine);

            jumpBoostCoroutine = StartCoroutine(JumpBoostRoutine(5f, 2.3f));
            Destroy(collision.gameObject);
        }

        // ─── SpeedItem 태그 처리 ────────────────────
        if (collision.CompareTag("SpeedItem"))
        {
            if (speedBoostCoroutine != null)
                StopCoroutine(speedBoostCoroutine);

            speedBoostCoroutine = StartCoroutine(SpeedBoostRoutine(3f, 4f));
            Destroy(collision.gameObject);
        }

        // ─── InvincibilityItem 태그 처리 ───────────
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
            else if (!isInvincible)
            {
                Die();
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
