using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.SceneTemplate;
using UnityEngine;

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

    [SerializeField] TileSection[] tileSections;

    [SerializeField] GameObject gamer;

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
            if (transform.parent.gameObject != null) DontDestroyOnLoad(transform.parent.gameObject);
            else DontDestroyOnLoad(gameObject);
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
        gamer.transform.position = tileSections[currentSection].tiles[currentTile].transform.position;
    }

    // Update is called once per frame
    void Update()
    {

        timer -= Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.Space))
        {

            stepsLeft = Random.Range(1, 12);

            text.text = "rolled: " + stepsLeft.ToString();

        }

        if(stepsLeft> 0 && timer < 0)
        {
            if (currentTile < tileSections[currentSection].tiles.Length - 1)
            {
                currentTile++;
                gamer.transform.position = MoveTo(currentSection, currentTile);
            }
            else
            {
                //change this for a selection screen
                currentSection = tileSections[currentSection].connectsTo[Random.Range(0, tileSections[currentSection].connectsTo.Length)];
                currentTile = 0;
                gamer.transform.position = MoveTo(currentSection, currentTile);
            }

            stepsLeft--;
            timer = delay;

        }
    }

    public void hello()
    {
        
    }

    Vector3 MoveTo(int section, int tile)
    {
        return tileSections[section].tiles[tile].transform.position;
    }
}
