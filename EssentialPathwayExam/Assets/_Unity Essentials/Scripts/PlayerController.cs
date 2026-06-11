using System.Collections.Specialized;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// WASD 또는 방향키를 사용하여
/// 전진/후진 및 좌우 회전을 처리하는 플레이어 컨트롤러
/// </summary>
public class PlayerController : MonoBehaviour
{
    [Tooltip("전진/후진 이동 속도 (초당 이동 거리)")]
    public float speed = 5.0f;

    [Tooltip("회전 속도 (초당 회전 각도)")]
    public float rotationSpeed = 120.0f;

    public float jumpForce = 5.0f;

    // Rigidbody 컴포넌트 참조
    private Rigidbody rb;

    private void Start()
    {
        // 현재 게임오브젝트의 Rigidbody 가져오기
        rb = GetComponent<Rigidbody>();

        // Rigidbody가 없으면 경고 출력
        if (rb == null)
            System.Diagnostics.Debug.WriteLine("PlayerController에는 Rigidbody가 필요합니다.");
    }

    // 매 프레임 실행
    private void Update()
    {
        // 입력 처리는 FixedUpdate에서 수행하므로
        // Update에서는 별도의 작업이 필요 없습니다.
        if (Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.VelocityChange);
        }
    }

    // 물리엔진 전용 함수
    private void FixedUpdate()
    {
        // 입력값 저장용 변수
        // x : 좌우 회전
        // y : 전진/후진
        Vector2 moveInput = Vector2.zero;

        // -----------------------------
        // 전진 입력 (W 또는 ↑)
        // -----------------------------
        if (Keyboard.current.wKey.isPressed ||
            Keyboard.current.upArrowKey.isPressed)
        {
            moveInput.y = 1f;
        }

        // -----------------------------
        // 후진 입력 (S 또는 ↓)
        // -----------------------------
        if (Keyboard.current.sKey.isPressed ||
            Keyboard.current.downArrowKey.isPressed)
        {
            moveInput.y = -1f;
        }

        // -----------------------------
        // 좌회전 입력 (A 또는 ←)
        // -----------------------------
        if (Keyboard.current.aKey.isPressed ||
            Keyboard.current.leftArrowKey.isPressed)
        {
            moveInput.x = -1f;
        }

        // -----------------------------
        // 우회전 입력 (D 또는 →)
        // -----------------------------
        if (Keyboard.current.dKey.isPressed ||
            Keyboard.current.rightArrowKey.isPressed)
        {
            moveInput.x = 1f;
        }

        // ==================================================
        // 이동 처리
        // ==================================================
        // 현재 바라보는 방향(transform.forward)으로
        // 전진 또는 후진 이동
        Vector3 movement =
            transform.forward *
            moveInput.y *
            speed *
            Time.fixedDeltaTime;

        // Rigidbody를 이용하여 위치 이동
        rb.MovePosition(rb.position + movement);

        // ==================================================
        // 회전 처리
        // ==================================================

        // 기본 회전 방향
        float turnDirection = moveInput.x;

        // 후진 중에는 자동차처럼 회전 방향을 반대로 변경
        if (moveInput.y < 0)
            turnDirection = -turnDirection;

        // 이번 프레임에 회전할 각도 계산
        float turn =
            turnDirection *
            rotationSpeed *
            Time.fixedDeltaTime;

        // Y축 기준 회전값 생성
        Quaternion turnRotation =
            Quaternion.Euler(0f, turn, 0f);

        // Rigidbody 회전 적용
        rb.MoveRotation(rb.rotation * turnRotation);
    }
}