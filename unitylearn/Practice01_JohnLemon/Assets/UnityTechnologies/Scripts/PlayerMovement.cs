using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float turnSpeed = 2000f;  // 회전 속도

    Vector3 m_Movement;   // 캐릭터의 이동 방향 벡터
    Animator m_Animator;   // 애니메이터 연결용
    Rigidbody m_Rigidbody;  // 리지드바디 연결용
    Quaternion m_Rotation = Quaternion.identity; // 회전 값 저장용

    float horizontal;                // 좌우 입력
    float vertical;                  // 앞/뒤 입력

    // 업데이트 이전에 호출됩니다.
    void Start()
    {
        m_Animator = GetComponent<Animator>();   // 애니메이터 컴포넌트 가져오기
        m_Rigidbody = GetComponent<Rigidbody>(); // 리지드바디 컴포넌트 가져오기 
    }

    // 물리 업데이트 처리
    void FixedUpdate()
    { 
        horizontal = Input.GetAxis("Horizontal"); // 수평으로 입력 받기
        vertical = Input.GetAxis("Vertical"); // 수직으로 입력 받기   

        // 앞/뒤 이동 벡터만 계산 (좌우는 이동 X)
        if (vertical > 0f)
            m_Movement = transform.forward;        // 전진
        else if (vertical < 0f)
            m_Movement = -transform.forward;       // 후진(뒷걸음질)
        else
            m_Movement = Vector3.zero;             // 정지


        // 애니메이터 파라미터 (원하는 대로 바꾸세요)
        // - IsWalking: 전/후진 중일 때 true
        // - Speed: 전/후진 방향을 구분하려면 float 파라미터로 사용(Blend Tree에 유용)
        bool isMoving = !Mathf.Approximately(vertical, 0f);
        m_Animator.SetBool("IsWalking", isMoving);
        m_Animator.SetFloat("Speed", vertical); // 선택 사항(Blend Tree 쓰는 경우)
    }

    // 애니메이터 이동 처리
    void OnAnimatorMove()
    {
        // 리지드바디 위치 및 회전 갱신
        //m_Rigidbody.MovePosition(m_Rigidbody.position + m_Movement * m_Animator.deltaPosition.magnitude);
        //m_Rigidbody.MoveRotation(m_Rotation);

        // 1) 회전: 좌우 입력 만큼 제자리에서 회전만 수행
        if (!Mathf.Approximately(horizontal, 0f))
        {
            float turn = horizontal * turnSpeed * Time.deltaTime;
            Quaternion turnRot = Quaternion.Euler(0f, turn, 0f);
            m_Rigidbody.MoveRotation(m_Rigidbody.rotation * turnRot);
        }

        // 2) 이동: 앞/뒤 입력이 있을 때만 Root Motion 거리만큼 이동
        if (m_Movement != Vector3.zero)
        {
            // deltaPosition.magnitude = 이번 프레임 애니메이션이 제시하는 "이동량"
            Vector3 delta = m_Movement * m_Animator.deltaPosition.magnitude;
            m_Rigidbody.MovePosition(m_Rigidbody.position + delta);
        }
    }
}
