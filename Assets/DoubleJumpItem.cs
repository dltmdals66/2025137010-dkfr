using UnityEngine;

public class DoubleJumpShoes : MonoBehaviour
{
    public int extraJumps = 1;
    public float effectDuration = 10f;  // ����Ʈ ���� �ð��� 10�ʷ� ����

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerComponent player = other.GetComponent<playerComponent>();
            if (player != null)
            {
                player.maxJumpCount += extraJumps;

                // GlowEffect ã�� 10�� ���� Ȱ��ȭ
                Transform glow = other.transform.Find("GlowEffect");
                if (glow != null)
                {
                    GameObject glowObj = glow.gameObject;
                    glowObj.SetActive(true);
                    player.StartCoroutine(DisableAfterDelay(glowObj, effectDuration));
                }

                Destroy(gameObject); // ������ �������
            }
        }
    }

    private System.Collections.IEnumerator DisableAfterDelay(GameObject obj, float delay)
    {
        yield return new WaitForSeconds(delay);
        obj.SetActive(false);
    }
}