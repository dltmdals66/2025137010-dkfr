using UnityEngine;

public class JumpBoostItem1 : MonoBehaviour
{
    public float boostAmount = 3f;        // 점프력 증가량
    public int extraJumps = 2;            // 최대 점프 횟수 증가

    public GameObject crownEffectPrefab;  // 왕관 임팩트 프리팹 (필요시 새로 생성용)

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerComponent pc = other.GetComponent<playerComponent>();
            if (pc != null)
            {
                // 점프 능력 증가
                pc.jumpForce += boostAmount;
                pc.maxJumpCount = extraJumps;

                // 왕관 임팩트 켜기 또는 생성
                if (pc.currentCrownEffect != null)
                {
                    pc.currentCrownEffect.SetActive(true);
                }
                else if (crownEffectPrefab != null)
                {
                    pc.currentCrownEffect = Instantiate(crownEffectPrefab, pc.transform);
                    pc.currentCrownEffect.transform.localPosition = new Vector3(0, 1f, 0);
                }

                // 아이템 제거
                Destroy(gameObject);
            }
        }
    }
}