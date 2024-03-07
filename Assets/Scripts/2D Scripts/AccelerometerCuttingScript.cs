using System;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class AccelerometerCuttingScript : MonoBehaviour
{
    // Sensitivity of the movement detection
    public float sensitivity = 5f;

    private float cutsNeeded = 30;

    private float currentCuts = 0;

    private float timer;

    private float delayCheck = 0.5f;

    private Animator _animator;

    private bool _stopMinigame;

    [SerializeField] private Sprite[] spritesphases = null;

    private float currentSprite = 0;
    
    private SpriteRenderer SR = null;
    
    private void Start()
    {
        SR = GetComponent<SpriteRenderer>();
        if (SR != null && spritesphases != null)
            SR.sprite = spritesphases[(int)currentSprite];
        _animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (!MinigameManager.instance.StopMinigame)
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

            if (SR != null && spritesphases != null)
            {
                if (currentCuts / cutsNeeded * 100 >= (currentSprite + 1) / spritesphases.Length * 100 && currentSprite + 1 != spritesphases.Length - 1)
                {
                    Debug.Log((currentSprite + 1) / spritesphases.Length);
                    currentSprite++;
                    SR.sprite = spritesphases[(int)currentSprite];
                }
            }

            if (currentCuts >= cutsNeeded)
            {
                if (SR != null && spritesphases != null)
                    SR.sprite = spritesphases[4];
                MinigameManager.instance.EndMinigameTimer();
            }
        }
    }
}