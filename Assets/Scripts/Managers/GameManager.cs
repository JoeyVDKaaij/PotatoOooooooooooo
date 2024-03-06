using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using static UnityEditor.Progress;
using static UnityEngine.GraphicsBuffer;
using Random = UnityEngine.Random;

[System.Serializable]
public struct Player
{
    public GameObject model;

    //public int tile;
    //public int section;

    //ITEMS
    public List<Item> items;
    public int numItems;

    public int seeds;
    public int treasure;
}

[System.Serializable]
public struct Item
{
    public string name;
    public string description;
    public Sprite picture;
    public int price;

    public int id;
}


public class GameManager : MonoBehaviour
{

    public static GameManager instance {  get; private set; }
    

    [Tooltip("Set the Structs that are the gamers.")] 
    public Player[] gamers;

    public Item[] items = new Item[3];


    [SerializeField]
    private int selectedGamer = 0;

    [SerializeField]
    private int turnPhase = 0;

    public int targetingItem = 0;


    public static event Action<int> AdvanceTurnPhase;

    public static event Action<int> UpdateUI;

    public static event Action<int> toggleUI;

    public static event Action<string, int> displayPopup;


    public int turnTimer = 0;


    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SceneManager.LoadScene(1);
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            SceneManager.LoadScene(0);
        }
    }



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
                if (selectedGamer >= gamers.Length)
                {
                    selectedGamer = 0;
                    turnTimer++;
                    Debug.Log("Starting round " + turnTimer);
                }

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

                        displayPopup?.Invoke("Player " + selectedGamer + " dug up a treasure chest!", 3);

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

    public void OpenShop()
    {
        toggleUI?.Invoke(5);
    }

    public void BuyShopItem(int item)
    {
        gamers[selectedGamer].seeds -= items[item].price;

        gamers[selectedGamer].items.Add(items[item]);

        gamers[selectedGamer].numItems++;

        UpdateUI?.Invoke(0);
        toggleUI?.Invoke(-1);

        MapManager.instance.lookingAtShop = false;
        
        //NextTurnPhase();
    }
    public void CancelBuyShopItem()
    {
        toggleUI?.Invoke(-1);
        MapManager.instance.lookingAtShop = false;
        //NextTurnPhase();
    }

    public void NPCBuyItem()
    {
        List<int> NPCBuyableItems = new List<int>();

        for (int i = 0; i < items.Length; i++)
        {
            if (gamers[selectedGamer].seeds >= items[i].price * 1.15f)
            {
                NPCBuyableItems.Add(i);
            }
        }

        if(NPCBuyableItems.Count > 0)
        {
            int itemToBuy = Random.Range(0, NPCBuyableItems.Count);

            BuyShopItem(NPCBuyableItems[itemToBuy]);

            Debug.Log("NPC bought item " + items[NPCBuyableItems[itemToBuy]].name);
            displayPopup?.Invoke("Player " + selectedGamer + " bought " + items[NPCBuyableItems[itemToBuy]].name + " from the store!", 2);

        }
        else
        {
            Debug.Log("NPC too poor LOL");
        }
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



    public void UseItem(int slot)
    {

        int item = gamers[selectedGamer].items[slot].id;

        switch (item)
        {
            //Grappling Hook
            case (0):
                targetingItem = 0;
                toggleUI?.Invoke(4);
                break;
            //Spyglass
            case (1):
                
                break;
            //Magic Pouch
            case (2):
                targetingItem = 2;
                toggleUI?.Invoke(4);
                break;
            //Bottle of juice
            case (3):

                break;
            //Golden Dice
            case (4):

                break;
            //Old Mysterious Map
            case (5):

                break;
            //Cannon
            case (6):
                targetingItem = 6;
                toggleUI?.Invoke(4);
                break;
        }
    }

    public void UseTargetedItem(int target)
    {
        //Debug.Log("SHOOTING");
        //Debug.Log(target);
        //Debug.Log(targetingItem);

        switch (targetingItem)
        {
            //Grappling Hook
            case (0):

                gamers[target].treasure--;
                gamers[selectedGamer].treasure++;

                displayPopup?.Invoke("You stole player " + target + "'s treasure! Shiver me timbers!", 3);

                for (int i = 0; i < 3; i++)
                {
                    if (gamers[selectedGamer].items[i].id == 0)
                    {
                        gamers[selectedGamer].items.RemoveAt(i);
                        break;
                    }
                }
                toggleUI?.Invoke(-1);
                UpdateUI?.Invoke(0);
                break;
                //Magic Pouch
            case (2):
                int stealableCoins = Math.Min(15, gamers[target].seeds);
                int minStolenCoins = Math.Max(1, (stealableCoins / 2));
                int coinsStolen = Random.Range(minStolenCoins, stealableCoins);

                Debug.Log(" max: " + stealableCoins + " min: " + minStolenCoins + " result: " + coinsStolen);
                displayPopup?.Invoke("You stole " + coinsStolen + " coins from player " + target + "!", 3);

                gamers[target].seeds -= coinsStolen;
                gamers[selectedGamer].seeds += coinsStolen;

                for (int i = 0; i < gamers[selectedGamer].items.Count; i++)
                {
                    if (gamers[selectedGamer].items[i].id == 2)
                    {
                        gamers[selectedGamer].items.RemoveAt(i);
                        break;
                    }
                }

                toggleUI?.Invoke(-1);
                UpdateUI?.Invoke(0);
                break;
                //Cannon
            case (6):
                int itemDestroyed = Random.Range(0, gamers[target].numItems);

                Debug.Log(" destroyed item: " + gamers[target].items[itemDestroyed].name);
                displayPopup?.Invoke("You destroyed player " + target + "'s " + gamers[target].items[itemDestroyed].name + "!", 3);

                gamers[target].items.RemoveAt(itemDestroyed);

                for (int i = 0; i < gamers[selectedGamer].items.Count; i++)
                {
                    if (gamers[selectedGamer].items[i].id == 6)
                    {
                        gamers[selectedGamer].items.RemoveAt(i);
                        break;
                    }
                }

                toggleUI?.Invoke(-1);
                UpdateUI?.Invoke(0);
                break;
        }
    }
}
