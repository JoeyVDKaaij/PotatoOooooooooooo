using System;
using UnityEngine;
using UnityEngine.UI;

public class TurnScript : MonoBehaviour
{
    private void OnEnable() { TurnManager.ChangeTurn += SwitchingTurn; }

    private void OnDisable() { TurnManager.ChangeTurn -= SwitchingTurn; }

    private void OnDestroy() { TurnManager.ChangeTurn -= SwitchingTurn; }

    public void Movement()
    {
        MapManager.instance.StartMoving(GameManager.instance.SelectedGamer);
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
