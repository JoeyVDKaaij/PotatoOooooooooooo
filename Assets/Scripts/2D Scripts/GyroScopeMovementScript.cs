using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class GyroScopeMovementScript : MonoBehaviour
{
    private Gyroscope _gyroscope;

    private float movementSpeed = 5;

    private float driftSpeed = 0;

    private float driftSpeedMultiplier = 0.1f;
    
    private Vector3 oldPosition;

    private Rigidbody2D rb;

    private bool endMinigame = false;
    
    private void Start()
    {
        if (SystemInfo.supportsGyroscope)
        {
            _gyroscope = Input.gyro;
            _gyroscope.enabled = true; // Enable the gyroscope
        }
        else
        {
            Debug.Log("Gyroscope not supported on this device.");
        }

        oldPosition = transform.position;

        rb = GetComponent<Rigidbody2D>();
        
        MinigameManager.EndMinigame += EndMinigame;
    }

    private void OnDestroy()
    {
        MinigameManager.EndMinigame -= EndMinigame;
    }

    private void Update()
    {
        if (_gyroscope != null && !endMinigame)
        {
            // Get the orientation of gravity
            Vector3 gravityDirection = -Input.gyro.gravity;

            // Adjust for orientation differences between device and Unity's coordinate system
            Vector3 movementVector = new Vector3(-gravityDirection.x, 0, 0);

            // Apply the movement to the object
            transform.Translate(movementVector * Time.deltaTime * 5f);
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Obstacle"))
        {
            Debug.Log("You died!");
        }
    }

    private void EndMinigame()
    {
        endMinigame = true;
    }
}