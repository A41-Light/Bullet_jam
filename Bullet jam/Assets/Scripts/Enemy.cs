using System;
using System.Numerics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using Quaternion = UnityEngine.Quaternion;
using Random = UnityEngine.Random;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;
using System.Collections;

public class Enemy : MonoBehaviour
{
    public float speed = 3f; // Speed of the enemy movement
    public float dashTimer = 5f; // Speed of the enemy dash
    public GameObject bulletPrefab; // Prefab of the bullet to be instantiated
    public GameObject Turret1; // Reference to the first turret GameObject
    public GameObject Turret2; // Reference to the second turret GameObject
    public float shootTimer = 0.05f; // Time between shots
    public float health = 5f;
    public Light2D light2D; // Reference to the Light2D component
    

    private float currentSpeed = 0f; // Current speed of the enemy
    private float dash_duration = 0.25f; // Duration of the dash in seconds
    private float is_dashingCounter = 0f; // Counter to track dash duration
    private float dashTimerCounter = 0f; // Timer to track dash cooldown
    private float is_hurt_timer = 0f; // Timer to track hurt duration
    private float is_hurt_duration = 0.05f; // Duration of the hurt effect in seconds
    private Rigidbody2D rb; // Reference to the Rigidbody2D component
    private GameObject player; // Reference to the player GameObject
    private Collider2D enemyCollider; // Reference to the player's collider
    private AudioSource audioSource; // Reference to the AudioSource component
    private float angle; // Angle of rotation for the enemy
    private float currentShootTimer = 0f; // Timer to track shooting cooldown
    private float minDistance = 5f;
    private float screenLeft, screenRight, screenTop, screenBottom; // Screen bounds
    private Vector3 bottomLeft, topRight; // Screen bounds in world coordinates
    private bool is_orbiting = false;
    private Animator animator; // Reference to the Animator component
    private float intensity;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        animator = GetComponentInChildren<Animator>(); // Get the Animator component attached to the child GameObject
        audioSource = GetComponent<AudioSource>(); // Get the AudioSource component attached to this GameObject
        minDistance = Random.Range(minDistance - 2f, minDistance + 2f); // Randomize the minimum distance from the player
        speed = Random.Range(2.5f, 3.5f); // Randomize the speed of the enemy between 1 and 5
        rb = GetComponent<Rigidbody2D>(); // Get the Rigidbody2D component attached to this GameObject
        intensity = light2D.intensity; // Get the initial intensity of the light
        player = GameObject.FindGameObjectWithTag("Player"); // Find the player GameObject by its tag
        enemyCollider = GetComponent<Collider2D>(); // Get the Collider2D component attached to this GameObject
        if (player == null)
        {
            Debug.LogError("Player not found! Make sure the player has the 'Player' tag.");
        }
    }

    // Update is called once per frame
    void Update()
    {
        GetBounds(); // Get the screen bounds in world coordinates

        ManageDashCounter(); // Manage the dash and shoot timers

        FacePlayer();
        Move();
        Shoot();

        if (dashTimerCounter == 0f  && Random.Range(0,1) < 0.0001 && !is_orbiting) // Check if the enemy is close to the player
        {
            Dash(); // Call the Dash method if the enemy is close to the player
            dashTimerCounter = dashTimer; // Reset the dash timer
        }

        if(is_dashingCounter == 0f)
        {
            enemyCollider.enabled = true; // Enable the enemy collider after the dash is over
            currentSpeed = speed; // Reset the current speed to normal speed
        }

        if(currentShootTimer > 0f)
        {
            currentShootTimer -= Time.deltaTime; // Decrease the shoot timer
        }

        if(is_hurt_timer < is_hurt_duration)
        {
            is_hurt_timer += Time.deltaTime; // Decrease the hurt timer
        }
        else
        {
            animator.SetBool("is_hurt", false); // Reset the hurt animation
            enemyCollider.enabled = true; // Enable the enemy collider after the hurt state is over
            light2D.intensity = intensity; // Reset the light intensity
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
            float distance = Vector3.Distance(player.transform.position, transform.position); // Calculate the distance to the player
            if(distance > minDistance + 0.2) // Check if the enemy is close to the player
            {
                is_orbiting = false; // Set the orbiting flag to false
                Vector3 direction = player.transform.position - transform.position; // Calculate the direction to the player
                rb.linearVelocity = direction.normalized * currentSpeed; // Move the enemy towards the player
            }
            else if(distance < minDistance - 0.2) // Check if the enemy is close to the player
            {
                is_orbiting = false; // Set the orbiting flag to false
                Vector3 direction = player.transform.position - transform.position; // Calculate the direction to the player
                rb.linearVelocity = -direction.normalized * currentSpeed; // Move the enemy away from the player
            }
            else
            {
                Orbit(); // Call the Orbit method if the enemy is at the right distance from the player
            }

            Vector3 pos = transform.position;

            if(!is_orbiting)
            {
                pos.x = Mathf.Clamp(pos.x, screenLeft, screenRight);
                pos.y = Mathf.Clamp(pos.y, screenBottom, screenTop);
                transform.position = pos;
            }
        }
    }

    void Dash()
    {
        if (player != null)
        {
            currentSpeed = speed * 4f; // Set the current speed to double the normal speed
            enemyCollider.enabled = false; // Disable the enemy collider to avoid collisions during the dash
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

    void GetBounds()
    {
        bottomLeft = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, 0));
        topRight   = Camera.main.ViewportToWorldPoint(new Vector3(1, 1, 0));
        screenLeft   = bottomLeft.x;
        screenRight  = topRight.x;
        screenTop    = topRight.y;
        screenBottom = bottomLeft.y;
    }

    void Orbit()
    {
        Vector2 directionToPlayer = player.transform.position - transform.position; // Calculate the direction to the player
        if (player != null)
        {
            is_orbiting = true; // Set the orbiting flag to true
            Vector2 tangentDirection = Vector2.Perpendicular(directionToPlayer).normalized;
            rb.linearVelocity= tangentDirection * currentSpeed;
        }
    }

    public void Hurt(float damage)
    {
        health -= damage; // Decrease the enemy's health by the damage amount
        is_hurt_timer = 0f; // Reset the hurt timer
        animator.SetBool("is_hurt", true); // Set the hurt animation
        audioSource.Play(); // Play the hurt sound effect
        enemyCollider.enabled = false; // Disable the enemy collider to avoid collisions during the hurt state
        light2D.intensity = 0f;
        if (health <= 0f)
        {
            Die(); // Call the Die method
        }
    }

    void Die()
    {
        Camera_Controller.instance.TriggerShake(0.2f, 0.1f); // Trigger the camera shake effect
        Destroy(gameObject); // Destroy the enemy GameObject
    }
}
