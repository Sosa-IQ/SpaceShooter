using UnityEngine;

public class Meteor : MonoBehaviour
{
    public float meteorSpeed = 2f;
    public float meteorLifetime = 8f;
    private Vector3 direction;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // grab direction based on initial x position
        if (transform.position.x < 0) {
            direction = new Vector3(1, -1, 0);
        }
        else {
            direction = new Vector3(-1, -1, 0);
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Move meteor
        MoveMeteor();
        // Destroy meteor on timer
        meteorLifetime -= Time.deltaTime;
        if (meteorLifetime <= 0) Destroy(gameObject);
    }

    void MoveMeteor() {
        // if meteor x < 0, move bottom right else move bottom left
        transform.position += meteorSpeed * Time.deltaTime * direction;
    }
}
