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

    [SerializeField]
    private GameObject[] toggleableUI;


    private void OnEnable()
    {
        GameManager.AdvanceTurnPhase += SwitchingTurn;
        GameManager.UpdateUI += UpdateUI;
        GameManager.toggleUI += ToggleUI;
    }

    private void OnDisable()
    {
        GameManager.AdvanceTurnPhase -= SwitchingTurn;
        GameManager.UpdateUI -= UpdateUI;
        GameManager.toggleUI -= ToggleUI;
    }

    private void OnDestroy()
    {
        GameManager.AdvanceTurnPhase -= SwitchingTurn;
        GameManager.UpdateUI -= UpdateUI;
        GameManager.toggleUI -= ToggleUI;
    }

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
        for (int i = 0; i < 4; i++)
        {
            seedText[i].text = GameManager.instance.gamers[i].seeds.ToString();

            treasureText[i].text = GameManager.instance.gamers[i].treasure.ToString();
        }
    }

    public void ToggleUI(int uiToToggle)
    {
        foreach (GameObject uiElement in toggleableUI)
        {
            uiElement.SetActive(false);
        }

        if (uiToToggle >= 0)
        {
            toggleableUI[uiToToggle].SetActive(true);
        }

        //if(uiToToggle == 0)
        //{
        //    if (GameManager.instance.gamers[GameManager.instance.SelectedGamer].seeds < 20)
        //}
    }
}
