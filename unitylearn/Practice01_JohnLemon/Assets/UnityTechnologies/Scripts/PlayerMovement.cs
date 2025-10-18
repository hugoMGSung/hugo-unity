using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Searcher.SearcherWindow.Alignment;

public class PlayerMovement : MonoBehaviour
{
    public float turnSpeed = 20f;  // 회전 속도

    Vector3 m_Movement;   // 캐릭터의 이동 방향 벡터
    Animator m_Animator;   // 애니메이터 연결용
    Rigidbody m_Rigidbody;  // 리지드바디 연결용
    Quaternion m_Rotation = Quaternion.identity; // 회전 값 저장용


    // 업데이트 이전에 호출됩니다.
    void Start()
    {
        m_Animator = GetComponent<Animator>();   // 애니메이터 컴포넌트 가져오기
        m_Rigidbody = GetComponent<Rigidbody>(); // 리지드바디 컴포넌트 가져오기 
    }

    // 물리 업데이트 처리
    void FixedUpdate()
    { 
        float horizontal = Input.GetAxis("Horizontal"); // 수평으로 입력 받기
        float vertical = Input.GetAxis("Vertical"); // 수직으로 입력 받기   

        m_Movement.Set(horizontal, 0f, vertical); // 이동 벡터 설정
        m_Movement.Normalize(); // 벡터 정규화

        bool hasHorizontalInput = !Mathf.Approximately(horizontal, 0f); // 수평 입력이 있는지 여부 판단
        bool hasVerticalInput = !Mathf.Approximately(vertical, 0f); // 수직 축에도 입력이 있는지 판단
        bool isWalking = hasHorizontalInput || hasVerticalInput;    // 걷고 있는지 여부 판단

        m_Animator.SetBool("IsWalking", isWalking);  // 애니메이터에 걷고 있는지 여부 전달

        // 캐릭터 회전 처리
        Vector3 desiredForward = Vector3.RotateTowards(transform.forward, m_Movement, turnSpeed * Time.deltaTime, 0f);
        m_Rotation = Quaternion.LookRotation(desiredForward);
    }

    // 애니메이터 이동 처리
    void OnAnimatorMove()
    {
        // 리지드바디 위치 및 회전 갱신
        m_Rigidbody.MovePosition(m_Rigidbody.position + m_Movement * m_Animator.deltaPosition.magnitude);
        m_Rigidbody.MoveRotation(m_Rotation);
    }
}
