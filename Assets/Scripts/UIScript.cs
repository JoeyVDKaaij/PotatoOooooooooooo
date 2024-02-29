using System;
using UnityEngine;
using UnityEngine.UI;

public class UIScript : MonoBehaviour
{
    private void OnEnable() { GameManager.AdvanceTurnPhase += SwitchingTurn; }

    private void OnDisable() { GameManager.AdvanceTurnPhase -= SwitchingTurn; }

    private void OnDestroy() { GameManager.AdvanceTurnPhase -= SwitchingTurn; }

    public void MovePlayer()
    {
        MapManager.instance.StartMoving(GameManager.instance.SelectedGamer);
    }

    private void SwitchingTurn(int turnPhase)
    {
        if (transform.childCount > 0)
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                transform.GetChild(i).gameObject.SetActive(GameManager.instance.SelectedGamer == 0 && turnPhase == 0);
            }
        }
    }
}
