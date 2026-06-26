using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    // 플레이어 오브젝트 참조
    public GameObject player;

    // 카메라와 플레이어 사이의 거리(오프셋)
    private Vector3 offset;

    // 게임 시작 시 한 번 호출
    void Start()
    {
        // 현재 카메라 위치와 플레이어 위치의 차이를 계산
        // 이 값을 계속 유지하여 카메라가 플레이어를 따라다니게 함
        offset = transform.position - player.transform.position;
    }

    // 모든 Update가 끝난 후 호출됨
    // 카메라 추적은 보통 LateUpdate에서 처리
    void LateUpdate()
    {
        // 플레이어 위치에 초기 오프셋을 더하여
        // 항상 같은 거리에서 플레이어를 따라가도록 설정
        transform.position = player.transform.position + offset;
    }
}