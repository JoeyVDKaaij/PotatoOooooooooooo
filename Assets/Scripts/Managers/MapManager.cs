using System;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

[System.Serializable]
public struct TileSection
{
    public GameObject[] tiles;
    public GameObject Intersection;
    public int[] connectsTo;
}

public class MapManager : MonoBehaviour
{
    public static MapManager instance { get; private set; }

    [Header("Map Settings")]
    [SerializeField, Tooltip("Set the tiles in groups")]
    private TileSection[] tileSections;

    [SerializeField, Tooltip("Set the dice to roll.")]
    private DiceScript dice;

    int[] currentTile = new int[4];
    int[] currentSection = new int[4];

    int playerTurn;
    bool canChoose;

    float timer;
    int stepsLeft;

    //player movement
    float totalDistance;
    float movedDistance;

    Vector3 movingFrom;
    Vector3 movingTo;
    Vector3 moveAlong;

    bool decidingPath;



    private void OnEnable() { GameManager.AdvanceTurnPhase += MoveNPC; }

    private void OnDisable() { GameManager.AdvanceTurnPhase -= MoveNPC; }


    private void Awake()
    {

        //Start();

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
        GameManager.AdvanceTurnPhase -= StartMoving;

        if (instance == this)
        {
            instance = null;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        foreach (GameObject gamer in GameManager.instance.gamers)
            gamer.transform.position = MoveTo(0, 0);

        movingFrom = MoveTo(0, 0);
    }

    // Update is called once per frame
    void Update()
    {

        if(GameManager.instance.TurnPhase == 1 && !decidingPath)
        {

            movedDistance += Time.deltaTime / 0.2f;

            GameManager.instance.gamers[GameManager.instance.SelectedGamer].transform.position = movingFrom + moveAlong.normalized * movedDistance;

            //Debug.Log(movingFrom);
            //Debug.Log(movingTo);
            //Debug.Log(moveAlong);

            //Debug.Log(totalDistance);
            //Debug.Log(movedDistance);

            //if it is time to step
            if (stepsLeft > 0 && movedDistance >= totalDistance)
            {

                movingFrom = MoveTo(currentSection[GameManager.instance.SelectedGamer], currentTile[GameManager.instance.SelectedGamer]);

                //if the tile is just a tile in the middle of a section (no intersection)
                if (currentTile[GameManager.instance.SelectedGamer] < tileSections[currentSection[GameManager.instance.SelectedGamer]].tiles.Length - 1)
                {
                    currentTile[GameManager.instance.SelectedGamer]++;
                    //GameManager.instance.gamers[GameManager.instance.SelectedGamer].transform.position = MoveTo(currentSection[GameManager.instance.SelectedGamer], currentTile[GameManager.instance.SelectedGamer]);

                    movingTo = MoveTo(currentSection[GameManager.instance.SelectedGamer], currentTile[GameManager.instance.SelectedGamer]);

                }
                //if the player is at an intersection, select a tile to move to
                else
                {
                    //change this for a selection screen
                    currentSection[GameManager.instance.SelectedGamer] = tileSections[currentSection[GameManager.instance.SelectedGamer]]
                        .connectsTo[Random.Range(0, tileSections[currentSection[GameManager.instance.SelectedGamer]].connectsTo.Length)];
                    currentTile[GameManager.instance.SelectedGamer] = 0;

                    //GameManager.instance.gamers[GameManager.instance.SelectedGamer].transform.position = MoveTo(currentSection[GameManager.instance.SelectedGamer], currentTile[GameManager.instance.SelectedGamer]);

                    movingTo = MoveTo(currentSection[GameManager.instance.SelectedGamer], currentTile[GameManager.instance.SelectedGamer]);
                }

                //prepare for next step
                stepsLeft--;
                //timer = delay;

                moveAlong = movingTo - movingFrom;
                totalDistance = Vector3.Magnitude(moveAlong);
                movedDistance = 0.0001f;

                //if the player is out of steps, load the next
                if (stepsLeft <= 0)
                {

                    GameManager.instance.gamers[GameManager.instance.SelectedGamer].transform.position = movingFrom;

                    Debug.Log("anyways, moving on");
                    //add tile actions here
                    GameManager.instance.NextTurnPhase();

                    //if (GameManager.instance.SelectedGamer > 0)
                    //{
                    GameManager.instance.NextTurnPhase();
                    //}

                    totalDistance = 0;

                    //last
                }
            }
        }
    }

    public void StartMoving(int pCurrentPhase)
    {
        if (GameManager.instance.gamers.Length > 0 && pCurrentPhase == 0)
        {
            dice.gameObject.SetActive(true);
            dice.transform.position =
                GameManager.instance.gamers[GameManager.instance.SelectedGamer].transform.position + Vector3.up;
            dice.RollTheDice(result =>
            {
                stepsLeft = result +1;
                dice.gameObject.SetActive(false);
                GameManager.instance.NextTurnPhase();
            });
            
            //if (text != null)
            //    text.text = "rolled: " + stepsLeft.ToString();
        }
        else
        {
            Debug.LogError("No gamers are set in the GameManager!");
        }
    }

    private void MoveNPC(int pCurrentPhase)
    {
        if (GameManager.instance.gamers.Length > 0)
        {
            if (GameManager.instance.SelectedGamer != 0 && pCurrentPhase == 0)
            {

                dice.gameObject.SetActive(true);
                dice.transform.position =
                    GameManager.instance.gamers[GameManager.instance.SelectedGamer].transform.position + Vector3.up;
                dice.RollTheDice(result =>
                {
                    stepsLeft = result + 1;
                    dice.gameObject.SetActive(false);
                    GameManager.instance.NextTurnPhase();
                });

                //if (text != null)
                //    text.text = "rolled: " + stepsLeft.ToString();
            }
        }
        else
        {
            Debug.LogError("No gamers are set in the GameManager!");
        }
    }

    public Vector3 MoveTo(int section, int tile)
    {
        return tileSections[section].tiles[tile].transform.position;
    }
}
