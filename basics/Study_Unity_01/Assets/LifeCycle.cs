using UnityEngine;

public class LifeCycle : MonoBehaviour
{
    // 게임 오브젝트 생성 시 가장 먼저 호출되는 함수
    void Awake()
    {
        Debug.Log("플레이어 오브젝트 호출");
    }

    // 게임 오브젝트가 활성화 될 때 호출되는 함수
    private void OnEnable()
    {
        Debug.Log("플레이어 오브젝트 활성화");
    }

    // 첫 번째 Update 함수가 호출되기 직전에 호출되는 함수
    void Start()
    {
        Debug.Log("플레이어 오브젝트 준비완료");
    }

    // 물리 연산이 필요한 프레임마다 호출되는 함수
    private void FixedUpdate()
    {
        Debug.Log("플레이어 오브젝트 물리연산");
    }


    // 게임 로직 업데이트가 필요한 프레임마다 호출되는 함수
    void Update()
    {
        Debug.Log("플레이어 오브젝트 업데이트");
    }

    // 모든 Update 함수가 호출된 후에 호출되는 함수
    void LateUpdate()
    {
        Debug.Log("플레이어 오브젝트 렌더링 직전 처리");
    }

    // 게임 오브젝트가 비활성화 될 때 호출되는 함수
    private void OnDisable()
    {
        Debug.Log("플레이어 오브젝트 비활성화");
    }

    // 게임 오브젝트가 비활성화 될 때 호출되는 함수
    void OnDestroy()
    {
        Debug.Log("플레이어 오브젝트 제거");
    }
}
