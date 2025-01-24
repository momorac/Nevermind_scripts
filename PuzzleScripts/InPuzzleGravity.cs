using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InPuzzleGravity : MonoBehaviour
{
    public float gravity = 9.81f;  // 중력 값
    private Rigidbody rb;

    void Start()
    {
        // Rigidbody 컴포넌트를 가져옴
        rb = GetComponent<Rigidbody>();

        // 중력 적용을 위해 Rigidbody의 기본 중력을 비활성화
        rb.useGravity = false;
    }

    void Update()
    {
        // 수동으로 중력 적용 (Rigidbody의 속도에 중력 가속도 추가)
        rb.velocity += Vector3.down * gravity * Time.deltaTime;
    }

    private void OnCollisionEnter(Collision collision)
    {
        // 충돌할 때, 땅에 닿았는지 확인 (태그로 확인)
        if (collision.gameObject.CompareTag("Ground"))
        {
            // 충돌 시 속도를 0으로 리셋 (땅에 착지 시)
            rb.velocity = Vector3.zero;
        }
    }
}
