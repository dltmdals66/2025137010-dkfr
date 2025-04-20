using System.Collections;
using UnityEngine;

public class JumpBoostItem : MonoBehaviour
{
    // 플레이어 안에 있는 임펙트 오브젝트를 연결해줘야 해 (비활성화 상태여야 함)
    public GameObject impactEffect;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // impactEffect를 연결하지 않고 자동으로 플레이어 자식에서 찾고 싶을 때:
            if (impactEffect == null)
            {
                Transform found = other.transform.Find("ImpactEffect");
                if (found != null)
                {
                    impactEffect = found.gameObject;
                }
            }

            // 임펙트 효과 보여주기
            if (impactEffect != null)
            {
                StartCoroutine(ShowImpactEffect());
            }

            // (선택) 여기서 점프 능력 상승 같은 기능 추가 가능

            // 아이템 사라지기
            Destroy(gameObject);
        }
    }

    IEnumerator ShowImpactEffect()
    {
        impactEffect.SetActive(true);         // 임펙트 켜기
        yield return new WaitForSeconds(3f);  // 3초 기다림
        impactEffect.SetActive(false);        // 임펙트 끄기
    }
}