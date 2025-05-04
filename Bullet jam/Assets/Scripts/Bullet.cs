using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 10f; // Speed of the bullet
    public float lifetime = 2f; // Time before the bullet is destroyed

    private Rigidbody2D rb;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        // Destroy the bullet after a certain time
        Destroy(gameObject, lifetime);
    }

    void FixedUpdate()
    {
        // Ensure the bullet moves in a straight line
        rb.MovePosition(rb.position + (Vector2)transform.right * speed * Time.fixedDeltaTime);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the bullet hits an enemy or any other object
        if (other.CompareTag("Enemy"))
        {
            // Destroy the enemy and the bullet
            Destroy(gameObject);
        }
    }
}
