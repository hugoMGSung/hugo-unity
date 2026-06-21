using UnityEngine;
using UnityEngine.AI;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(NavMeshAgent))]
public class PathWalker : MonoBehaviour
{
    // 이동할 웨이포인트(목적지)들을 담는 리스트
    public List<Transform> waypoints = new List<Transform>();
    // NavMeshAgent 컴포넌트 (Unity 내비게이션 시스템)
    private NavMeshAgent agent;
    // 현재 목적지 인덱스
    private int currentDestinationIndex = -1;
    // 웨이포인트를 반복할지 여부
    public bool loop = true;
    // 각 웨이포인트에서 대기할 시간
    public float waitTimeAtWaypoint = 1.0f;

    void Start()
    {
        // NavMeshAgent 컴포넌트 가져오기
        agent = GetComponent<NavMeshAgent>();
        // 웨이포인트 이동 루틴 시작
        StartCoroutine(GoToNextPoint());
    }

    // 웨이포인트를 순차적으로 이동하는 코루틴
    private IEnumerator GoToNextPoint()
    {
        while (true) // 무한 루프 (조건에 따라 break)
        {
            // 웨이포인트가 없으면 종료
            if (waypoints.Count == 0)
                yield break;

            // 다음 목적지 인덱스 계산 (리스트 끝에 도달하면 다시 처음으로)
            currentDestinationIndex = (currentDestinationIndex + 1) % waypoints.Count;

            // 다음 웨이포인트 위치 가져오기
            Vector3 nextWaypointPosition = waypoints[currentDestinationIndex].position;
            // Y축은 현재 에이전트의 높이를 유지하고 X, Z만 웨이포인트 값 사용
            Vector3 nextPosition = new Vector3(
                nextWaypointPosition.x,
                agent.transform.position.y,
                nextWaypointPosition.z
            );

            // NavMeshAgent에 목적지 설정
            agent.SetDestination(nextPosition);

            // 에이전트가 목적지에 도착할 때까지 대기
            while (agent.pathPending || agent.remainingDistance > agent.stoppingDistance)
            {
                yield return null; // 다음 프레임까지 대기
            }

            // 웨이포인트에서 일정 시간 대기
            yield return new WaitForSeconds(waitTimeAtWaypoint);

            // 마지막 웨이포인트에 도달했고 loop가 false라면 종료
            if (currentDestinationIndex == waypoints.Count - 1 && !loop)
            {
                yield break;
            }
        }
    }
}
