using UnityEngine;

public class KeyInterface : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.anyKeyDown)
        {
            Debug.Log("아무 키가 눌려졌습니다.");
        }

        // Time.deltaTime 은 컴퓨터 성능에 따라 움직임이 달라지는 것을 방지하기 위함
        // 💡 변경: Input.GetAxisRaw("Vertical")을 두 번째(Y축)에서 세 번째(Z축) 성분으로 이동
        Vector3 vec = new Vector3(
            Input.GetAxisRaw("Horizontal") * Time.deltaTime,
            0, // Y축 성분을 0으로 설정
            Input.GetAxisRaw("Vertical") * Time.deltaTime // Z축 성분에 Vertical 입력 사용
        );

        transform.Translate(vec);
    }
}
