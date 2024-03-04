using System;
using UnityEngine;

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

        ChangeUIValue();

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

    public void ChangeUIValue()
    {
        UpdateUI?.Invoke(0);
    }

}
