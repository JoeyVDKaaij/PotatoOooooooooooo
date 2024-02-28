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
    public static MapManager instance {  get; private set; }

    [Header("Map Settings")]
    [SerializeField, Tooltip("Set the tiles in groups")] 
    private TileSection[] tileSections;

    private int selectedGamer = 0;

    int[] currentTile = new int[4];
    int[] currentSection = new int[4];

    int playerTurn;
    bool canChoose;

    float timer;
    [SerializeField, Tooltip("Set the delay")]
    float delay;
    int stepsLeft;

    [SerializeField, Tooltip("Set the text that shows what you roll")]
    TMP_Text text;

    [SerializeField, Tooltip("Set the dice that is being used")]
    private GameObject dice;

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
        TurnManager.ChangeTurn -= MoveNPC;
        
        if (instance == this)
        {
            instance = null;
        }
    }
    
    // Start is called before the first frame update
    void Start()
    {
        foreach (GameObject gamer in GameManager.instance.gamers)
            gamer.transform.position = tileSections[currentSection[selectedGamer]].tiles[currentTile[selectedGamer]].transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        timer -= Time.deltaTime;

        if(stepsLeft> 0 && timer < 0)
        {
            if (currentTile[selectedGamer] < tileSections[currentSection[selectedGamer]].tiles.Length - 1)
            {
                currentTile[selectedGamer]++;
                GameManager.instance.gamers[selectedGamer].transform.position = MoveTo(currentSection[selectedGamer], currentTile[selectedGamer]);
            }
            else
            {
                //change this for a selection screen
                currentSection[selectedGamer] = tileSections[currentSection[selectedGamer]].connectsTo[Random.Range(0, tileSections[currentSection[selectedGamer]].connectsTo.Length)];
                currentTile[selectedGamer] = 0;
                GameManager.instance.gamers[selectedGamer].transform.position = MoveTo(currentSection[selectedGamer], currentTile[selectedGamer]);
            }

            stepsLeft--;
            timer = delay;

            if (stepsLeft == 0)
                TurnManager.instance.ChangePlayersTurn();
        }
    }
    
    public void StartMoving(int pSelectedGamer)
    {
        if (GameManager.instance.gamers.Length > 0)
        {
            pSelectedGamer = Math.Clamp(pSelectedGamer, 0, GameManager.instance.gamers.Length);
            
            selectedGamer = pSelectedGamer;
            if (dice != null)
            {
                dice.GetComponent<DiceScript>().RollTheDice(result =>
                {
                    stepsLeft = result;
                });
            }
            else stepsLeft = Random.Range(1, 12);

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
        
    private Vector3 MoveTo(int section, int tile)
    {
        return tileSections[section].tiles[tile].transform.position;
    }
}
