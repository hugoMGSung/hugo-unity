using UnityEngine;

public class ObjectMove : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    Vector3 target = new Vector3(-0.001f, 0.99f, 9.38f);

    // Update is called once per frame
    void Update()
    {
        // 위치 이동
        //transform.position = Vector3.MoveTowards(transform.position, target, 0.2f);

        // 천천히 이동
        //Vector3 velocity = Vector3.zero;

        //transform.position = Vector3.SmoothDamp(transform.position, target, ref velocity, 0.1f);

        // 선형보간 이동
        //transform.position = Vector3.Lerp(transform.position, target, 0.1f);

        // 구면 선형보간 이동
        transform.position = Vector3.Slerp(transform.position, target, 0.1f);
    }
}
