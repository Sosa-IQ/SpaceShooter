using System;
using System.Drawing;
using UnityEngine;
using UnityEngine.Rendering;

public class Player : MonoBehaviour
{

    public float moveSpeed = 6.0f;
    public float rotationSpeed = 50.0f;
    public float maxRotation = 10.0f;
    public GameObject bullet;
    private BoxCollider2D boxCollider;
    float bulletCooldown = 0f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

        boxCollider = GetComponent<BoxCollider2D>();

    }

    // Update is called once per frame
    void Update()
    {
        MoveShip();

        // Fire bullet
        bulletCooldown -= Time.deltaTime;
        if (Input.GetButton("Fire1")) FireBullet();
    }

    void MoveShip() {

        // Input values
        float moveHorizontal = Input.GetAxisRaw("Horizontal");
        float moveVertical = Input.GetAxisRaw("Vertical");

        // Calculate target rotation based on movement direction
        float targetRotation = -moveHorizontal * maxRotation;

        // Get current Z rotation
        float currentRotation = transform.rotation.eulerAngles.z;
        if (currentRotation > 180) {
            currentRotation -= 360;
        }
        // Smoothly lean to target rotation
        float newRotation = Mathf.Lerp(currentRotation, targetRotation, rotationSpeed * Time.deltaTime);
        
        // Apply rotation
        transform.rotation = Quaternion.Euler(0, 0, newRotation);

        // Move ship
        Vector3 movement = new Vector3(moveHorizontal, moveVertical, 0.0f).normalized;

        // Try moving horizontally
        Vector3 horizontalMove = transform.position + new Vector3(movement.x, 0, 0) * moveSpeed * Time.deltaTime;
        if (!WouldHitWall(horizontalMove))
        {
            transform.position = horizontalMove;
        }

        // Try moving vertically
        Vector3 verticalMove = transform.position + new Vector3(0, movement.y, 0) * moveSpeed * Time.deltaTime;
        if (!WouldHitWall(verticalMove))
        {
            transform.position = verticalMove;
        }

    }

    bool WouldHitWall(Vector3 position)
    {
        // Check if the new position would overlap with any wall triggers
        Collider2D[] hits = Physics2D.OverlapBoxAll(position, boxCollider.size, 0f);
        foreach (Collider2D hit in hits)
        {
            if (hit.CompareTag("Wall"))
            {
                // Debug.Log("Hit Wall!");
                return true;
            }
        }
        return false;
    }

    void FireBullet() {
        // Create bullet object
        float fireRate = bullet.GetComponent<Bullet>().fireDelay;
        if (bulletCooldown <= 0) {
            bulletCooldown = fireRate;
            // offset bullet in front of player
            Vector3 bulletOffset = transform.rotation * new Vector3(0, 0.5f, 0);
            Instantiate(bullet, transform.position + bulletOffset, transform.rotation);
        }
    }
}
