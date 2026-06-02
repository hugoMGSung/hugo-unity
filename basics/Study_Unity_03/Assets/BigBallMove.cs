using UnityEngine;

public class BigBallMove : MonoBehaviour
{
    Rigidbody rb;

    // 게임 시작 시 한번 실행
    void Start()
    {
        // 현재 오브젝트의 Rigidbody 컴포넌트 가져오기
        rb = GetComponent<Rigidbody>();

        // BigBall을 오른쪽 방향으로 이동시키는 속도 설정
        //rb.linearVelocity = Vector3.right;

        // BigBall에 오른쪽 방향으로 힘 추가
        //rb.AddForce(Vector3.right * 1000, ForceMode.Impulse);

        // 현재 BigBall의 Rigidbody Mass가 3000이므로
        // 1000의 힘을 주면 가속도는 1000 / 3000 = 0.3333 정도가 됨
    }

    // 물리 연산용 업데이트 함수
    // Rigidbody 관련 처리는 Update()보다 FixedUpdate() 사용 권장
    void FixedUpdate()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        Vector3 dir = Vector3.zero;

        // 좌우 입력이 있으면 좌우만 처리
        if (h != 0)
        {
            dir = new Vector3(h, 0, 0);
        }
        // 좌우 입력이 없을 때만 상하 처리
        else if (v != 0)
        {
            dir = new Vector3(0, 0, v);
        }

        // 힘 추가
        rb.AddForce(dir, ForceMode.Impulse);

        // 회전 추가
        rb.AddTorque(dir, ForceMode.Impulse);
    }
}