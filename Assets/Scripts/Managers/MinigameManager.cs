using System;
using System.Linq;
using UnityEngine;
using TMPro;
using UnityEngine.Playables;
using Random = UnityEngine.Random;

public class MinigameManager : MonoBehaviour
{
    public static MinigameManager instance { get; private set; }

    [SerializeField, Tooltip("Set the current MiniGame that the player is playing.")]
    private Minigame _minigame;

    [SerializeField, Tooltip("Set the GameObject that is being used as a scoreboard.")]
    private GameObject winningScreen = null;

    [SerializeField, Tooltip("Set the Text GameObject that is being used to show who wins.")]
    private TMP_Text winnerText;

    [SerializeField, Tooltip("Set the Text GameObject that is being used to show who wins.")]
    private GameObject continueFromWinningText;

    // Scores from Swept By The Wind
    private float[] playerScoresSBTW = {0,30,45,60};
    // Scores from Spread Your Sails
    private float[] playerScoresSYS = {0,20,25,30};
    // Scores from Hungry Crew
    private float[] playerScoresHC = {0,0,0,0};
    // Scores from Escape The Monster
    private float[] playerScoresETM = {4,4,4,4};
    // Scores from Know Your Enemies
    private float[] playerScoresKYE = {0,1,2,3};

    [SerializeField]
    private bool stopMinigame = true;

    [SerializeField]
    private bool beginMinigame = false;

    private float timer = 0;

    [SerializeField, Tooltip("Set the time how long the Hungry Crew minigame goes on for.")] 
    private float timeUntilShootingEnds = 30;

    [SerializeField, Tooltip("Set the time how long the Know Your Enemies minigame goes on for.")] 
    private float timeUntilMemoryEnds = 30;

    private float winningScreenTime = 3;

    private bool continueFromWinning = false;

    private float continueTextToggleTimer = 1;

    private bool audioPlayed = false;

    [SerializeField] private AudioClip winSoundEffect;

    [SerializeField] private AudioClip loseSoundEffect;

    [SerializeField] private PlayableDirector startAnimation;

    [SerializeField] private PlayableDirector finishAnimation;

    private bool playingFinishAnimation = false;
    
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
        if (!stopMinigame) 
        {
            switch (_minigame)
            {
                case Minigame.sweptbythewind:
                    SweptByTheWind();
                    break;
                case Minigame.spreadyoursails:
                    SpreadYourSails();
                    break;
                case Minigame.hungrycrew:
                    HungryCrew();
                    break;
                case Minigame.knowyourenemies:
                    KnowYourEnemies();
                    break;
            }
        }
        else if (beginMinigame)
        {
            WinningScreen();
        }
        else if (startAnimation.state != PlayState.Playing)
        {
            stopMinigame = false;
            beginMinigame = true;
        }
    }
    # endregion

    private void SweptByTheWind()
    {
        timer += Time.deltaTime;

        if (timer >= playerScoresSBTW[playerScoresSBTW.ToList().IndexOf(playerScoresSBTW.Max())])
        {
            playerScoresSBTW[0] = timer;
            timer = 0;
            stopMinigame = true;
        }
    }

    private void HungryCrew()
    {
        timer += Time.deltaTime;

        if (timer >= timeUntilShootingEnds) stopMinigame = true;
    }

    private void KnowYourEnemies()
    {
        timer += Time.deltaTime;
        
        if (timer >= timeUntilMemoryEnds) stopMinigame = true;
    }

    private void SpreadYourSails()
    {
        timer += Time.deltaTime;
    }

    public void ChangeMinigame(Minigame pMinigame)
    {
        _minigame = pMinigame;
        for (int i = 0; i < playerScoresETM.Length; i++) playerScoresETM[i] = 4;
        playerScoresSBTW[0] = 0;
        for (int i = 0; i < playerScoresHC.Length; i++) playerScoresHC[i] = 0;
        for (int i = 0; i < playerScoresKYE.Length; i++) playerScoresKYE[i] = 0;
        playerScoresSYS[0] = 0;
    }

    public Minigame _Minigame
    {
        get { return _minigame; }
    }

    public void ShotTarget(int pPlayer = 0)
    {
        playerScoresHC[pPlayer]++;
    }

    public void EndMinigameTimer()
    {
        switch (_minigame)
        {
            case Minigame.sweptbythewind:
                playerScoresSBTW[0] = timer;
                break;
            case Minigame.spreadyoursails:
                playerScoresSYS[0] = timer;
                break;
            case Minigame.hungrycrew:
                break;
            case Minigame.escapethemonster:
                break;
            case Minigame.knowyourenemies:
                break;
        }
        timer = 0;
        stopMinigame = true;
    }

    public void ReachedTheEnd(int pPlayerNumber)
    {
        for (int i = 0; i < playerScoresETM.Length; i++)
        {
            if (i != pPlayerNumber) playerScoresETM[i]--;
        }

        if (pPlayerNumber == 0)
        {
            stopMinigame = true;
        }
    }

    public void GotDetected(int pPlayerNumber)
    {
        playerScoresETM[pPlayerNumber] = 0;

        if (pPlayerNumber == 0)
        {
            float max = playerScoresETM.Max();
            var indicesWithMaxValue = playerScoresETM
                .Select((value, index) => new { Value = value, Index = index })
                .Where(item => Math.Abs(item.Value - max) < float.Epsilon)
                .Select(item => item.Index);

            int miniGameWinner = Random.Range(0, indicesWithMaxValue.Count());
            int[] indicesArray = indicesWithMaxValue.ToArray();
            playerScoresETM[indicesArray[miniGameWinner]] = 5;
            
            stopMinigame = true;
        }
    }

    public void CorrectShipCombo()
    {
        playerScoresKYE[0]++;
    }
    
    public bool StopMinigame
    {
        get { return stopMinigame; }
    }

    public void StartAnimation()
    {
        startAnimation.Play();
    }
    
    private void WinningScreen()
    {
        if (finishAnimation.state != PlayState.Playing && playingFinishAnimation)
        {
            int winningPLayerId = 0;
            switch (_minigame)
            {
                case Minigame.sweptbythewind:
                    winningPLayerId = playerScoresSBTW.ToList().IndexOf(playerScoresSBTW.Max());
                    break;
                case Minigame.spreadyoursails:
                    winningPLayerId = playerScoresSYS.ToList().IndexOf(playerScoresSYS.Min());
                    break;
                case Minigame.hungrycrew:
                    winningPLayerId = playerScoresHC.ToList().IndexOf(playerScoresHC.Max());
                    break;
                case Minigame.escapethemonster:
                    winningPLayerId = playerScoresETM.ToList().IndexOf(playerScoresETM.Max());
                    break;
                case Minigame.knowyourenemies:
                    winningPLayerId = playerScoresKYE.ToList().IndexOf(playerScoresKYE.Max());
                    break;
            }
            GameManager.instance.gamers[winningPLayerId].seeds += 10;
            if (!audioPlayed)
            {
                audioPlayed = true;
                if (winningPLayerId == 0)
                    AudioManager.instance.PlaySound(winSoundEffect);
                else
                    AudioManager.instance.PlaySound(loseSoundEffect);
            }

            if (winningScreen != null)
            {
                if (!winningScreen.activeSelf) winningScreen.SetActive(true);
                if (winnerText != null) winnerText.SetText("Player " + (winningPLayerId + 1) + " WINS!!");

                timer += Time.deltaTime;

                if (timer >= winningScreenTime && !continueFromWinning)
                {
                    continueFromWinning = true;
                    timer = 0;
                }
                else if (timer >= continueTextToggleTimer && continueFromWinning && continueFromWinningText != null)
                {
                    continueFromWinningText.SetActive(!continueFromWinningText.activeSelf);
                    timer = 0;
                }

                if (Input.touchCount > 0 && continueFromWinning)
                {
                    beginMinigame = false;
                    timer = 0;
                    audioPlayed = false;
                    winningScreen.SetActive(false);
                    continueFromWinningText.SetActive(false);
                    ScenesManager.instance.ChangeScene(0);
                }
            }
        }
        else if (!playingFinishAnimation)
        {
            finishAnimation.Play();
            playingFinishAnimation = true;
        }
    }
}

public enum Minigame
{
    sweptbythewind,
    spreadyoursails,
    hungrycrew,
    escapethemonster,
    knowyourenemies
}