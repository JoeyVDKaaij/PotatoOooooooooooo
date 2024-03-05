using System;
using UnityEngine;

public class MinigameManager : MonoBehaviour
{
    public static MinigameManager instance { get; private set; }

    [SerializeField, Tooltip("Set the current MiniGame that the player is playing.")]
    private Minigame _minigame;

    private float[] playerScores = {0,0,0,0};

    public static event Action EndMinigame;

    private float timer = 0;

    [SerializeField, Tooltip("Set the time how long the shooting minigames goes on for.")] 
    private float timeUntilShootingEnds = 30;
    
    # region UnityFunctions
    
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            // if (transform.parent.gameObject != null) DontDestroyOnLoad(transform.parent.gameObject);
            // else 
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnDestroy()
    {
        if (instance == this)
        {
            instance = null;
        }
    }
    
    void Update()
    {
        switch (_minigame)
        {
            case Minigame.dodge:
                Dodge();
                break;
            case Minigame.cuttherope:
                break;
            case Minigame.shoot:
                Shoot();
                break;
            case Minigame.redlightgreenlight:
                RedLightGreenLight();
                break;
            case Minigame.memory:
                break;
        }
    }
    # endregion

    private void RedLightGreenLight()
    {
        bool endingMinigame = true;
        for (int i = 0; i < playerScores.Length; i++)
        {
            if (playerScores[i] == 0)
                endingMinigame = false;
        }
    }

    private void Dodge()
    {
        playerScores[0]++;
        
        float firstScore = 0;
        bool winning = true;
        for (int i = 0; i < playerScores.Length; i++)
        {
            if (i == 0)
                firstScore = playerScores[i];
            else if (playerScores[i] >= firstScore) winning = false;
        }
        if (winning) EndMinigame?.Invoke();
    }

    private void Shoot()
    {
        // Minigame stops when player got most points
        //
        // float firstScore = 0;
        // bool winning = true;
        // for (int i = 0; i < playerScores.Length; i++)
        // {
        //     if (i == 0)
        //         firstScore = playerScores[i];
        //     else if (playerScores[i] >= firstScore) winning = false;
        // }
        // if (winning) EndMinigame?.Invoke();
        
        // Minigame ends when timer runs out.
        timer += Time.deltaTime;
        
        if (timer >= timeUntilShootingEnds) EndMinigame?.Invoke();
    }

    public void ChangeMinigame(Minigame pMinigame)
    {
        _minigame = pMinigame;
        for (int i = 0; i < playerScores.Length; i++) playerScores[i] = 0;
    }

    public Minigame _Minigame
    {
        get { return _minigame; }
    }

    public void ShotTarget(int pPlayer = 0)
    {
        playerScores[pPlayer]++;
    }
}

    

public enum Minigame
{
    dodge,
    cuttherope,
    shoot,
    redlightgreenlight,
    memory
}