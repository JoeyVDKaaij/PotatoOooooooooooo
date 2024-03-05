using System;
using UnityEngine;
using Random = UnityEngine.Random;

[System.Serializable]
public struct Player
{
    public GameObject model;

    //public int tile;
    //public int section;

    //ITEMS

    public int seeds;
    public int treasure;
}


public class GameManager : MonoBehaviour
{

    public static GameManager instance {  get; private set; }
    

    [Tooltip("Set the Structs that are the gamers.")] 
    public Player[] gamers;


    [SerializeField]
    private int selectedGamer = 0;

    [SerializeField]
    private int turnPhase = 0;


    public static event Action<int> AdvanceTurnPhase;

    public static event Action<int> UpdateUI;

    public static event Action<int> toggleUI;


    public int SelectedGamer
    { get { return selectedGamer; } }

    public int TurnPhase
    { get { return turnPhase; } }


    private void Awake()
    {
        if (instance == null)
        {
            instance = this;

            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        Screen.orientation = ScreenOrientation.Portrait;

    }

    private void OnDestroy()
    {
        if (instance == this)
        {
            instance = null;
        }
    }

    //private void ChangeSelectedGamer(int nextPlayer)
    //{
    //    selectedGamer = nextPlayer;
    //}


    public void NextTurnPhase()
    {

        turnPhase++;

        switch(turnPhase)
        {
            //MOVEMENT PHASE
            case (1):

                AdvanceTurnPhase?.Invoke(turnPhase);

            break;
            //TILE PHASE
            case (2):

                AdvanceTurnPhase?.Invoke(turnPhase);

            break;
            //MENU PHASE
            case (3):
                turnPhase = 0;
                selectedGamer++;
                if(selectedGamer >= gamers.Length) selectedGamer = 0;

                AdvanceTurnPhase?.Invoke(turnPhase);

            break;
        }
    }

    public void TileAction(TileScript.TileType type)
    {
        switch(type)
        {
            case(TileScript.TileType.PlusSeeds):
                gamers[selectedGamer].seeds += 3;
                NextTurnPhase();
                break;
            case (TileScript.TileType.MinusSeeds):
                gamers[selectedGamer].seeds -= 3;
                NextTurnPhase();
                break;
            case (TileScript.TileType.Minigame):
                //Minigame
                NextTurnPhase();
                break;
            case (TileScript.TileType.Action):
                //Action
                NextTurnPhase();
                break;
            case (TileScript.TileType.Random):
                TileAction((TileScript.TileType)Random.Range(0, 3));
                break;
            case (TileScript.TileType.Treasure):

                if (selectedGamer == 0)
                {
                    if (gamers[selectedGamer].seeds >= 20)
                    {
                        toggleUI?.Invoke(0);
                    }
                    else
                    {
                        toggleUI?.Invoke(1);
                        Invoke("CancelBuyTreasure", 3);
                    }
                }
                else
                {
                    if (gamers[selectedGamer].seeds >= 20)
                    {
                        BuyTreasure();
                    }
                    else
                    {
                        toggleUI?.Invoke(1);
                        Invoke("CancelBuyTreasure", 3);
                    }
                }
                break;
        }

        UpdateUI?.Invoke(0);

    }

    public void ChangeUIValue()
    {
        UpdateUI?.Invoke(0);
    }

    //public void ToggleUI()
    //{
    //    invoke?.toggleUI
    //}

    public void BuyTreasure()
    {
        //also add moving treasure!
        gamers[selectedGamer].seeds -= 20;
        gamers[selectedGamer].treasure += 1;
        UpdateUI?.Invoke(0);
        toggleUI?.Invoke(-1);
        NextTurnPhase();
    }
    public void CancelBuyTreasure()
    {
        toggleUI?.Invoke(-1);
        NextTurnPhase();
    }

    public void Action(int action)
    {
        switch(action)
        {
            //surprise!
            case (0):

                break;
            //
            case (1):

                break;
        }
    }


}
