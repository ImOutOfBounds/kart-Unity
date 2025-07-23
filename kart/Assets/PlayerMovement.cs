using UnityEngine;
using static UnityEditor.Searcher.SearcherWindow.Alignment;

[RequireComponent(typeof(Rigidbody))]
public class CarController : MonoBehaviour
{
    public float acceleration = 1000f;
    public float initiaAceleration = 1000f;
    public float turnSpeed = 200f;
    public float maxSpeed = 100f;

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        float verticalInput = Input.GetAxis("Vertical");     // W/S ou setas
        float horizontalInput = Input.GetAxis("Horizontal"); // A/D ou setas

        if (verticalInput > 0)
        {
            acceleration = initiaAceleration * 1.5f;
        }
        else if (verticalInput < 0)
        {
            acceleration = initiaAceleration * 0.2f;
        }
        else
        {
            acceleration = initiaAceleration;
        }
        // Calcula a força de avanço
        Vector3 forwardForce = transform.forward * verticalInput * acceleration * Time.fixedDeltaTime;

        // Aplica força para mover o carro para frente/trás
        if (rb.linearVelocity.magnitude < maxSpeed)
        {
            rb.AddForce(forwardForce, ForceMode.Acceleration);
        }

        // Rotação apenas se o carro estiver se movendo
        if (rb.linearVelocity.magnitude > 0.1f)
        {
            float rotation = horizontalInput * turnSpeed * Time.fixedDeltaTime;
            Quaternion turnOffset = Quaternion.Euler(0, rotation, 0);
            rb.MoveRotation(rb.rotation * turnOffset);
        }
    }
}
