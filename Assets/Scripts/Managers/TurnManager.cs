using System;
using UnityEngine;

public class TurnManager : MonoBehaviour
{
    public static TurnManager instance {  get; private set; }

    private int currentPlayer = 0;
    
    public static event Action<int> ChangeTurn;
    
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
        ChangeTurn += NextPlayerTurnChanges;
    }

    private void OnDestroy()
    {
        ChangeTurn -= NextPlayerTurnChanges;
        if (instance == this)
        {
            instance = null;
        }
    }

    public void ChangePlayersTurn()
    {
        int nextPlayer = currentPlayer + 1;
        if (nextPlayer < GameManager.instance.gamers.Length)
            ChangeTurn?.Invoke(nextPlayer);
        else 
            ChangeTurn?.Invoke(0);
    }

    private void NextPlayerTurnChanges(int pSelectedPlayer)
    {
        currentPlayer = pSelectedPlayer;
    }
}