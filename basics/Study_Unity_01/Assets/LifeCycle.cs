using UnityEngine;

public class LifeCycle : MonoBehaviour
{
    // ���� ������Ʈ ���� �� ���� ���� ȣ��Ǵ� �Լ�
    void Awake()
    {
        Debug.Log("�÷��̾� ������Ʈ ȣ��");
    }

    // ���� ������Ʈ�� Ȱ��ȭ �� �� ȣ��Ǵ� �Լ�
    private void OnEnable()
    {
        Debug.Log("�÷��̾� ������Ʈ Ȱ��ȭ");
    }

    // ù ��° Update �Լ��� ȣ��Ǳ� ������ ȣ��Ǵ� �Լ�
    void Start()
    {
        Debug.Log("�÷��̾� ������Ʈ �غ�Ϸ�");
    }

    // ���� ������ �ʿ��� �����Ӹ��� ȣ��Ǵ� �Լ�
    private void FixedUpdate()
    {
        Debug.Log("�÷��̾� ������Ʈ ��������");
    }


    // ���� ���� ������Ʈ�� �ʿ��� �����Ӹ��� ȣ��Ǵ� �Լ�
    void Update()
    {
        Debug.Log("�÷��̾� ������Ʈ ������Ʈ");
    }

    // ��� Update �Լ��� ȣ��� �Ŀ� ȣ��Ǵ� �Լ�
    void LateUpdate()
    {
        Debug.Log("�÷��̾� ������Ʈ ������ ���� ó��");
    }

    // ���� ������Ʈ�� ��Ȱ��ȭ �� �� ȣ��Ǵ� �Լ�
    private void OnDisable()
    {
        Debug.Log("�÷��̾� ������Ʈ ��Ȱ��ȭ");
    }

    // ���� ������Ʈ�� ��Ȱ��ȭ �� �� ȣ��Ǵ� �Լ�
    void OnDestroy()
    {
        Debug.Log("�÷��̾� ������Ʈ ����");
    }
}
