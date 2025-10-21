using UnityEngine;

public class Observer : MonoBehaviour
{
    public Transform player;  // 플레이어의 Transform 참조
    public GameEnding gameEnding; // GameEnding 스크립트 참조

    bool m_IsPlayerInRange;   // 플레이어가 트리거 영역에 있는지 여부

    void OnTriggerEnter(Collider other)
    {
        if (other.transform == player)
        {
            m_IsPlayerInRange = true;   // 플레이어가 트리거 영역에 들어옴
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.transform == player)
        {
            m_IsPlayerInRange = false;      //  플레이어가 트리거 영역에서 나감
        }
    }

    void Update()
    {
        if (m_IsPlayerInRange)  // 플레이어가 트리거 영역에 있을 때
        {
            // 플레이어와 관찰자 사이에 장애물이 있는지 확인
            Vector3 direction = player.position - transform.position + Vector3.up;
            Ray ray = new Ray(transform.position, direction);
            RaycastHit raycastHit;

            // 레이캐스트가 플레이어에 닿는지 확인
            if (Physics.Raycast(ray, out raycastHit))
            {
                if (raycastHit.collider.transform == player) // 플레이어가 보임
                {
                    gameEnding.CaughtPlayer();
                }
            }
        }
    }
}
