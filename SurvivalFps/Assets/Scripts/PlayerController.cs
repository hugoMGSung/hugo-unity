using System;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private float walkSpeed = 3.0f;  // private�� SerializeField�� ���̸� �ν����Ϳ��� ���� ����


    private Rigidbody myRigid;  // ���� �÷��̾��� ��ü�� ����
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        myRigid = GetComponent<Rigidbody>();  // �÷��̾��� Rigidbody�� ������
    }

    

    // Update is called once per frame
    void Update()
    {
        Move();
    }

    private void Move()
    {
        float _moveDirX = Input.GetAxisRaw("Horizontal");  // �¿� ����Ű �Է�
        float _moveDirZ = Input.GetAxisRaw("Vertical");  // ���� ����Ű �Է�

        Vector3 _moveHorizontal = transform.right * _moveDirX;  // �÷��̾��� ������ �������� �̵�
        Vector3 _moveVertical = transform.forward * _moveDirZ;  // �÷��̾��� ���� �������� �̵�

        // �ջ�� ���� 1�� ������ִ� �� normalized
        Vector3 _velocity = (_moveHorizontal + _moveVertical).normalized * walkSpeed;  // �̵� ����� �ӵ��� ���
        myRigid.MovePosition(_velocity * Time.fixedDeltaTime + myRigid.position);  // Rigidbody�� �̿��� �̵�
    }
}
