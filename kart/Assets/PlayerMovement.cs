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

    public GameObject wheelFL;
    public GameObject wheelFR;
    public GameObject wheelRL;
    public GameObject wheelRR;

    public float wheelRadius = 0.5f;

    private Quaternion initialRotationFL;
    private Quaternion initialRotationFR;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        initialRotationFL = wheelFL.transform.rotation;
        initialRotationFR = wheelFR.transform.rotation;
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

        UpdateWheelRotation(wheelFL, wheelFR, wheelRL, wheelRR, horizontalInput);
    }

    void UpdateWheelRotation(GameObject wheelFL, GameObject wheelFR, GameObject wheelRL, GameObject wheelRR, float horizontalInput)
    {
        // Atualiza a rotação das rodas frontais e traseiras
        RotateWheel(wheelRL);
        RotateWheel(wheelRR);

        RotateWheel(wheelFL);
        RotateWheel(wheelFR);
    }

    void RotateWheel(GameObject wheel)
    {
        float speed = rb.linearVelocity.magnitude * 2;

        float rotationsPerSecond = speed / (2 * Mathf.PI * wheelRadius);
        float rotationAngle = rotationsPerSecond * 360 * Time.deltaTime; // Ângulo de rotação

        wheel.transform.Rotate(0, rotationAngle, 0);
    }

    void TorceFrontWheels(GameObject wheel, float horizontalInput)
    {
        // Se o input for 0, reseta a rotação
        if (horizontalInput == 0)
        {
            if (wheel == wheelFL)
            {
                // Restaura a rotação original da roda
                wheel.transform.rotation = Quaternion.Euler(wheel.transform.rotation.eulerAngles.x, wheel.transform.rotation.eulerAngles.y, rb.rotation.eulerAngles.x);

            }
            else if (wheel == wheelFR)
            {
                wheel.transform.rotation = Quaternion.Euler(wheel.transform.rotation.eulerAngles.x, wheel.transform.rotation.eulerAngles.y, rb.rotation.eulerAngles.x);

            }
        }
        else
        {
            // Calcula a rotação com base no input horizontal
            float rotation = horizontalInput * turnSpeed * -3 * Time.fixedDeltaTime;

            // Verifica a rotação atual no eixo Z
            float currentRotationZ = wheel.transform.rotation.eulerAngles.z;

            // Ajusta a rotação para o intervalo de -180 a 180
            if (currentRotationZ > 180)
            {
                currentRotationZ -= 360; // Corrige a rotação para o intervalo -180 a 180
            }

            // Verifica se a rotação atual está dentro dos limites de -50 a 50 graus
            if (Mathf.Abs(currentRotationZ) < 50)
            {
                // Aplica a rotação no eixo Z
                Quaternion turnOffset = Quaternion.Euler(0, 0, rotation);
                wheel.transform.rotation = wheel.transform.rotation * turnOffset;
            }
        }
    }





}
