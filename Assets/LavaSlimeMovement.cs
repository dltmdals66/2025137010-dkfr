using UnityEngine;

public class LavaSlimeMovement : MonoBehaviour
{
    public float upY = 0.8f;
    public float downY = -0.5f;
    public float speed = 2f;

    [Header("Jump Timing")]
    public float waitTime = 1f;              // ���� �� ��� �ð�
    public bool useRandomWaitTime = false;   // ���� �������
    public float minWaitTime = 0.5f;         // ���� �ּҰ�
    public float maxWaitTime = 2f;           // ���� �ִ밪

    [Header("Sprites")]
    public Sprite upSprite;
    public Sprite downSprite;

    private Vector3 startPos;
    private float timer = 0f;
    private bool goingUp = true;

    private SpriteRenderer sr;

    void Start()
    {
        startPos = transform.position;
        sr = GetComponent<SpriteRenderer>();

        if (useRandomWaitTime)
        {
            waitTime = Random.Range(minWaitTime, maxWaitTime);
        }

        if (goingUp && upSprite != null)
            sr.sprite = upSprite;
    }

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= waitTime)
        {
            goingUp = !goingUp;
            timer = 0f;

            if (useRandomWaitTime)
                waitTime = Random.Range(minWaitTime, maxWaitTime); // ���� ���� �ֱ⵵ �����ϰ�

            if (goingUp && upSprite != null)
                sr.sprite = upSprite;
            else if (!goingUp && downSprite != null)
                sr.sprite = downSprite;
        }

        float targetY = goingUp ? startPos.y + upY : startPos.y + downY;
        transform.position = Vector3.Lerp(transform.position, new Vector3(startPos.x, targetY, startPos.z), Time.deltaTime * speed);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerComponent player = other.GetComponent<playerComponent>();
            if (player != null)
            {
                player.Die();
            }
        }
    }
}