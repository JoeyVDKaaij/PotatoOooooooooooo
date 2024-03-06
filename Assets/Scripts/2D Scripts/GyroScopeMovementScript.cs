using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class GyroScopeMovementScript : MonoBehaviour
{
    private Gyroscope _gyroscope;

    private float movementSpeed = 5;
    
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
    }

    private void Update()
    {
        if (_gyroscope != null  && !MinigameManager.instance.StopMinigame)
        {
            // Get the orientation of gravity
            Vector3 gravityDirection = -Input.gyro.gravity;

            // Adjust for orientation differences between device and Unity's coordinate system
            Vector3 movementVector = new Vector3(-gravityDirection.x, 0, 0);

            // Apply the movement to the object
            transform.Translate(movementVector * Time.deltaTime * movementSpeed);
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Obstacle"))
        {
            MinigameManager.instance.EndMinigameTimer();
        }
    }
}