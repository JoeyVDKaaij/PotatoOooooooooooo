using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIScript : MonoBehaviour
{

    [SerializeField]
    private TMP_Text[] seedText;

    [SerializeField]
    private TMP_Text[] treasureText;

    [SerializeField]
    private GameObject turnUI;

    [SerializeField]
    private GameObject Scoreboard;



    private void OnEnable() { GameManager.AdvanceTurnPhase += SwitchingTurn;
                              GameManager.UpdateUI += UpdateUI; }

    private void OnDisable() { GameManager.AdvanceTurnPhase -= SwitchingTurn;
                               GameManager.UpdateUI += UpdateUI; }

    private void OnDestroy() { GameManager.AdvanceTurnPhase -= SwitchingTurn;
                               GameManager.UpdateUI += UpdateUI; }

    private void Start()
    {
        UpdateUI(0);
    }

    public void MovePlayer()
    {
        MapManager.instance.StartMoving(GameManager.instance.SelectedGamer);
    }

    private void SwitchingTurn(int turnPhase)
    {
        //if (transform.childCount > 0)
        //{
        //    for (int i = 0; i < transform.childCount; i++)
        //    {

        turnUI.SetActive(GameManager.instance.SelectedGamer == 0 && turnPhase == 0);

        //    }
        //}
    }

    public void UpdateUI(int input)
    {
        for(int i = 0; i < 4;i++)
        {
            seedText[i].text = GameManager.instance.gamers[i].seeds.ToString();

            treasureText[i].text = GameManager.instance.gamers[i].treasure.ToString();
        }
    }
}
