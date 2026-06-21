using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

public class CameraController : MonoBehaviour
{
    // 카메라가 바라볼 대상(예: 캐릭터, 오브젝트)
    public Transform target;

    // 카메라와 대상 사이의 기본 거리
    public float distance = 5.0f;
    // 줌 속도
    public float zoomSpeed = 4f;
    // 최소 줌 거리
    public float minZoom = 5f;
    // 최대 줌 거리
    public float maxZoom = 15f;
    // 마우스 X축 회전 속도
    public float xSpeed = 120.0f;
    // 마우스 Y축 회전 속도
    public float ySpeed = 120.0f;
    // 카메라가 아래로 회전할 수 있는 최소 각도
    public float yMinLimit = -20f;
    // 카메라가 위로 회전할 수 있는 최대 각도
    public float yMaxLimit = 80f;

    // 카메라 초기 위치와 회전을 저장
    private Vector3 originalPos;
    private Quaternion originalRot;

    // 현재 카메라 회전 각도 (x: 좌우, y: 상하)
    float x = 0.0f;
    float y = 0.0f;

    void Start()
    {
        // 시작 시 카메라의 현재 회전 각도를 가져와 초기화
        Vector3 angles = transform.eulerAngles;
        x = angles.y;
        y = angles.x;

        // 카메라의 초기 위치와 회전 저장
        originalPos = transform.position;
        originalRot = transform.rotation;
    }

    void LateUpdate()
    {
        // 대상이 존재할 때만 카메라 제어
        if (target)
        {
            // 마우스가 UI 요소 위에 있을 경우 카메라 제어를 막음
            if (EventSystem.current.IsPointerOverGameObject())
            {
                return;
            }

            // 마우스 왼쪽 버튼을 누르고 있을 때 카메라 회전
            if (Input.GetMouseButton(0))
            {
                // 마우스 이동에 따라 카메라 회전 각도 변경
                x += Input.GetAxis("Mouse X") * xSpeed * 0.02f;
                y -= Input.GetAxis("Mouse Y") * ySpeed * 0.02f;

                // y축 회전 각도를 제한 (위/아래로 너무 많이 회전하지 않도록)
                y = ClampAngle(y, yMinLimit, yMaxLimit);
            }

            // 마우스 휠로 줌 조정 (버튼 누름 여부와 상관없이)
            distance = Mathf.Clamp(distance - Input.GetAxis("Mouse ScrollWheel") * zoomSpeed, minZoom, maxZoom);

            // 새로운 회전과 위치 계산
            Quaternion rotation = Quaternion.Euler(y, x, 0);
            Vector3 position = rotation * new Vector3(0.0f, 0.0f, -distance) + target.position;

            // 카메라에 적용
            transform.rotation = rotation;
            transform.position = position;
        }
    }

    // 각도를 제한하는 함수
    static float ClampAngle(float angle, float min, float max)
    {
        if (angle < -360F)
        {
            angle += 360F;
        }
        if (angle > 360F)
        {
            angle -= 360F;
        }
        return Mathf.Clamp(angle, min, max);
    }

    // 카메라를 초기 위치와 회전으로 리셋하는 함수
    public void ResetCameraPosition()
    {
        transform.position = originalPos;
        transform.rotation = originalRot;
        distance = maxZoom; // 줌도 초기화
    }
}
