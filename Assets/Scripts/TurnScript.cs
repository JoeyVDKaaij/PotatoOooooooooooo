using System;
using UnityEngine;
using UnityEngine.UI;

public class TurnScript : MonoBehaviour
{
    [Header("Turn Settings")]
    [SerializeField, Tooltip("Set which player does the actions in this turn"), Range(0, 3)]
    private int currentGamer = 0;
    
    private void OnEnable()
    {
        TurnManager.ChangeTurn += SwitchingTurn;
    }

    private void OnDisable()
    {
        TurnManager.ChangeTurn -= SwitchingTurn;
    }

    private void OnDestroy()
    {
        TurnManager.ChangeTurn -= SwitchingTurn;
    }

    public void Movement()
    {
        MapManager.instance.StartMoving(currentGamer);
    }

    private void SwitchingTurn(int pSelectedPlayer)
    {
        if (transform.childCount > 0)
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                transform.GetChild(i).gameObject.SetActive(pSelectedPlayer == 0);
            }
        }
    }
}
