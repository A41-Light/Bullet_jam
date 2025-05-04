
using System.Numerics;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;
using Quaternion = UnityEngine.Quaternion;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

public class Player : MonoBehaviour
{
    public float speed = 5f;
    public float mouseSensitivity = 5f;
    public GameObject bulletPrefab;
    public GameObject Turret1;
    public GameObject Turret2;
    public float shootTimer = 0.05f; // Time between shots
    public float currentShootTimer = 0f; // Timer to track shooting cooldown
    public float offset = 0.5f; // Offset for bullet spawn position

    private Rigidbody2D rb;
    private Vector2 moveInput;
    private float angle;
    

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Confined;

        ProcessInput();

        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0f; 

        Vector3 direction = mousePos - transform.position;

        angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle-90);

        if(currentShootTimer > 0f)
        {
            currentShootTimer -= Time.deltaTime; // Decrease the shoot timer
        }


        Shoot();

    }

    void FixedUpdate()
    {
        Move();
    }

    void ProcessInput()
    {

        float moveX = Input.GetAxis("Horizontal") * speed;
        float moveY = Input.GetAxis("Vertical") * speed;
        moveInput = new Vector2(moveX, moveY);

    }

    void Move()
    {
        rb.linearVelocity = moveInput;
    }

    void Shoot()
    {
        if (Input.GetMouseButton(0) && currentShootTimer <= 0f)
        {
            Instantiate(bulletPrefab, Turret1.transform.position, Quaternion.Euler(0, 0, angle));
            Instantiate(bulletPrefab, Turret2.transform.position, Quaternion.Euler(0, 0, angle));
            currentShootTimer = shootTimer; // Reset the shoot timer
        }
    }
}
