using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Image = UnityEngine.UI.Image;
using UnityEngine.SceneManagement;
using static UnityEditor.Progress;

[System.Serializable]
public struct ImageList
{
    public Image[] images;
}

public class UIScript : MonoBehaviour
{

    [SerializeField]
    private TMP_Text[] seedText;

    [SerializeField]
    private TMP_Text[] treasureText;

    [SerializeField]
    private ImageList[] playerItemImages;

    [SerializeField]
    private GameObject turnUI;

    [SerializeField]
    private GameObject Scoreboard;

    [SerializeField]
    private GameObject[] toggleableUI;


    [SerializeField]
    private GameObject[] ShopSlots;
    [SerializeField]
    private TMP_Text ShopText;
    [SerializeField]
    private Image ShopImage;
    [SerializeField]
    private Button ShopButton;

    int selectedItem;

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
        InitializeShop();
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

            for (int j = 0; j < 3; j++) 
            {
                if (GameManager.instance.gamers[i].items.Count > j)
                {
                    playerItemImages[i].images[j].sprite = GameManager.instance.gamers[i].items[j].picture;
                }
            }
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

        ShowShopInfo(0);

        //if(uiToToggle == 0)
        //{
        //    if (GameManager.instance.gamers[GameManager.instance.SelectedGamer].seeds < 20)
        //}
    }

    public void InitializeShop()
    {
        for(int i = 0; i < ShopSlots.Length; i++)
        {
            ShopSlots[i].transform.GetChild(0).GetComponent<TMP_Text>().text = GameManager.instance.items[i].name + "\nPrice:" + GameManager.instance.items[i].price;

            ShopSlots[i].transform.GetChild(1).GetComponent<Image>().sprite = GameManager.instance.items[i].picture;
        }
    }

    public void CloseShop()
    {
        GameManager.instance.CancelBuyShopItem();
    }

    public void ShowShopInfo(int item)
    {
        ShopImage.sprite = GameManager.instance.items[item].picture;
        ShopText.text = GameManager.instance.items[item].description;

        if(GameManager.instance.gamers[GameManager.instance.SelectedGamer].seeds < GameManager.instance.items[item].price)
        {
            ShopButton.interactable = false;
        }
        else
        {
            ShopButton.interactable = true;
        }

        selectedItem = item;

    }

    public void BuySelectedItem()
    {
        GameManager.instance.BuyShopItem(selectedItem);
    }
}
