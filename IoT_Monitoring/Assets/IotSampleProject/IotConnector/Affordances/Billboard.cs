using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Billboard : MonoBehaviour
{
    // 카메라 참조 변수 (Inspector에서 지정 가능)
    public Camera mainCamera;

    private void Start()
    {
        // mainCamera가 지정되지 않았다면, 씬의 기본 카메라(Camera.main)를 가져옴
        if (mainCamera == null)
        {
            mainCamera = Camera.main;
        }
    }

    private void Update()
    {
        // 오브젝트가 항상 카메라를 바라보도록 설정
        // transform.position + mainCamera.transform.rotation * Vector3.forward :
        // 카메라가 바라보는 방향을 기준으로 오브젝트가 회전하도록 계산
        // Vector3.up : 오브젝트의 위쪽 방향을 월드 좌표계의 위쪽(Y축)으로 고정
        transform.LookAt(transform.position + mainCamera.transform.rotation * Vector3.forward, Vector3.up);
    }
}
