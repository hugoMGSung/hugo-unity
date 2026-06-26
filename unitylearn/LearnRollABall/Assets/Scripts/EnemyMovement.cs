using UnityEngine;
using UnityEngine.AI;

public class EnemyMovement : MonoBehaviour
{
    // 플레이어의 위치(Transform)를 참조합니다.
    public Transform player;

    // 경로 탐색을 위한 NavMeshAgent 컴포넌트 참조입니다.
    private NavMeshAgent navMeshAgent;

    // Start는 첫 번째 프레임 업데이트 전에 호출됩니다.
    void Start()
    {
        // 이 오브젝트에 연결된 NavMeshAgent 컴포넌트를 가져와 저장합니다.
        navMeshAgent = GetComponent<NavMeshAgent>();
    }

    // Update는 프레임마다 한 번씩 호출됩니다.
    void Update()
    {
        // 플레이어 참조가 비어있지 않다면(존재한다면)...
        if (player != null)
        {
            // 적의 목적지를 플레이어의 현재 위치로 설정합니다.
            navMeshAgent.SetDestination(player.position);
        }
    }
}