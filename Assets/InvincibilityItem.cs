using UnityEngine;

public class InvincibilityItem : MonoBehaviour
{
    public float invincibilityDuration = 4f;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerComponent pc = other.GetComponent<playerComponent>();
            if (pc != null)
            {
                pc.StartCoroutine(pc.BecomeInvincible(invincibilityDuration));
                Destroy(gameObject);
            }
        }
    }
}