using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float speed = 3f; // Speed of the enemy movement
    public float dashTimer = 5f; // Speed of the enemy dash
    public GameObject bulletPrefab; // Prefab of the bullet to be instantiated
    public GameObject Turret1; // Reference to the first turret GameObject
    public GameObject Turret2; // Reference to the second turret GameObject
    public float shootTimer = 0.05f; // Time between shots

    private float currentSpeed = 0f; // Current speed of the enemy
    private float dash_duration = 0.25f; // Duration of the dash in seconds
    private float is_dashingCounter = 0f; // Counter to track dash duration
    private float dashTimerCounter = 0f; // Timer to track dash cooldown
    private Rigidbody2D rb; // Reference to the Rigidbody2D component
    private GameObject player; // Reference to the player GameObject
    private float angle; // Angle of rotation for the enemy
    private float currentShootTimer = 0f; // Timer to track shooting cooldown

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        speed = Random.Range(2.5f, 3.5f); // Randomize the speed of the enemy between 1 and 5
        rb = GetComponent<Rigidbody2D>(); // Get the Rigidbody2D component attached to this GameObject
        player = GameObject.FindGameObjectWithTag("Player"); // Find the player GameObject by its tag
        if (player == null)
        {
            Debug.LogError("Player not found! Make sure the player has the 'Player' tag.");
        }
    }

    // Update is called once per frame
    void Update()
    {
        ManageDashCounter(); // Manage the dash and shoot timers

        FacePlayer();
        Move();
        Shoot();

        if (dashTimerCounter == 0f  && Random.Range(0,1) < 0.0001) // Check if the enemy is close to the player
        {
            Dash(); // Call the Dash method if the enemy is close to the player
            dashTimerCounter = dashTimer; // Reset the dash timer
        }

        if(is_dashingCounter == 0f)
            currentSpeed = speed; // Reset the current speed to normal speed

        if(currentShootTimer > 0f)
        {
            currentShootTimer -= Time.deltaTime; // Decrease the shoot timer
        }

    }

    void FacePlayer()
    {
        if (player != null)
        {
            Vector3 direction = player.transform.position - transform.position; // Calculate the direction to the player
            angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg; // Calculate the angle in degrees
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, 0, angle - 90), Time.deltaTime * 20);

        }
    }

    void Move()
    {
        if (player != null)
        {
            Vector3 direction = player.transform.position - transform.position; // Calculate the direction to the player
            rb.linearVelocity = direction.normalized * currentSpeed; // Move the enemy towards the player
        }
    }

    void Dash()
    {
        if (player != null)
        {
            currentSpeed = speed * 4f; // Set the current speed to double the normal speed
            is_dashingCounter = dash_duration; // Set the dash duration
        }
    }

    void Shoot()
    {
        if (currentShootTimer <= 0f)
        {
            Instantiate(bulletPrefab, Turret1.transform.position, Quaternion.Euler(0, 0, angle));
            Instantiate(bulletPrefab, Turret2.transform.position, Quaternion.Euler(0, 0, angle));
            currentShootTimer = shootTimer; // Reset the shoot timer
        }
    }

    void ManageDashCounter()
    {
        if(dashTimerCounter > 0f)
        {
            dashTimerCounter -= Time.deltaTime; // Decrease the dash timer
        }
        if(dashTimerCounter < 0f)
        {
            dashTimerCounter = 0f; // Reset the dash timer
        }

        if(is_dashingCounter > 0f)
        {
            is_dashingCounter -= Time.deltaTime; // Decrease the dash timer
        }
        if(is_dashingCounter < 0f)
        {
            is_dashingCounter = 0f; // Reset the dash timer
        }
    }
}
