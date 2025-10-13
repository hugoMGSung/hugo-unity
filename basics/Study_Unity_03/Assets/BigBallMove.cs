using UnityEngine;

public class BigBallMove : MonoBehaviour
{
    Rigidbody rb;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        //rb.linearVelocity = Vector3.right;  // ���������� �ӵ� ����
        //rb.AddForce(Vector3.right * 1000, ForceMode.Impulse); // ���������� �� �߰�
        // ���� BigBall�� Rigidbody Mass�� 3000�̹Ƿ�, 1000�� ���� �ָ� ���ӵ��� 1000/3000 = 0.3333�� ��
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // �� �����Ӹ��� �Է°����� ȭ���� ������������ �����̰� ��
        Vector3 dir = new Vector3(Input.GetAxisRaw("Horizontal"), 
                                  0,
                                  Input.GetAxisRaw("Vertical"));

        rb.AddForce(dir, ForceMode.Impulse); // �� �߰�

        // ȸ����
        rb.AddTorque(dir, ForceMode.Impulse);  // �̰͵� �˾Ƶ־�
    }
}
