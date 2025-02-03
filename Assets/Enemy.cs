using UnityEngine;

public class Enemy : MonoBehaviour
{

    public float enemySpeed = 2f;
    public float rotationSpeed = 50f;
    public int hitpoints = 1;
    public GameObject speedPowerup;
    public GameObject fireratePowerup;
    Transform player;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
        // track player position
        if (player == null) {
            GameObject go = GameObject.Find("Player");
            // if player is found, set player to player's transform
            if (go != null) {
                player = go.transform;
            }
        }

        if (player == null) return;

        // Rotate to face player
        Vector3 direction = player.position - transform.position;
        direction.Normalize();

        float zAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg + 90;
        Quaternion desiredRot = Quaternion.Euler(0, 0, zAngle);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, desiredRot, rotationSpeed * Time.deltaTime);

        // Move enemy
        Vector3 movement = new Vector3(0, enemySpeed * Time.deltaTime, 0.0f);
        transform.position -= transform.rotation * movement;
    }

    void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Bullet")) {
            hitpoints--; // Decrement hitpoints if hit by bullet
            if (hitpoints <= 0) {
                Destroy(gameObject); // Destroy self if hitpoints are 0
                // Randomly drop powerup
                DropPowerup();
            }
            Destroy(other.gameObject); // Destroy bullet
        } else if (other.CompareTag("Player")) {
            Destroy(gameObject); // Destroy self if hit player
            other.GetComponent<Player>().hitpoints--; // Decrement player hitpoints
            if (other.GetComponent<Player>().hitpoints <= 0) {
                Destroy(other.gameObject); // If player hp is 0, destroy player
            }
        }
    }

    void DropPowerup() {
        var playerComp = player.GetComponent<Player>();
        // if player is at max powerups, don't drop powerup
        if (playerComp.speedPowerup >= 3 && playerComp.fireratePowerup >= 3) return;
        // 20% chance to drop powerup
        if (Random.value <= 0.20) {
            bool dropSpeedPowerup = Random.value < 0.5; // 50% chance to drop speed powerup
            
            // if player has max speed powerups, drop firerate powerup (vice versa)
            if (playerComp.speedPowerup >= 3) {
                dropSpeedPowerup = false;
            } else if (playerComp.fireratePowerup >= 3) {
                dropSpeedPowerup = true;
            }

            if (dropSpeedPowerup) {
                Instantiate(speedPowerup, transform.position, Quaternion.identity);
            } else {
                Instantiate(fireratePowerup, transform.position, Quaternion.identity);
            }
        }
    }
}
