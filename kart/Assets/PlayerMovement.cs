using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float initialSpeed = 2000f;
    public float speed = 200f;
    public float rotationSpeed = 100f; // Velocidade de rotação

    void Update()
    {
        // Entrada do teclado
        float horizontal = Input.GetAxis("Horizontal"); // A/D ou Setas Esquerda/Direita
        float vertical = Input.GetAxis("Vertical");     // W/S ou Setas Cima/Baixo

        // Ajuste da velocidade com base no input vertical
        if (vertical > 0)
        {
            speed = initialSpeed * 2;
        }
        else if (vertical < 0)
        {
            speed = initialSpeed * 0.5f;
        }
        else
        {
            speed = initialSpeed;
        }

        // Movimento para frente/trás
        Vector3 movement = transform.forward * vertical * speed * Time.deltaTime;
        transform.Translate(movement, Space.World);

        // Rotação com base no input horizontal (eixo Y)
        float rotation = horizontal * rotationSpeed * Time.deltaTime;
        transform.Rotate(0f, rotation, 0f);
    }
}
