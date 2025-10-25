using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameEnding : MonoBehaviour
{
    public float fadeDuration = 1f;  // 페이드아웃 간격
    public float displayImageDuration = 1f;   // 이미지 표시 시간
    public GameObject player;  // 트리거가 발생할 오브젝트

    public CanvasGroup exitBackgroundImageCanvasGroup;   // 페이드아웃에 사용할 캔버스 그룹
    public CanvasGroup caughtBackgroundImageCanvasGroup;    // 잡혔을 때 사용할 캔버스 그룹


    public AudioSource exitAudio;
    public AudioSource caughtAudio;

    bool m_IsPlayerAtExit;  // 플레이어가 트리거 영역에 있는지 여부
    bool m_IsPlayerCaught;  // 플레이어가 잡혔는지 여부
    float m_Timer;    // 경과 시간

    bool m_HasAudioPlayed;

    // 이 클래스는 먼저 플레이어가 제어하는 게임 오브젝트를 감지해야 함
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == player)  // 트리거 영역에 플레이어가 들어왔는지 확인
        {
            m_IsPlayerAtExit = true; // 트리거 상태 설정
        }
    }

    // 플레이어가 잡혔을 때 호출되는 메서드
    public void CaughtPlayer()
    {
        m_IsPlayerCaught = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (m_IsPlayerAtExit)  // 플레이어가 트리거 영역에 들어왔는지 확인
        {
            EndLevel(exitBackgroundImageCanvasGroup, false, exitAudio);
        }
        else if (m_IsPlayerCaught)  // 플레이어가 잡혔는지 확인
        {
            EndLevel(caughtBackgroundImageCanvasGroup, true, caughtAudio);
        }
    }

    // 레벨 종료 처리
    void EndLevel(CanvasGroup imageCanvasGroup, bool doRestart, AudioSource audioSource)
    {
        if (!m_HasAudioPlayed)
        {
            audioSource.Play();
            m_HasAudioPlayed = true;
        }  // 오디오 추가

        m_Timer += Time.deltaTime;  // 경과 시간 누적
        imageCanvasGroup.alpha = m_Timer / fadeDuration;  // 페이드아웃 효과 적용

        if (m_Timer > fadeDuration + displayImageDuration) // 지정된 시간이 지나면
        {
            if (doRestart)  // 재시작 여부에 따라
            {
                SceneManager.LoadScene(0); // 첫 번째 씬(인덱스 0)으로 재시작
            }
            else
            {
                Application.Quit();  // 애플리케이션 종료
            }
        }
    }

    
}
