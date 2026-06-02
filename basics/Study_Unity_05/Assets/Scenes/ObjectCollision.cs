using UnityEngine;

public class ObjectCollision : MonoBehaviour
{
    public AudioSource collsionSound;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        collsionSound = GetComponent<AudioSource>();    
    }

    private void OnCollisionEnter(Collision collision)
    {
        collsionSound.Play();
        print("충돌발생!");
    }
}
