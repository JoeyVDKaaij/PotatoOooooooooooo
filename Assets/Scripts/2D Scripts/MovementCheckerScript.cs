using System;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Assertions.Must;

public class MovementCheckerScript : MonoBehaviour
{
    private CheckPhase checkPhase;

    private float timer;

    [SerializeField, Tooltip("The cooldown in seconds before the GameObject checks again."), Min(0)]
    private float coolDown = 4; 
    [SerializeField, Tooltip("How long before the checking actually starts")]
    private float headsupCount = 1;

    [SerializeField, Tooltip("Set the boats")]
    private GameObject[] boats = null;

    private List<Vector3> boatOldPositions = new List<Vector3>();

    private void Start()
    {
        if (boats[0] != null)
        {
            foreach (var boat in boats)
            {
                boatOldPositions.Add(Vector3.zero);
            }
        }
        else
        {
            Debug.LogError("No boats to check!");
        }
    }

    void Update()
    {
        if (boats[0] != null)
        {
            timer += Time.deltaTime;

            if (timer >= coolDown && checkPhase == CheckPhase.sleep)
            {
                Debug.Log("ABOUT TO CHECK");
                checkPhase = CheckPhase.awakening;
                MentionPhaseChange();
                timer = 0;
            }
            else if (timer >= headsupCount && checkPhase == CheckPhase.awakening)
            {
                Debug.Log("CHECKING MOVEMENT");
                checkPhase = CheckPhase.awake;
                MentionPhaseChange();
                for (int i = 0; i < boats.Length; i++)
                {
                    boatOldPositions[i] = boats[i].transform.position;
                }
                timer = 0;
            }
            else if (timer >= coolDown && checkPhase == CheckPhase.awake)
            {
                Debug.Log("NOT CHECKING ANYMORE");
                checkPhase = CheckPhase.sleep;
                MentionPhaseChange();
                timer = 0;
            }

            if (checkPhase == CheckPhase.awake)
            {
                for (int i = 0; i < boats.Length; i++)
                {
                    if (boatOldPositions[i] != boats[i].transform.position)
                    {
                        Debug.Log("YOU MOVED DURING CHECK WTF BRO?!?!?!??!??!?");
                    }
                }
            }
        }
    }

    private void MentionPhaseChange()
    {
        foreach (var boat in boats)
        {
            boat.GetComponent<Movement2DScript>().SyncAIWithCheckPhase(checkPhase);
        }
    }
}

public enum CheckPhase
{
    sleep,
    awakening,
    awake
}