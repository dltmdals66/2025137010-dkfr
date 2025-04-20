using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayrMovement : MonoBehaviour
{
    public float moveSpeed = 5f; // �����̴� �ӵ�
    private Rigidbody2D rb;
    private Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        // �¿� �̵� �Է� �ޱ� (-1: ����, 1: ������, 0: ����)
        float horizontalInput = Input.GetAxisRaw("Horizontal");

        // �̵� ���⿡ ���� �ӵ� ����
        Vector2 movement = new Vector2(horizontalInput, 0f) * moveSpeed;
        rb.velocity = movement;

        // �����̴� ������ Ȯ���ؼ� �ִϸ��̼� ���� ����
        if (horizontalInput != 0)
        {
            animator.SetBool("IsWalking", true); // Animator�� "IsWalking" ������ true�� ����
        }
        else
        {
            animator.SetBool("IsWalking", false); // Animator�� "IsWalking" ������ false�� ����
        }

        // (���� ����) ĳ���� ���� ������
        if (horizontalInput > 0)
        {
            GetComponent<SpriteRenderer>().flipX = false; // ���������� �̵� �� ���� ����
        }
        else if (horizontalInput < 0)
        {
            GetComponent<SpriteRenderer>().flipX = true;  // �������� �̵� �� �¿� ����
        }

    }
}
