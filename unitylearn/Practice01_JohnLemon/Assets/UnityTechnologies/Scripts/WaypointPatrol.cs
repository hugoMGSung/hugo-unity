using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class WaypointPatrol : MonoBehaviour
{
    public NavMeshAgent navMeshAgent;   // NavMeshAgent ���� ������Ʈ ����
    public Transform[] waypoints;       // ������ ��������Ʈ �迭

    int m_CurrentWaypointIndex;         // ���� ��������Ʈ �ε���

    // �ʱ�ȭ �Լ�
    void Start()
    {
        // ù ��° ��������Ʈ�� �̵� ����
        navMeshAgent.SetDestination(waypoints[0].position);
    }

    // �� ������ ȣ��Ǵ� ������Ʈ �Լ�
    void Update()
    {
        // ���� ��������Ʈ�� �����ߴ��� Ȯ��
        if (navMeshAgent.remainingDistance < navMeshAgent.stoppingDistance)
        {
            // ���� ��������Ʈ�� �̵�
            m_CurrentWaypointIndex = (m_CurrentWaypointIndex + 1) % waypoints.Length;
            // ���� ��������Ʈ�� ������ ����
            navMeshAgent.SetDestination(waypoints[m_CurrentWaypointIndex].position);
        }
    }
}
