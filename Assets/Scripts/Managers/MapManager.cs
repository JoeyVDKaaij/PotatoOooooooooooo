using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using static System.Collections.Specialized.BitVector32;
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
    [SerializeField]
    float totalDistance;
    [SerializeField]
    float movedDistance;

    Vector3 movingFrom;
    Vector3 movingTo;
    Vector3 moveAlong;

    bool decidingPath;
    public bool lookingAtShop;
    public bool doubleDice;

    [SerializeField]
    private TileScript[] possibleTreasureTiles;

    [SerializeField]
    private Material plusSeedsMat;
    [SerializeField]
    private Material treasureMat;

    [SerializeField, Tooltip("Set the camera.")]
    private Camera mainCamera;


    public static event Action<int> ShowSteps;



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
        foreach (Player gamer in GameManager.instance.gamers)
            gamer.model.transform.position = MoveTo(0, 0);

        movingFrom = MoveTo(0, 0);
    }

    // Update is called once per frame
    void Update()
    {

        //Debug.Log((TileScript.TileType)Random.Range(0, 3));

        //Debug.Log(Camera.main);

        //deciding path check

        if (GameManager.instance.TurnPhase == 1 && !decidingPath && !lookingAtShop)
        {

            movedDistance += Time.deltaTime / 0.2f;

            GameManager.instance.gamers[GameManager.instance.SelectedGamer].model.transform.position = movingFrom + moveAlong.normalized * movedDistance;

            //Debug.Log(stepsLeft);

            //Debug.Log(movingFrom);
            //Debug.Log(movingTo);
            //Debug.Log(moveAlong);

            //Debug.Log(totalDistance);
            //Debug.Log(movedDistance);

            //if it is time to step


            if (stepsLeft > 0 && movedDistance >= totalDistance)
            {

                //Debug.Log("TURN");

                int gamerI = GameManager.instance.SelectedGamer;

                movingFrom = MoveTo(currentSection[gamerI], currentTile[gamerI]);

                //if the tile is just a tile in the middle of a section (no intersection)
                if (currentTile[gamerI] < tileSections[currentSection[gamerI]].tiles.Length - 1)
                {

                    currentTile[gamerI]++;

                    //if the player passes a shop
                    if (tileSections[currentSection[gamerI]].tiles[currentTile[gamerI]].GetComponent<TileScript>().type == TileScript.TileType.Shop)
                    {
                        if (gamerI == 0)
                        {
                            lookingAtShop = true;
                            GameManager.instance.OpenShop();
                        }
                        else
                        {
                            GameManager.instance.NPCBuyItem();
                        }

                        currentTile[gamerI]++;
                    }


                    movingTo = MoveTo(currentSection[gamerI], currentTile[gamerI]);

                }
                //if the player is at an intersection, select a tile to move to
                else
                {
                    //change this for a selection screen

                    if (GameManager.instance.SelectedGamer != 0)
                    {

                        currentSection[gamerI] = tileSections[currentSection[gamerI]].connectsTo[Random.Range(0, tileSections[currentSection[gamerI]].connectsTo.Length)];
                        currentTile[gamerI] = 0;

                        movingTo = MoveTo(currentSection[gamerI], currentTile[gamerI]);

                    }
                    else
                    {
                        if (tileSections[currentSection[gamerI]].connectsTo.Length == 1)
                        {

                            currentSection[gamerI] = tileSections[currentSection[gamerI]].connectsTo[0];
                            currentTile[gamerI] = 0;


                            //currentSection[gamerI] = tileSections[currentSection[gamerI]]
                            //.connectsTo[Random.Range(0, tileSections[currentSection[gamerI]].connectsTo.Length)];
                            //currentTile[gamerI] = 0;

                            movingTo = MoveTo(currentSection[gamerI], currentTile[gamerI]);

                        }
                        else
                        {
                            tileSections[currentSection[gamerI]].Intersection.transform.GetChild(0).gameObject.SetActive(true);
                            Debug.Log("found a crossing!");
                            decidingPath = true;
                            return;
                        }
                    }


                }


                ShowSteps(stepsLeft);

                //prepare for next step
                stepsLeft--;

                moveAlong = movingTo - movingFrom;
                totalDistance = Vector3.Magnitude(moveAlong);
                movedDistance = 0.0001f;

                //Debug.Log(totalDistance);
                //Debug.Log(movedDistance);
            }

            //if the player is out of steps, load the next
            if (stepsLeft <= 0 && movedDistance >= totalDistance)
            {

                GameManager.instance.gamers[GameManager.instance.SelectedGamer].model.transform.position = movingTo;

                Debug.Log("anyways, moving on");
                //add tile actions here
                GameManager.instance.NextTurnPhase();

                ShowSteps(0);

                TileScript.TileType type = tileSections[currentSection[GameManager.instance.SelectedGamer]].tiles[currentTile[GameManager.instance.SelectedGamer]].GetComponent<TileScript>().type;

                Debug.Log(type.ToString());

                GameManager.instance.TileAction(type);

                //if (GameManager.instance.SelectedGamer > 0)
                //{
                //GameManager.instance.NextTurnPhase();
                //}

                movedDistance = 0;
                totalDistance = 0;

                //last
            }
        }
        else if (decidingPath)
        {
            if (Input.GetMouseButtonDown(0))
            {
                RaycastHit hit;
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out hit, 100.0f))
                {

                    int gamerI = GameManager.instance.SelectedGamer;

                    if (hit.transform.gameObject == tileSections[currentSection[gamerI]].Intersection.transform.GetChild(0).GetChild(0).gameObject)
                    {

                        tileSections[currentSection[gamerI]].Intersection.transform.GetChild(0).gameObject.SetActive(false);

                        currentSection[gamerI] = tileSections[currentSection[gamerI]].connectsTo[0];
                        currentTile[gamerI] = 0;

                        movingTo = MoveTo(currentSection[gamerI], currentTile[gamerI]);

                        decidingPath = false;

                        stepsLeft--;

                        moveAlong = movingTo - movingFrom;
                        totalDistance = Vector3.Magnitude(moveAlong);
                        movedDistance = 0.0001f;
                    }
                    else if (hit.transform.gameObject == tileSections[currentSection[GameManager.instance.SelectedGamer]].Intersection.transform.GetChild(0).GetChild(1).gameObject)
                    {

                        tileSections[currentSection[gamerI]].Intersection.transform.GetChild(0).gameObject.SetActive(false);

                        currentSection[gamerI] = tileSections[currentSection[gamerI]].connectsTo[1];
                        currentTile[gamerI] = 0;

                        movingTo = MoveTo(currentSection[gamerI], currentTile[gamerI]);

                        decidingPath = false;

                        stepsLeft--;

                        moveAlong = movingTo - movingFrom;
                        totalDistance = Vector3.Magnitude(moveAlong);
                        movedDistance = 0.0001f;
                    }
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
                GameManager.instance.gamers[GameManager.instance.SelectedGamer].model.transform.position + Vector3.up;

            //during a normal roll
            if (!doubleDice)
            {
                dice.RollTheDice(result =>
                {
                    stepsLeft = result;
                    dice.gameObject.SetActive(false);
                    GameManager.instance.NextTurnPhase();

                    movingFrom = MoveTo(currentSection[GameManager.instance.SelectedGamer], currentTile[GameManager.instance.SelectedGamer]);
                    movingTo = MoveTo(currentSection[GameManager.instance.SelectedGamer], currentTile[GameManager.instance.SelectedGamer]);

                    ShowSteps(result);

                }, mainCamera.transform);
            }
            //during a double roll
            else
            {

                doubleDice = false;

                int totalResult = 0;

                dice.RollTheDice(result =>
                {
                    totalResult += result;

                    dice.transform.position =
                        GameManager.instance.gamers[GameManager.instance.SelectedGamer].model.transform.position + Vector3.up;

                    dice.RollTheDice(result =>
                    {
                        totalResult += result;
                        stepsLeft = totalResult;
                        dice.gameObject.SetActive(false);
                        GameManager.instance.NextTurnPhase();

                        movingFrom = MoveTo(currentSection[GameManager.instance.SelectedGamer], currentTile[GameManager.instance.SelectedGamer]);
                        movingTo = MoveTo(currentSection[GameManager.instance.SelectedGamer], currentTile[GameManager.instance.SelectedGamer]);

                        ShowSteps(totalResult);

                    }, mainCamera.transform);
                }, mainCamera.transform);
            }
            



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
                    GameManager.instance.gamers[GameManager.instance.SelectedGamer].model.transform.position + Vector3.up;
                dice.RollTheDice(result =>
                {
                    stepsLeft = result;
                    dice.gameObject.SetActive(false);
                    GameManager.instance.NextTurnPhase();

                    movingFrom = MoveTo(currentSection[GameManager.instance.SelectedGamer], currentTile[GameManager.instance.SelectedGamer]);
                    movingTo = MoveTo(currentSection[GameManager.instance.SelectedGamer], currentTile[GameManager.instance.SelectedGamer]);

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
        //Debug.Log(tileSections[section].tiles[tile]);

        return tileSections[section].tiles[tile].transform.position;
    }


    public void swapPlayers(int player1, int player2)
    {
        int sec1 = currentSection[player1];
        int sec2 = currentSection[player2];

        int tile1 = currentTile[player1];
        int tile2 = currentTile[player2];

        currentSection[player1] = sec2;
        currentSection[player2] = sec1;

        currentTile[player1] = tile2;
        currentTile[player2] = tile1;

        GameManager.instance.gamers[player1].model.transform.position = MoveTo(currentSection[player1], currentTile[player1]);
        GameManager.instance.gamers[player2].model.transform.position = MoveTo(currentSection[player2], currentTile[player2]);
    }


    public void SwapTreasureTile()
    {
        List<TileScript> tilesAvailable = new List<TileScript>(possibleTreasureTiles);

        TileScript currentTreasureTile = null;

        foreach (TileScript tile in tilesAvailable)
        {
            if (tile.type == TileScript.TileType.Treasure)
            {
                currentTreasureTile = tile;
                tilesAvailable.Remove(tile);
                break;
            }
        }

        int chosenTile = Random.Range(0, tilesAvailable.Count);

        currentTreasureTile.type = TileScript.TileType.PlusSeeds;
        tilesAvailable[chosenTile].type = TileScript.TileType.Treasure;

        currentTreasureTile.transform.GetChild(0).GetComponent<MeshRenderer>().sharedMaterial = plusSeedsMat;
        tilesAvailable[chosenTile].transform.GetChild(0).GetComponent<MeshRenderer>().sharedMaterial = treasureMat;
    }


    public int FindClosestPlayer(int originPlayer)
    {

        int shortestDistance = 10000;
        int closestI = 0;

        for(int i = 0; i < 4; i++)
        {
            if (i != originPlayer)
            {
                if (currentSection[i] == currentSection[originPlayer])
                {
                    if (Math.Abs(currentTile[originPlayer] - currentTile[i]) < shortestDistance)
                    {
                        shortestDistance = currentTile[originPlayer] - currentTile[i];
                        closestI = i;
                    }
                }
            }
        }

        return closestI;

    }

    public bool IsPlayerClose(int originPlayer)
    {
        for (int i = 0; i < 4; i++)
        {
            if (i != originPlayer && currentSection[i] == currentSection[originPlayer])
            {
                return true;
            }
        }

        return false;
    }
}
