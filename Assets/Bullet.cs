using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float bulletSpeed = 6f;
    public float fireDelay = 0.5f;
    public float lifetime = 4.0f;
    private BoxCollider2D boxCollider;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        boxCollider = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        // Move bullet
        Vector3 movement = new Vector3(0, bulletSpeed * Time.deltaTime, 0.0f);
        transform.position += transform.rotation * movement;

        // Destroy bullet on timer
        lifetime -= Time.deltaTime;
        if (lifetime <= 0) Destroy(gameObject);
    }
}
