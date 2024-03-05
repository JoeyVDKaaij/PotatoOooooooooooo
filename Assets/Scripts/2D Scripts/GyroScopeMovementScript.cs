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
        if (_gyroscope != null && endMinigame)
        {
            // Get the rotation rate from the gyroscope
            Quaternion gyroRotation = _gyroscope.attitude;

            // Extract the Euler angles for easier manipulation
            Vector3 euler = gyroRotation.eulerAngles;

            if (oldPosition.x > transform.position.x)
            {
                oldPosition = transform.position;
                driftSpeed += driftSpeedMultiplier;
            }
            else driftSpeed = 0;
            
            // Detect leftward leaning
            if (euler.z > 30f && euler.z < 135f)
            {
                rb.velocity = new Vector3(-(movementSpeed + driftSpeed), 0);
            }
            // Detect rightward leaning
            else if (euler.z > 225f && euler.z < 330f)
            {
                rb.velocity = new Vector3((movementSpeed + driftSpeed), 0);
            }
            else rb.velocity = Vector2.zero; 
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