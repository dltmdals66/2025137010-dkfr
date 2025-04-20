using UnityEngine;
using UnityEngine.UI;

public class DragonHealth : MonoBehaviour
{
    public int maxHealth = 5;
    private int currentHealth;

    public Image[] healthOrbs; // 빨간 네모들
    public Color damagedColor = Color.white; // 맞았을 때 색
    public Color normalColor = Color.red;    // 원래 색

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
            Debug.Log("드래곤 사망");
            // 드래곤 죽는 처리 추가 가능
        }
    }

    void UpdateHealthUI()
    {
        for (int i = 0; i < healthOrbs.Length; i++)
        {
            if (i < currentHealth)
                healthOrbs[i].color = normalColor;  // 아직 체력 있음 → 빨강
            else
                healthOrbs[i].color = damagedColor; // 깎인 체력 → 흰색
        }
    }
}