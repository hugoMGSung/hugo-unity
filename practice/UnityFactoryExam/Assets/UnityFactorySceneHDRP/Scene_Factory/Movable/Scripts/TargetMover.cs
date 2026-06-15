using UnityEngine;

public class TargetMover : MonoBehaviour
{
    [SerializeField] private float speed = 1f;
    [SerializeField] private float range = 0.3f;

    [SerializeField] private float fixedY = 0.828f;

    private Vector3 startPosition;

    private void Start()
    {
        startPosition = transform.position;

        // 시작 높이도 고정
        startPosition.y = fixedY;
    }

    private void Update()
    {
        // -range ~ +range 사이만 이동
        float x = Mathf.Sin(Time.time * speed) * range;

        transform.position = new Vector3(
            startPosition.x + x,
            fixedY,
            startPosition.z
        );
    }
}
