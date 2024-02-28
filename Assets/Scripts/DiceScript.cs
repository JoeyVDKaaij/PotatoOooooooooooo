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

    private float timer;
    
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }
    
    public void RollTheDice(Action<int> callback)
    {
        StartCoroutine(WaitForValueChange(callback));
    }

    private void OnCollisionStay(Collision other)
    {
        if (rolling)
        {
            foreach (ContactPoint contactPoint in other.contacts)
            {
                Vector3 normal = contactPoint.normal;

                if (normal == transform.up)
                    diceRoll = 1;
                else if (normal == -transform.up)
                    diceRoll = 6;
                else if (normal == transform.right)
                    diceRoll = 5;
                else if (normal == -transform.right)
                    diceRoll = 2;
                else if (normal == transform.forward)
                    diceRoll = 4;
                else if (normal == -transform.forward)
                    diceRoll = 3;
            }
        }
        else diceRoll = 0;
    }

    IEnumerator WaitForValueChange(Action<int> callback)
    {
        rb.AddForce(RandomizeVector() * rollSpeed, ForceMode.Impulse);
        rb.AddTorque(new Vector3(1,1,1));
        
        rolling = true;
        
        // Simulating waiting for the value to change, replace this with your actual condition
        while (diceRoll == 0)
        {
            yield return null; // Yielding null will wait for the next frame
        }

        yield return new WaitForSeconds(checkDelay);
        
        rolling = false;
        timer = 0;
        
        // Once the value changes, return it via the callback
        callback(diceRoll);
    }

    private Vector3 RandomizeVector()
    {
        Vector3 vector = new Vector3();
        
        vector.x = Random.Range(-5,5);
        vector.y = Random.Range(-1,1);
        vector.z = Random.Range(-5,5);

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
