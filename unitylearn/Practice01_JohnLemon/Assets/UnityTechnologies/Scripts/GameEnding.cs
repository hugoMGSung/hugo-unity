using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEnding : MonoBehaviour
{
    public float fadeDuration = 1f;  // 페이드아웃 간격
    public GameObject player;  // 트리거가 발생할 오브젝트
    public float displayImageDuration = 1f;
    public CanvasGroup exitBackgroundImageCanvasGroup;

    bool m_IsPlayerAtExit;
    float m_Timer;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (m_IsPlayerAtExit)  // 플레이어가 트리거 영역에 들어왔는지 확인
        {
            EndLevel();
        }
    }

    private void EndLevel()
    {
        m_Timer += Time.deltaTime;  // 경과 시간 누적

        exitBackgroundImageCanvasGroup.alpha = m_Timer / fadeDuration;  // 페이드아웃 효과 적용

        if (m_Timer > fadeDuration + displayImageDuration)
        {
            Application.Quit();  // 게임 종료
        }
    }

    // 이 클래스는 먼저 플레이어가 제어하는 게임 오브젝트를 감지해야 함
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == player)  // 트리거 영역에 플레이어가 들어왔는지 확인
        {
            m_IsPlayerAtExit = true; // 트리거 상태 설정
        }
    }
}
