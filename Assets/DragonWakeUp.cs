using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class DragonController : MonoBehaviour
{
    [Header("References")]
    public Transform player;
    public Transform G1;
    public Transform G2;
    public Transform G3;
    public GameObject goalObject; // ✅ 드래곤이 죽으면 나타날 Goal 오브젝트

    [Header("Settings")]
    public float wakeUpDistance = 5f;
    public float flySpeed = 6f;
    public float diveSpeed = 20f;
    public float riseSpeed = 5f;
    public float diveChance = 0.005f;
    public int maxHealth = 5;

    private int currentHealth;
    private Animator animator;
    private bool hasWokenUp = false;
    private bool isDiving = false;
    private bool isInvincible = false;
    private float cooldownTimer = 0f;
    private SpriteRenderer sr;
    private Vector3 lastDirection;

    void Start()
    {
        animator = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
        currentHealth = maxHealth;

        // ✅ Goal 오브젝트 처음에는 비활성화
        if (goalObject != null)
            goalObject.SetActive(false);
    }

    void Update()
    {
        if (!hasWokenUp && Vector2.Distance(transform.position, player.position) < wakeUpDistance)
        {
            animator.SetTrigger("WakeUp");
            hasWokenUp = true;
            StartCoroutine(WaitThenFlyToG1());
        }
    }

    IEnumerator WaitThenFlyToG1()
    {
        yield return new WaitForSeconds(1.5f);
        animator.SetTrigger("Fly");
        StartCoroutine(FlyToG1());
    }

    IEnumerator FlyToG1()
    {
        while (Vector2.Distance(transform.position, G1.position) > 0.1f)
        {
            FlipTowards(G1.position);
            transform.position = Vector2.MoveTowards(transform.position, G1.position, flySpeed * Time.deltaTime);
            yield return null;
        }

        StartCoroutine(FlyBetweenRocks());
    }

    IEnumerator FlyBetweenRocks()
    {
        Transform[] points = new Transform[] { G2, G3 };
        int currentIndex = 0;

        while (true)
        {
            Vector3 target = points[currentIndex].position;
            lastDirection = target;

            while (Vector2.Distance(transform.position, target) > 0.1f)
            {
                FlipTowards(target);
                transform.position = Vector2.MoveTowards(transform.position, target, flySpeed * Time.deltaTime);

                if (!isDiving && cooldownTimer <= 0f && Random.value < diveChance)
                {
                    StartCoroutine(DiveAttack());
                    yield break;
                }

                cooldownTimer -= Time.deltaTime;
                yield return null;
            }

            currentIndex = 1 - currentIndex;
            yield return new WaitForSeconds(0.3f);
        }
    }

    IEnumerator DiveAttack()
    {
        isDiving = true;
        animator.SetTrigger("Dive");

        Vector3 diveTarget = player.position;
        FlipTowards(diveTarget);

        while (Vector2.Distance(transform.position, diveTarget) > 0.1f)
        {
            transform.position = Vector2.MoveTowards(transform.position, diveTarget, diveSpeed * Time.deltaTime);
            yield return null;
        }

        Vector3 riseTarget = new Vector3(transform.position.x, G2.position.y, transform.position.z);

        while (Vector2.Distance(transform.position, riseTarget) > 0.1f)
        {
            transform.position = Vector2.MoveTowards(transform.position, riseTarget, riseSpeed * Time.deltaTime);
            yield return null;
        }

        animator.SetTrigger("Fly");

        cooldownTimer = 2f;
        isDiving = false;
        StartCoroutine(FlyBetweenRocksFrom(lastDirection));
    }

    IEnumerator FlyBetweenRocksFrom(Vector3 fromDirection)
    {
        Transform[] points = new Transform[] { G2, G3 };
        int currentIndex = (Vector3.Distance(fromDirection, G2.position) < Vector3.Distance(fromDirection, G3.position)) ? 1 : 0;

        while (true)
        {
            Vector3 target = points[currentIndex].position;
            lastDirection = target;

            while (Vector2.Distance(transform.position, target) > 0.1f)
            {
                FlipTowards(target);
                transform.position = Vector2.MoveTowards(transform.position, target, flySpeed * Time.deltaTime);

                if (!isDiving && cooldownTimer <= 0f && Random.value < diveChance)
                {
                    StartCoroutine(DiveAttack());
                    yield break;
                }

                cooldownTimer -= Time.deltaTime;
                yield return null;
            }

            currentIndex = 1 - currentIndex;
            yield return new WaitForSeconds(0.3f);
        }
    }

    void FlipTowards(Vector3 target)
    {
        if (sr != null)
        {
            sr.flipX = target.x > transform.position.x;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        playerComponent player = other.GetComponent<playerComponent>();
        if (player == null) return;

        Vector2 contactPoint = other.transform.position;
        float headY = transform.Find("HeadZone").position.y;

        if (contactPoint.y > headY)
        {
            TakeDamage(1);
            Rigidbody2D rb = other.GetComponent<Rigidbody2D>();
            if (rb != null)
                rb.velocity = new Vector2(rb.velocity.x, 4.2f); // 튕기기
        }
        else
        {
            if (player.isInvincible)
            {
                Debug.Log("무적 상태이므로 플레이어가 살아남음");
                return;
            }

            Debug.Log("플레이어 사망");
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); // 즉시 씬 재시작
        }
    }

    void TakeDamage(int amount)
    {
        if (isInvincible) return;

        currentHealth -= amount;
        Debug.Log("드래곤 피격! 현재 체력: " + currentHealth);
        StartCoroutine(InvincibilityCoroutine());

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    IEnumerator InvincibilityCoroutine()
    {
        isInvincible = true;
        yield return new WaitForSeconds(3f);
        isInvincible = false;
    }

    void Die()
    {
        animator.SetTrigger("Die");
        Debug.Log("드래곤 사망");

        // ✅ Goal 오브젝트 활성화
        if (goalObject != null)
        {
            goalObject.SetActive(true);
        }

        Destroy(gameObject, 1.5f);
    }
}