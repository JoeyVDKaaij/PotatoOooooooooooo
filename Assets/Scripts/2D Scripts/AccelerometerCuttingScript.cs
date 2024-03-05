using System;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class AccelerometerCuttingScript : MonoBehaviour
{
    // Sensitivity of the movement detection
    public float sensitivity = 5f;

    private int cutsNeeded = 30;

    private int currentCuts = 0;

    private float timer;

    private float delayCheck = 0.5f;

    private Animator _animator;

    private void Start()
    {
        _animator = GetComponent<Animator>();
    }

    void Update()
    {
        // Get the acceleration vector
        Vector3 acceleration = Input.acceleration;

        // Check the z-component of the acceleration vector
        float yAcceleration = acceleration.y;

        timer += Time.deltaTime;
        
        // Check if the z-component of acceleration exceeds the sensitivity threshold
        if (yAcceleration > sensitivity)
        {
            _animator.SetBool("MoveDown", !_animator.GetBool("MoveDown"));
            if (timer >= delayCheck)
            {
                Debug.Log("Phone is moving forwards");
                // Your if statement or method call here
                currentCuts++;
                timer = 0;
            }
        }
        
        if (currentCuts >= cutsNeeded) Debug.Log("You win!!");
    }
}