using UnityEditor.Rendering;
using UnityEngine;

public class CharacterMove : MonoBehaviour
{
    [SerializeField] public float speed = 10.0f;        // 이동 속도
    [SerializeField] public float rotateSpeed = 120.0f; // 회전 속도
    [SerializeField] public float jumpSpeed = 8.0f;     // 점프힘
    [SerializeField] public float gravity = 20.0f;      // 중력 값. 점프할 때 사용

    private Vector3 moveDirection = Vector3.zero;       // 실제 이동 방향 벡터

    // step2. Animator 추가
    Animator anim;

    private void Start()
    {
        // step2. Animator 컴포넌트 가져오기
        anim = GetComponent<Animator>();
    }

    // 매 프레임마다 실행
    void Update()
    {
        // 현재 오브젝트의 CharacterController 컴포넌트 가져오기
        CharacterController controller = GetComponent<CharacterController>();

        // 좌우 입력값 받기 (A/D, ←/→)
        float h = Input.GetAxis("Horizontal");

        // 앞뒤 입력값 받기 (W/S, ↑/↓)
        float v = Input.GetAxis("Vertical");

        // 좌우 회전 처리
        transform.Rotate(0, h * rotateSpeed * Time.deltaTime, 0);

        // step2. 애니메이터에 이동값 전달
        anim.SetFloat("Speed", Mathf.Abs(v));

        // 캐릭터가 바닥에 닿아있을 때만 이동 입력 처리
        if (controller.isGrounded)
        {
            // 전후 이동 방향
            moveDirection = transform.forward * v;

            // 이동 속도 적용
            moveDirection *= speed;

            // 점프 버튼(Space) 입력 시
            if (Input.GetButton("Jump"))
                moveDirection.y = jumpSpeed;
        }

        // 중력 적용
        // Time.deltaTime
        // 컴퓨터 성능마다 속도가 달라지는 걸 막기 위해서 사용
        moveDirection.y -= gravity * Time.deltaTime;

        // 실제 이동 처리
        controller.Move(moveDirection * Time.deltaTime);
    }
}