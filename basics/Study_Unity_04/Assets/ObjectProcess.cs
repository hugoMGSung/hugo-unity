using UnityEngine;

public class ObjectProcess : MonoBehaviour
{
    int iValue;
    float fValue;
    bool bValue;
    string sValue;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        iValue = 100;
        fValue = 3.14f;
        bValue = true;
        sValue = "Hello, Unity!";
    }

    // Update is called once per frame
    void Update()
    {
        print($"iValue + {iValue}");
        print($"fValue + {fValue}");
        print($"bValue + {bValue}");
        print($"sValue + {sValue}");
    }
}
