using UnityEngine;

public class RotateAtFrequency : MonoBehaviour
{
    [Tooltip("초당 회전 횟수(1초에 몇 바퀴 회전할지).")]
    public float frequency = 1.0f; // 기본값: 1초에 1회전

    [Tooltip("회전 축, 예: (0,1,0)은 Y축 기준 회전.")]
    public Vector3 rotationAxis = Vector3.up;

    void Update()
    {
        // 1회전 = 360도 → 360 * frequency = 초당 회전 각도
        float degreesPerSecond = 360f * frequency;

        // 지정된 축을 기준으로 매 프레임 회전
        // Time.deltaTime을 곱해 프레임 속도와 무관하게 일정한 회전 속도 유지
        transform.Rotate(rotationAxis.normalized, degreesPerSecond * Time.deltaTime);
    }
}
