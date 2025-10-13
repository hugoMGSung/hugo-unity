using UnityEngine;

public class BigBallMove : MonoBehaviour
{
    Rigidbody rb;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        //rb.linearVelocity = Vector3.right;  // 오른쪽으로 속도 설정
        //rb.AddForce(Vector3.right * 1000, ForceMode.Impulse); // 오른쪽으로 힘 추가
        // 현재 BigBall의 Rigidbody Mass가 3000이므로, 1000의 힘을 주면 가속도가 1000/3000 = 0.3333이 됨
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // 매 프레임마다 입력값으로 화면의 동서남북으로 움직이게 함
        Vector3 dir = new Vector3(Input.GetAxisRaw("Horizontal"), 
                                  0,
                                  Input.GetAxisRaw("Vertical"));

        rb.AddForce(dir, ForceMode.Impulse); // 힘 추가

        // 회전력
        rb.AddTorque(dir, ForceMode.Impulse);  // 이것도 알아둬야
    }
}
