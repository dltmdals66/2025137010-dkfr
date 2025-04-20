using UnityEngine;

public class DoubleJumpShoes : MonoBehaviour
{
    public int extraJumps = 1;
    public float effectDuration = 10f;  // 임팩트 지속 시간도 10초로 설정

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerComponent player = other.GetComponent<playerComponent>();
            if (player != null)
            {
                player.maxJumpCount += extraJumps;

                // GlowEffect 찾고 10초 동안 활성화
                Transform glow = other.transform.Find("GlowEffect");
                if (glow != null)
                {
                    GameObject glowObj = glow.gameObject;
                    glowObj.SetActive(true);
                    player.StartCoroutine(DisableAfterDelay(glowObj, effectDuration));
                }

                Destroy(gameObject); // 아이템 사라지게
            }
        }
    }

    private System.Collections.IEnumerator DisableAfterDelay(GameObject obj, float delay)
    {
        yield return new WaitForSeconds(delay);
        obj.SetActive(false);
    }
}