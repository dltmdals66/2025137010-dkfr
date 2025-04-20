using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayrMovement : MonoBehaviour
{
    public float moveSpeed = 5f; // 움직이는 속도
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
        // 좌우 이동 입력 받기 (-1: 왼쪽, 1: 오른쪽, 0: 멈춤)
        float horizontalInput = Input.GetAxisRaw("Horizontal");

        // 이동 방향에 따라 속도 설정
        Vector2 movement = new Vector2(horizontalInput, 0f) * moveSpeed;
        rb.velocity = movement;

        // 움직이는 중인지 확인해서 애니메이션 상태 변경
        if (horizontalInput != 0)
        {
            animator.SetBool("IsWalking", true); // Animator의 "IsWalking" 변수를 true로 설정
        }
        else
        {
            animator.SetBool("IsWalking", false); // Animator의 "IsWalking" 변수를 false로 설정
        }

        // (선택 사항) 캐릭터 방향 뒤집기
        if (horizontalInput > 0)
        {
            GetComponent<SpriteRenderer>().flipX = false; // 오른쪽으로 이동 시 원래 방향
        }
        else if (horizontalInput < 0)
        {
            GetComponent<SpriteRenderer>().flipX = true;  // 왼쪽으로 이동 시 좌우 반전
        }

    }
}
