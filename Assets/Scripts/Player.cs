using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Movement")] // Atributos visibles de Movimiento
    [SerializeField] private float speed; // Velocidad a la que se mueve el jugador.
    [SerializeField] private float jumpHeight; // Altura del salto en unidades de Unity.
    [SerializeField] private float rotationSensitivity; // Sensibilidad de rotación con el mouse.

    private Transform head; // Objeto hijo que representa la cabeza que permite rotar la cabeza en X.
    private readonly float gravity = -9.8f; // Gravedad (es una constante).
    private CharacterController character; // Character Controller en el Game Object que usa este Script.
    private Control input; // Objeto de Control (Input Actions).
    private float velocityY = 0; // velocidad en Y actual del jugador.

    // Awake se ejecuta antes de cualquier cosa.
    private void Awake()
    {
        // Inicializar la variable de input.
        input = new Control();

        // Acción que se realiza en el salto.
        input.FPS.Jump.performed += ctx =>
        {
            if (character.isGrounded)
            {
                velocityY += Mathf.Sqrt(jumpHeight * -3.0f * gravity);
            }
        };

        Cursor.lockState = CursorLockMode.Locked; // Desaparecer el mouse de la pantalla (podemos sacarlo con Esc)
    }

    // OnEnable se ejecuta al activar el objeto.
    private void OnEnable()
    {
        input.FPS.Enable();
    }

    //OnDisable se ejecuta al desactivar el objeto.
    private void OnDisable()
    {
        input.FPS.Disable();
    }

    // Start se ejecuta antes del primer frame.
    private void Start()
    {
        character = GetComponent<CharacterController>();
        head = transform.GetChild(0); // Busca el primer Transform dentro de este Transform (o sea el primer hijo).
    }

    // Update se ejecuta cada frame.
    private void Update()
    {

    }

    // FixedUpdate se ejecuta cada fixedFrame que es un framing que no cambia aunque la computadora tenga más o menos FPS.
    private void FixedUpdate()
    {
        Vector2 mov = input.FPS.Move.ReadValue<Vector2>(); // Recuperamos el valor de Move.

        Vector2 rot = input.FPS.Look.ReadValue<Vector2>(); // Recuperamos el valor de Look.

        // Si nuestro personaje está en el suelo y la velocidad es menor a 0, se detiene.
        if(character.isGrounded && velocityY < 0)
            velocityY = 0;

        // Rotar el cuerpo en X y la cabeza en Y.
        transform.Rotate(Vector3.up * rot.x * rotationSensitivity);
        head.Rotate(Vector3.right * rot.y * rotationSensitivity);

        // Configurar vector de movimiento.
        Vector3 velocityXZ = transform.rotation * new Vector3(mov.x, 0, mov.y);

        // Mover el personaje en X y Z.
        character.Move(velocityXZ * speed);

        // Configurar la velocidad en Y
        velocityY += gravity * Time.deltaTime;

        // Mover el personaje en Y.
        character.Move(velocityY * Vector3.up * Time.deltaTime);
    }
}
