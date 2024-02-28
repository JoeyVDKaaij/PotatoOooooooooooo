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

    private int selectedGamer = 0;

    int[] currentTile = new int[4];
    int[] currentSection = new int[4];

    int playerTurn;
    bool canChoose;

    float timer;
    [SerializeField]
    float delay;
    int stepsLeft;

    //player movement
    float totalDistance;
    float movedDistance;

    Vector3 movingFrom;
    Vector3 movingTo;
    Vector3 moveAlong;

    [SerializeField]
    TMP_Text text;



    private void OnEnable()
    {
        TurnManager.ChangeTurn += MoveNPC;
    }

    private void OnDisable()
    {
        TurnManager.ChangeTurn -= MoveNPC;
    }

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
        TurnManager.ChangeTurn -= StartMoving;

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
        timer -= Time.deltaTime;

        movedDistance += Time.deltaTime / 0.2f;

        GameManager.instance.gamers[selectedGamer].transform.position = movingFrom + moveAlong.normalized * movedDistance;

        //Debug.Log(movingFrom);
        //Debug.Log(movingTo);
        //Debug.Log(moveAlong);

        //Debug.Log(totalDistance);
        //Debug.Log(movedDistance);

        //if it is time to step
        if (stepsLeft > 0 && movedDistance >= totalDistance)
        {

            movingFrom = MoveTo(currentSection[selectedGamer], currentTile[selectedGamer]);

            //if the tile is just a tile in the middle of a section (no intersection)
            if (currentTile[selectedGamer] < tileSections[currentSection[selectedGamer]].tiles.Length - 1)
            {
                currentTile[selectedGamer]++;
                //GameManager.instance.gamers[selectedGamer].transform.position = MoveTo(currentSection[selectedGamer], currentTile[selectedGamer]);

                movingTo = MoveTo(currentSection[selectedGamer], currentTile[selectedGamer]);

            }
            //if the player is at an intersection, select a tile to move to
            else
            {
                //change this for a selection screen
                currentSection[selectedGamer] = tileSections[currentSection[selectedGamer]].connectsTo[Random.Range(0, tileSections[currentSection[selectedGamer]].connectsTo.Length)];
                currentTile[selectedGamer] = 0;

                //GameManager.instance.gamers[selectedGamer].transform.position = MoveTo(currentSection[selectedGamer], currentTile[selectedGamer]);

                movingTo = MoveTo(currentSection[selectedGamer], currentTile[selectedGamer]);
            }

            //prepare for next step
            stepsLeft--;
            //timer = delay;

            moveAlong = movingTo - movingFrom;
            totalDistance = Vector3.Magnitude(moveAlong);
            movedDistance = 0;

            //if the player is out of steps, load the next
            if (stepsLeft <= 0)
            {
                TurnManager.instance.NextTurnPhase();
                totalDistance = 0;

                //last
            }


        }
    }

    public void StartMoving(int pSelectedGamer)
    {
        if (GameManager.instance.gamers.Length > 0)
        {
            pSelectedGamer = Math.Clamp(pSelectedGamer, 0, GameManager.instance.gamers.Length);

            selectedGamer = pSelectedGamer;

            movingFrom = MoveTo(currentTile[selectedGamer], currentSection[selectedGamer]);

            stepsLeft = Random.Range(1, 12);

            if (text != null)
                text.text = "rolled: " + stepsLeft.ToString();
        }
        else
        {
            Debug.LogError("No gamers are set in the GameManager!");
        }
    }

    private void MoveNPC(int pSelectedGamer)
    {
        if (GameManager.instance.gamers.Length > 0)
        {
            if (pSelectedGamer != 0)
            {
                pSelectedGamer = Math.Clamp(pSelectedGamer, 0, GameManager.instance.gamers.Length);

                selectedGamer = pSelectedGamer;

                stepsLeft = Random.Range(1, 12);

                if (text != null)
                    text.text = "rolled: " + stepsLeft.ToString();
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
