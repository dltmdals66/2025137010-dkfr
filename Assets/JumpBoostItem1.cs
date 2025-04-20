using UnityEngine;

public class JumpBoostItem1 : MonoBehaviour
{
    public float boostAmount = 3f;        // ������ ������
    public int extraJumps = 2;            // �ִ� ���� Ƚ�� ����

    public GameObject crownEffectPrefab;  // �հ� ����Ʈ ������ (�ʿ�� ���� ������)

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerComponent pc = other.GetComponent<playerComponent>();
            if (pc != null)
            {
                // ���� �ɷ� ����
                pc.jumpForce += boostAmount;
                pc.maxJumpCount = extraJumps;

                // �հ� ����Ʈ �ѱ� �Ǵ� ����
                if (pc.currentCrownEffect != null)
                {
                    pc.currentCrownEffect.SetActive(true);
                }
                else if (crownEffectPrefab != null)
                {
                    pc.currentCrownEffect = Instantiate(crownEffectPrefab, pc.transform);
                    pc.currentCrownEffect.transform.localPosition = new Vector3(0, 1f, 0);
                }

                // ������ ����
                Destroy(gameObject);
            }
        }
    }
}