using UnityEngine;

public class CubeMover : MonoBehaviour
{
    InputSystem_Actions input;
    Vector2 move;

    public float speed = 5f;

    void Awake()
    {
        input = new InputSystem_Actions();

        input.Player.Move.performed += ctx =>
        {
            move = ctx.ReadValue<Vector2>();
        };

        input.Player.Move.canceled += ctx =>
        {
            move = Vector2.zero;
        };
    }

    void OnEnable()
    {
        input.Enable();
    }

    void OnDisable()
    {
        input.Disable();
    }

    void Update()
    {
        transform.Translate(
            new Vector3(move.x, 0, move.y)
            * speed * Time.deltaTime
        );
    }
}