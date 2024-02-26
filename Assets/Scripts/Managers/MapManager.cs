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

    int currentTile;
    int currentSection;

    int playerTurn;
    bool canChoose;

    float timer;
    [SerializeField]
    float delay;
    int stepsLeft;

    [SerializeField]
    TMP_Text text;


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
    
    // Start is called before the first frame update
    void Start()
    {
        foreach (GameObject gamer in GameManager.instance.gamers)
            gamer.transform.position = tileSections[currentSection].tiles[currentTile].transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        timer -= Time.deltaTime;

        if(stepsLeft> 0 && timer < 0)
        {
            if (currentTile < tileSections[currentSection].tiles.Length - 1)
            {
                currentTile++;
                GameManager.instance.gamers[selectedGamer].transform.position = MoveTo(currentSection, currentTile);
            }
            else
            {
                //change this for a selection screen
                currentSection = tileSections[currentSection].connectsTo[Random.Range(0, tileSections[currentSection].connectsTo.Length)];
                currentTile = 0;
                GameManager.instance.gamers[selectedGamer].transform.position = MoveTo(currentSection, currentTile);
            }

            stepsLeft--;
            timer = delay;

        }
    }
    
    public void StartMoving(int pSelectedGamer)
    {
        if (GameManager.instance.gamers.Length > 0)
        {
            pSelectedGamer = Math.Clamp(pSelectedGamer, 0, GameManager.instance.gamers.Length);
            
            selectedGamer = pSelectedGamer;

            stepsLeft = Random.Range(1, 12);

            if (text != null)
                text.text = "rolled: " + stepsLeft.ToString();
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
