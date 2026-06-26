using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public TextMeshProUGUI countText;

    public GameObject winTextObject;

    // 플레이어의 Rigidbody 컴포넌트
    private Rigidbody rb;

    private int count;

    // 입력받은 이동값 (좌우, 앞뒤)
    private float movementX;
    private float movementY;

    // 플레이어 이동 속도
    public float speed = 0;

    // 게임 시작 시 한 번 호출
    void Start()
    {
        // 현재 오브젝트에 붙어있는 Rigidbody 컴포넌트 가져오기
        rb = GetComponent<Rigidbody>();
        count = 0; // 초기 카운트 값 설정
        winTextObject.SetActive(false);
    }

    void SetCountText()
    {
        countText.text = "Count: " + count.ToString();

        if (count >= 12)
        {
            winTextObject.SetActive(true);

            Destroy(GameObject.FindGameObjectWithTag("Enemy")); // 적 오브젝트 제거
        }
    }

    // 이동 입력이 발생할 때마다 호출됨
    void OnMove(InputValue movementValue)
    {
        // 입력값을 Vector2 형태로 가져옴
        // x = 좌우(A,D), y = 앞뒤(W,S)
        Vector2 movementVector = movementValue.Get<Vector2>();

        // 입력값 저장
        movementX = movementVector.x;
        movementY = movementVector.y;
    }

    // 물리 연산 주기마다 호출됨
    void FixedUpdate()
    {
        // 입력값을 이용하여 3차원 이동 벡터 생성
        // X축 = 좌우 이동
        // Z축 = 앞뒤 이동
        Vector3 movement = new Vector3(movementX, 0.0f, movementY);

        // Rigidbody에 힘을 가해 플레이어 이동
        rb.AddForce(movement * speed);
    }

    void OnTriggerEnter(Collider other)
    {
        // 플레이어가 충돌한 물체에 "PickUp" 태그가 있는지 확인합니다.
        if (other.gameObject.CompareTag("PickUp"))
        {
            // 충돌한 물체를 비활성화합니다(사라지게 합니다).
            other.gameObject.SetActive(false);
            count++; // 카운트 증가

            SetCountText();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            // 현재 오브젝트 제거
            Destroy(gameObject);

            // Update the winText to display "You Lose!"
            winTextObject.gameObject.SetActive(true);
            winTextObject.GetComponent<TextMeshProUGUI>().text = "You Lose!";

        }
    }
}