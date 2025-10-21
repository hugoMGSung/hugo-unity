using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class WaypointPatrol : MonoBehaviour
{
    public NavMeshAgent navMeshAgent;   // NavMeshAgent 게임 오브젝트 참조
    public Transform[] waypoints;       // 순찰할 웨이포인트 배열

    int m_CurrentWaypointIndex;         // 현재 웨이포인트 인덱스

    // 초기화 함수
    void Start()
    {
        // 첫 번째 웨이포인트로 이동 시작
        navMeshAgent.SetDestination(waypoints[0].position);
    }

    // 매 프레임 호출되는 업데이트 함수
    void Update()
    {
        // 현재 웨이포인트에 도착했는지 확인
        if (navMeshAgent.remainingDistance < navMeshAgent.stoppingDistance)
        {
            // 다음 웨이포인트로 이동
            m_CurrentWaypointIndex = (m_CurrentWaypointIndex + 1) % waypoints.Length;
            // 다음 웨이포인트로 목적지 설정
            navMeshAgent.SetDestination(waypoints[m_CurrentWaypointIndex].position);
        }
    }
}
