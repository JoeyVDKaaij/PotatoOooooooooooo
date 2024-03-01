using System;
using UnityEngine;
using System.Collections;
using Random = UnityEngine.Random;

[RequireComponent(typeof(Rigidbody))]
public class DiceScript : MonoBehaviour
{
    private Rigidbody rb;

    private int diceRoll = 0;

    private bool rolling = false;

    private Vector3 oldPosition;

    [Header("Dice Settings")]
    [SerializeField, Tooltip("Roll the dice by pressing a button for testing purposes.")]
    private bool testDiceRoll = false;

    [SerializeField, Tooltip("How much power the roll has."), Min(0)]
    private float rollSpeed = 10;

    [SerializeField, Tooltip("Wait until the delay is over before it checks the collisions after roll."), Min(0)]
    private float checkDelay = 1;
    
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }
    
    public void RollTheDice(Action<int> callback, Transform pARCamera = null)
    {
        if (pARCamera != null)
            StartCoroutine(WaitForValueChange(callback, pARCamera));
        else 
            StartCoroutine(WaitForValueChange(callback));
            
    }

    private void OnCollisionStay(Collision other)
    {
        if (rolling)
        {
            foreach (ContactPoint contactPoint in other.contacts)
            {
                Vector3 normal = contactPoint.normal;

                // Get cube's local axes
                Vector3 front = transform.forward;
                Vector3 back = -transform.forward;
                Vector3 right = transform.right;
                Vector3 left = -transform.right;
                Vector3 up = transform.up;
                Vector3 down = -transform.up;

                // Calculate dot products
                float dotFront = Vector3.Dot(normal, front);
                float dotBack = Vector3.Dot(normal, back);
                float dotRight = Vector3.Dot(normal, right);
                float dotLeft = Vector3.Dot(normal, left);
                float dotUp = Vector3.Dot(normal, up);
                float dotDown = Vector3.Dot(normal, down);

                // Determine the side of collision
                if (dotFront > 0.9f)
                {
                    diceRoll = 4;
                }
                else if (dotBack > 0.9f)
                {
                    diceRoll = 3;
                }
                else if (dotRight > 0.9f)
                {
                    diceRoll = 5;
                }
                else if (dotLeft > 0.9f)
                {
                    diceRoll = 2;
                }
                else if (dotUp > 0.9f)
                {
                    diceRoll = 1;
                }
                else if (dotDown > 0.9f)
                {
                    diceRoll = 6;
                }
            }
        }
        else diceRoll = 0;
    }

    IEnumerator WaitForValueChange(Action<int> callback, Transform pARCamera = null)
    {
        if (pARCamera != null)
            rb.AddForce(RandomizeVector(pARCamera) * rollSpeed, ForceMode.Impulse);
        else 
            rb.AddForce(RandomizeVector() * rollSpeed, ForceMode.Impulse);
        rb.AddTorque(new Vector3(1,1,1));
        
        yield return new WaitForSeconds(checkDelay);

        if (transform.position == oldPosition)
        {
            rb.AddForce(0,0.1f,0, ForceMode.Impulse);
        }
        
        rolling = true;
        
        // Simulating waiting for the value to change, replace this with your actual condition
        while (diceRoll == 0)
        {
            yield return null; // Yielding null will wait for the next frame
        }
        
        rolling = false;
        
        // Once the value changes, return it via the callback
        callback(diceRoll);
    }

    private Vector3 RandomizeVector(Transform pARCamera = null)
    {
        Vector3 vector = new Vector3();
        if (pARCamera != null)
            vector = pARCamera.forward * Random.Range(5, 10);
        else
        {
            vector.x = Random.Range(-5,5);
            vector.y = Random.Range(-1,1);
            vector.z = Random.Range(-5,5);
        }
        
        
        if (vector != Vector3.zero)
        {
            vector.y = 1;
        }

        if (vector.x == 0)
            vector.x = 1;
        
        if(vector.z == 0)
            vector.z = 1;
        
        return vector;
    }
    
    private void Update()
    {
        if (testDiceRoll && Input.GetKeyDown(KeyCode.Space)) RollTheDice(result =>
            {
                Debug.Log("You rolled " + result);
            }
        );

        oldPosition = transform.position;
    }
}
