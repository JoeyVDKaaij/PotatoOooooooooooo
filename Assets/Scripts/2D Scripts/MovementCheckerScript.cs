using System;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Assertions.Must;

[RequireComponent(typeof(SpriteRenderer))]
public class MovementCheckerScript : MonoBehaviour
{
    private CheckPhase checkPhase;

    private float timer;

    [SerializeField, Tooltip("The cooldown in seconds before the GameObject checks again."), Min(0)]
    private float coolDown = 4; 
    [SerializeField, Tooltip("How long before the checking actually starts")]
    private float headsupCount = 1;
    [SerializeField, Tooltip("How long before the checking actually starts")]
    private AudioClip awakeningSound = null;
    [SerializeField, Tooltip("How long before the checking actually starts")]
    private AudioClip awakeSound = null;
    [SerializeField, Tooltip("How long before the checking actually starts")]
    private AudioClip sleepSound = null;

    [SerializeField, Tooltip("Set the boats")]
    private GameObject[] boats = null;

    [SerializeField, Tooltip("Set the dragon sprites (from sleep, then awakening and then awake)")]
    private Sprite[] dragonSprites = null;

    private List<Vector3> boatOldPositions = new List<Vector3>();

    private SpriteRenderer sr;
    private Animator _animator;

    private void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        _animator = GetComponent<Animator>();
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
        if (boats[0] != null && !MinigameManager.instance.StopMinigame)
        {
            timer += Time.deltaTime;

            if (timer >= coolDown && checkPhase == CheckPhase.sleep)
            {
                if (awakeningSound != null) AudioManager.instance.PlaySound(awakeningSound);
                if (dragonSprites != null)
                    sr.sprite = dragonSprites[0];
                checkPhase = CheckPhase.awakening;
                MentionPhaseChange();
                timer = 0;
                _animator.SetInteger(0,1);
            }
            else if (timer >= headsupCount && checkPhase == CheckPhase.awakening)
            {
                if (awakeSound != null) AudioManager.instance.PlaySound(awakeSound);
                if (dragonSprites != null)
                    sr.sprite = dragonSprites[2];
                checkPhase = CheckPhase.awake;
                MentionPhaseChange();
                for (int i = 0; i < boats.Length; i++)
                {
                    boatOldPositions[i] = boats[i].transform.position;
                }
                timer = 0;
                _animator.SetInteger(0,2);
            }
            else if (timer >= coolDown && checkPhase == CheckPhase.awake)
            {
                if (sleepSound != null) AudioManager.instance.PlaySound(sleepSound);
                if (dragonSprites != null)
                    sr.sprite = dragonSprites[3];
                checkPhase = CheckPhase.sleep;
                MentionPhaseChange();
                timer = 0;
                _animator.SetInteger(0,0);
            }

            if (checkPhase == CheckPhase.awake)
            {
                for (int i = 0; i < boats.Length; i++)
                {
                    if (boatOldPositions[i] != boats[i].transform.position)
                    {
                        MinigameManager.instance.GotDetected(i);
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