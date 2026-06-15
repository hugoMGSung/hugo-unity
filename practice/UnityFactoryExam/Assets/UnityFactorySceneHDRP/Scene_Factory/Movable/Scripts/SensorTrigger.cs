using UnityEngine;
using System.Collections;

public class SensorTrigger : MonoBehaviour
{
    public ConveyorBelt conveyor;

    public float StopDuration = 2.0f;

    private bool isProcessing = false;

    private void OnTriggerEnter(Collider other)
    {
        if (isProcessing) return;

        if (other.CompareTag("Product"))
        {
            StartCoroutine(Process(other));
        }
    }

    private IEnumerator Process(Collider product)
    {
        isProcessing = true;

        Debug.Log("제품 감지 - 컨베이어 정지");
        conveyor.Stop();

        yield return new WaitForSeconds(StopDuration);

        Debug.Log("컨베이어 재시작");
        conveyor.StartBelt();

        // 같은 박스가 센서에 남아있어도 바로 다시 감지하지 않도록 약간 대기
        yield return new WaitForSeconds(0.5f);

        isProcessing = false;
    }
}