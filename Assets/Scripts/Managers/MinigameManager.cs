using System;
using UnityEngine;

public class MinigameManager : MonoBehaviour
{
    public static MinigameManager instance { get; private set; }

    [SerializeField, Tooltip("Set the current MiniGame that the player is playing.")]
    private Minigame _minigame;

    private float[] playerScoresSBTW = {0,0,0,0};
    private float[] playerScoresSYS = {0,0,0,0};
    private float[] playerScoresHC = {0,0,0,0};
    private float[] playerScoresETM = {0,0,0,0};
    private float[] playerScoresKYE = {0,0,0,0};

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
            case Minigame.sweptbythewind:
                SweptByTheWind();
                break;
            case Minigame.spreadthesails:
                SpreadYourSails();
                break;
            case Minigame.hungrycrew:
                HungryCrew();
                break;
            case Minigame.escapethemonster:
                EscapeTheMonster();
                break;
            case Minigame.knowyourenemies:
                KnowYourEnemies();
                break;
        }
    }
    # endregion

    private void EscapeTheMonster()
    {
        bool endingMinigame = true;
        for (int i = 0; i < playerScoresETM.Length; i++)
        {
            if (playerScoresETM[i] == 0)
                endingMinigame = false;
        }
    }

    private void SweptByTheWind()
    {
        playerScoresSBTW[0]++;
        
        float firstScore = 0;
        bool winning = true;
        for (int i = 0; i < playerScoresSBTW.Length; i++)
        {
            if (i == 0)
                firstScore = playerScoresSBTW[i];
            else if (playerScoresSBTW[i] >= firstScore) winning = false;
        }
        if (winning) EndMinigame?.Invoke();
    }

    private void HungryCrew()
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

    private void KnowYourEnemies()
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

    private void SpreadYourSails()
    {
        timer += Time.deltaTime;
    }

    public void ChangeMinigame(Minigame pMinigame)
    {
        _minigame = pMinigame;
        for (int i = 0; i < playerScoresETM.Length; i++) playerScoresETM[i] = 0;
        for (int i = 0; i < playerScoresSBTW.Length; i++) playerScoresSBTW[i] = 0;
        for (int i = 0; i < playerScoresHC.Length; i++) playerScoresHC[i] = 0;
        for (int i = 0; i < playerScoresKYE.Length; i++) playerScoresKYE[i] = 0;
        for (int i = 0; i < playerScoresSYS.Length; i++) playerScoresSYS[i] = 0;
    }

    public Minigame _Minigame
    {
        get { return _minigame; }
    }

    public void ShotTarget(int pPlayer = 0)
    {
        playerScoresHC[pPlayer]++;
    }

    public void CutRope()
    {
        playerScoresSYS[0] = timer;
        EndMinigame?.Invoke();
    }
}

public enum Minigame
{
    sweptbythewind,
    spreadthesails,
    hungrycrew,
    escapethemonster,
    knowyourenemies
}