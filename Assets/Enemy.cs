using UnityEngine;

public class Enemy : MonoBehaviour
{

    public float enemySpeed = 2f;
    public float rotationSpeed = 50f;
    public int hitpoints = 1;
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
}
