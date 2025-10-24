using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class WaypointPatrol : MonoBehaviour
{
    public NavMeshAgent navMeshAgent;    // NavMeshAgent 참조
    public Transform[] waypoints;        // 순찰 웨이포인트들

    int m_CurrentWaypointIndex = 0;

    void Awake()
    {
        // 인스펙터에서 비워두면 자동 할당
        if (navMeshAgent == null)
            navMeshAgent = GetComponent<NavMeshAgent>();
    }

    IEnumerator Start()
    {
        // 웨이포인트 없으면 더 진행하지 않음
        if (waypoints == null || waypoints.Length == 0)
        {
            Debug.LogWarning("[WaypointPatrol] waypoints 가 비어있어요.");
            yield break;
        }

        // NavMesh 위에 안전하게 올려놓기 (오프메시에 스폰되는 경우 대비)
        if (!navMeshAgent.isOnNavMesh)
        {
            if (NavMesh.SamplePosition(transform.position, out NavMeshHit hit, 2f, NavMesh.AllAreas))
            {
                // 현재 위치 근처의 NavMesh 지점으로 워프
                navMeshAgent.Warp(hit.position);
            }
            else
            {
                Debug.LogError("[WaypointPatrol] 현재 위치 근처에 NavMesh가 없어요. NavMesh를 Bake했는지/Agent가 닿아있는지 확인!");
                yield break;
            }
        }

        // 첫 목적지 설정 (웨이포인트 위치도 NavMesh에 스냅)
        SetDestinationSafe(waypoints[m_CurrentWaypointIndex].position);

        // 첫 경로 계산 완료될 때까지 한 프레임 이상 대기
        while (navMeshAgent.pathPending)
            yield return null;
    }

    void Update()
    {
        // 에이전트 상태 체크: NavMesh 위가 아니면 접근 금지
        if (navMeshAgent == null || !navMeshAgent.enabled || !navMeshAgent.isOnNavMesh)
            return;

        // 경로 계산 중이면 아직 remainingDistance 보지 않음
        if (navMeshAgent.pathPending)
            return;

        // 도착 판정: 남은 거리 <= 정지거리 && (경로 없음 or 거의 정지)
        if (navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance &&
            (!navMeshAgent.hasPath || navMeshAgent.velocity.sqrMagnitude < 0.01f))
        {
            m_CurrentWaypointIndex = (m_CurrentWaypointIndex + 1) % waypoints.Length;
            SetDestinationSafe(waypoints[m_CurrentWaypointIndex].position);
        }
    }

    void SetDestinationSafe(Vector3 targetPos)
    {
        // 웨이포인트가 메쉬 밖에 있으면 가장 가까운 NavMesh 점으로 보정
        if (NavMesh.SamplePosition(targetPos, out NavMeshHit hit, 2f, NavMesh.AllAreas))
        {
            navMeshAgent.SetDestination(hit.position);
        }
        else
        {
            Debug.LogWarning($"[WaypointPatrol] 웨이포인트 {m_CurrentWaypointIndex}가 NavMesh 바깥이에요. 반경 2m 내 대체 지점이 없음.");
        }
    }
}
