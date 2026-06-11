using System;
using UnityEngine;

public class Collactible : MonoBehaviour
{
    public float rotateSpeed = 0.7f;

    public GameObject onCollectEffect;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0, rotateSpeed, 0);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // 충돌한 상대의 태그가 Player인지 확인
        {
            // Destroy the collectible
            Destroy(gameObject);

            // 파티클 이펙트 예시화
            Instantiate(onCollectEffect, transform.position, transform.rotation);

        }
    }
    
}
