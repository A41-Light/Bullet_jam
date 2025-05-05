
using System.Numerics;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using Quaternion = UnityEngine.Quaternion;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

public class Player : MonoBehaviour
{
    Vector3 bottomLeft ;
    Vector3 topRight  ;
    float screenLeft;
    float screenRight;
    float screenTop;
    float screenBottom;

    public float speed = 5f;
    public float mouseSensitivity = 5f;
    public GameObject bulletPrefab;
    public GameObject Turret1;
    public GameObject Turret2;
    public Light2D light2D; // Reference to the Light2D component
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
        GetBounds();

        light2D.intensity = Mathf.Sin(Time.time * 2f) * 1f + 2f;

        ProcessInput();

        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0f; 

        Vector3 direction = mousePos - transform.position;

        angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        //transform.rotation = Quaternion.Euler(0, 0, angle-90);
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, 0, angle - 90), Time.deltaTime * mouseSensitivity);


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
        Vector3 pos = transform.position;
        pos.x = Mathf.Clamp(pos.x, screenLeft, screenRight);
        pos.y = Mathf.Clamp(pos.y, screenBottom, screenTop);
        transform.position = pos;
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

    void GetBounds()
    {
        bottomLeft = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, 0));
        topRight   = Camera.main.ViewportToWorldPoint(new Vector3(1, 1, 0));
        screenLeft   = bottomLeft.x;
        screenRight  = topRight.x;
        screenTop    = topRight.y;
        screenBottom = bottomLeft.y;
    }
}
