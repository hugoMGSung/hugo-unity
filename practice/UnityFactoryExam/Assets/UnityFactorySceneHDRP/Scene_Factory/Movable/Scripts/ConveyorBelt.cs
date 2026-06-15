using UnityEngine;

public class ConveyorBelt : MonoBehaviour
{
    public Vector3 moveDirection = Vector3.right;
    public float speed = 0.1f;

    private bool isRunning = true;

    private void OnCollisionStay(Collision collision)
    {
        Rigidbody rb = collision.rigidbody;
        if (rb == null) return;

        if (!isRunning)
        {
            rb.linearVelocity = Vector3.zero;
            return;
        }

        Vector3 velocity = moveDirection.normalized * speed;

        rb.linearVelocity = new Vector3(
            velocity.x,
            rb.linearVelocity.y,
            velocity.z
        );
    }

    public void Stop()
    {
        isRunning = false;
        Debug.Log("Conveyor Stop");
    }

    public void StartBelt()
    {
        isRunning = true;
        Debug.Log("Conveyor Start");
    }
}