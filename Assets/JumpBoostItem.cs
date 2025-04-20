using System.Collections;
using UnityEngine;

public class JumpBoostItem : MonoBehaviour
{
    // �÷��̾� �ȿ� �ִ� ����Ʈ ������Ʈ�� ��������� �� (��Ȱ��ȭ ���¿��� ��)
    public GameObject impactEffect;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // impactEffect�� �������� �ʰ� �ڵ����� �÷��̾� �ڽĿ��� ã�� ���� ��:
            if (impactEffect == null)
            {
                Transform found = other.transform.Find("ImpactEffect");
                if (found != null)
                {
                    impactEffect = found.gameObject;
                }
            }

            // ����Ʈ ȿ�� �����ֱ�
            if (impactEffect != null)
            {
                StartCoroutine(ShowImpactEffect());
            }

            // (����) ���⼭ ���� �ɷ� ��� ���� ��� �߰� ����

            // ������ �������
            Destroy(gameObject);
        }
    }

    IEnumerator ShowImpactEffect()
    {
        impactEffect.SetActive(true);         // ����Ʈ �ѱ�
        yield return new WaitForSeconds(3f);  // 3�� ��ٸ�
        impactEffect.SetActive(false);        // ����Ʈ ����
    }
}