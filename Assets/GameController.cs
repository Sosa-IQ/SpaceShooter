using System;
using System.Drawing;
using UnityEngine;
using UnityEngine.Rendering;

public class GameController : MonoBehaviour
{

    public float moveSpeed = 6.0f;
    public float rotationSpeed = 50.0f;
    public float maxRotation = 10.0f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        MoveShip();
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
        transform.position += movement * moveSpeed * Time.deltaTime;

    }
}
