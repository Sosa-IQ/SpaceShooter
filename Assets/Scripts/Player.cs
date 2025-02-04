using System;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.Rendering;

public class Player : MonoBehaviour
{
    public int hitpoints = 3;
    public float moveSpeed = 4.5f;
    public float fireDelay = 0.45f;
    public float rotationSpeed = 50.0f;
    public float maxRotation = 10.0f;
    public GameObject bullet;
    public GameObject healthBar;
    public Image healthBarFill;
    private BoxCollider2D boxCollider;
    public Animator animator;
    float bulletCooldown = 0f;
    public int speedPowerup = 0;
    public int fireratePowerup = 0;
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

        // Update health bar
        healthBarFill.fillAmount = (float) hitpoints / 3;
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

        // Update health bar position
        Vector3 offsetDirection = Quaternion.Euler(0, 0, 0) * Vector3.up;
        // Adjust the offset to maintain center position upon rotations
        Vector3 rotatedOffset = Quaternion.Euler(0, 0, transform.rotation.eulerAngles.z) * (offsetDirection * -0.6f);
        // Position the health bar
        healthBar.transform.position = transform.position + rotatedOffset;

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
        if (bulletCooldown <= 0) {
            bulletCooldown = fireDelay;
            // offset bullet in front of player
            Vector3 bulletOffset = transform.rotation * new Vector3(0, 0.5f, 0);
            Instantiate(bullet, transform.position + bulletOffset, transform.rotation);
        }
    }

    void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Powerup")) {
            Destroy(other.gameObject);
            // if speed powerup, increase speed
            if (other.name == "speedPU(Clone)" && speedPowerup < 3) {
                speedPowerup++;
                moveSpeed += 2;
            }
            // if firereate powerup, decrease fire delay
            else if (other.name == "fireratePU(Clone)" && fireratePowerup < 3) {
                fireratePowerup++;
                fireDelay -= 0.1f;
            }
        }
    }

    public void DestroyPlayer() {
        Destroy(gameObject);
    }
}
