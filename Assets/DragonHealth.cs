using UnityEngine;
using UnityEngine.UI;

public class DragonHealth : MonoBehaviour
{
    public int maxHealth = 5;
    private int currentHealth;

    public Image[] healthOrbs; // ���� �׸��
    public Color damagedColor = Color.white; // �¾��� �� ��
    public Color normalColor = Color.red;    // ���� ��

    private void Start()
    {
        currentHealth = maxHealth;
        UpdateHealthUI();
    }

    public void TakeDamage()
    {
        if (currentHealth <= 0) return;

        currentHealth--;
        UpdateHealthUI();

        if (currentHealth <= 0)
        {
            Debug.Log("�巡�� ���");
            // �巡�� �״� ó�� �߰� ����
        }
    }

    void UpdateHealthUI()
    {
        for (int i = 0; i < healthOrbs.Length; i++)
        {
            if (i < currentHealth)
                healthOrbs[i].color = normalColor;  // ���� ü�� ���� �� ����
            else
                healthOrbs[i].color = damagedColor; // ���� ü�� �� ���
        }
    }
}