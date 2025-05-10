using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour
{
    Vector3 bottomLeft;
    Vector3 topRight  ;
    float screenLeft;
    float screenRight;
    float screenTop  ;
    float screenBottom;

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
        GetBounds(); // Get the screen bounds
        if(CheckBounds())
        {
            Destroy(gameObject); // Destroy the bullet if it goes out of bounds
        }

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
            other.GetComponent<Enemy>().Hurt(0.4f);
            // Destroy the enemy and the bullet
            Destroy(gameObject);
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

    bool CheckBounds()
    {
        // Check if the bullet is out of bounds and destroy it
        if (transform.position.x < screenLeft || transform.position.x > screenRight ||
            transform.position.y < screenBottom || transform.position.y > screenTop)
            return true;
        return false;
    }

}
