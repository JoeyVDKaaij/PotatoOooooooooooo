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

    private void Update()
    {
        if (testDiceRoll && Input.GetKeyDown(KeyCode.Space)) RollTheDice(result =>
            {
                Debug.Log("You rolled " + result);
            }
        );
    }

    public void RollTheDice(Action<int> callback)
    {
        StartCoroutine(WaitForValueChange(callback));
    }

    private void OnCollisionStay(Collision other)
    {
        if (rolling)
        {
            timer += Time.time;
            if (timer >= checkDelay)
            {
                foreach (ContactPoint contactPoint in other.contacts)
                {
                    Vector3 normal = contactPoint.normal;

                    if (normal == Vector3.up)
                        diceRoll = 1;
                    else if (normal == Vector3.down)
                        diceRoll = 2;
                    else if (normal == Vector3.left)
                        diceRoll = 3;
                    else if (normal == Vector3.right)
                        diceRoll = 4;
                    else if (normal == Vector3.forward)
                        diceRoll = 5;
                    else if (normal == Vector3.back)
                        diceRoll = 6;
                }
            }
        }
        else diceRoll = 0;
    }

    IEnumerator WaitForValueChange(Action<int> callback)
    {
        rb.AddForce(RandomizeVector() * rollSpeed);
        
        rolling = true;
        
        // Simulating waiting for the value to change, replace this with your actual condition
        while (diceRoll == 0)
        {
            yield return null; // Yielding null will wait for the next frame
        }

        rolling = false;
        timer = 0;
        
        // Once the value changes, return it via the callback
        callback(diceRoll);
    }

    private Vector3 RandomizeVector()
    {
        Vector3 vector = new Vector3();
        
        vector.x = Random.Range(-1,1);
        vector.y = Random.Range(-1,1);
        vector.z = Random.Range(-1,1);

        if (vector != Vector3.zero) vector.y = 1;
        
        return vector;
    }
}
