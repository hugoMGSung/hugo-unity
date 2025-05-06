using System;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private float walkSpeed = 3.0f;  // private에 SerializeField을 붙이면 인스펙터에서 수정 가능


    private Rigidbody myRigid;  // 실제 플레이어의 육체를 뜻함
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        myRigid = GetComponent<Rigidbody>();  // 플레이어의 Rigidbody를 가져옴
    }

    

    // Update is called once per frame
    void Update()
    {
        Move();
    }

    private void Move()
    {
        float _moveDirX = Input.GetAxisRaw("Horizontal");  // 좌우 방향키 입력
        float _moveDirZ = Input.GetAxisRaw("Vertical");  // 상하 방향키 입력

        Vector3 _moveHorizontal = transform.right * _moveDirX;  // 플레이어의 오른쪽 방향으로 이동
        Vector3 _moveVertical = transform.forward * _moveDirZ;  // 플레이어의 앞쪽 방향으로 이동

        // 합산된 값이 1로 만들어주는 것 normalized
        Vector3 _velocity = (_moveHorizontal + _moveVertical).normalized * walkSpeed;  // 이동 방향과 속도를 계산
        myRigid.MovePosition(_velocity * Time.fixedDeltaTime + myRigid.position);  // Rigidbody를 이용해 이동
    }
}
