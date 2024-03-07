using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
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

    public int protection;

    public Sprite icon;
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

        if (turnTimer >= 20)
        {
            ScenesManager.instance.ChangeScene(8);
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

        Debug.Log(turnPhase);

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

                    for(int i = 0; i < gamers.Length; i++)
                    {
                        if (gamers[i].protection > 0)
                        {
                            gamers[i].protection--;
                        }
                    }

                }

                AdvanceTurnPhase?.Invoke(turnPhase);
                toggleUI?.Invoke(-1);

                if(selectedGamer > 0)
                {
                    NPCTakeTurn();
                }
               

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
                StartMinigame();
                NextTurnPhase();
                break;
            case (TileScript.TileType.Action):
                StartAction();
                //NextTurnPhase();
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

                        displayPopup?.Invoke("Player " + selectedGamer + " dug up a treasure chest! The treasure has moved to a different spot.", 3);

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
        MapManager.instance.SwapTreasureTile();
        if (selectedGamer == 0)
        {
            displayPopup?.Invoke("You dug up a treasure chest! The treasure has moved to a different spot.", 3);
        }
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



    public void StartAction()
    {

        int action = 0;

        if (MapManager.instance.IsPlayerClose(selectedGamer))
        {
            action = Random.Range(0, 3);
        }
        else
        {
            action = Random.Range(0, 4);
        }


        switch(action)
        {
            //coins rain (rare)
            case (0):
                int seedsGained = Random.Range(12, 20);
                displayPopup?.Invoke("Yarr, tonight we feast! Everyone gets " + seedsGained + " seeds!", 4);

                for(int i = 0; i < gamers.Length; i++)
                {
                    gamers[i].seeds += 15;
                }

                Invoke("NextTurnPhase", 2);

                break;
            //Whirlpool
            case (1):
                int seedsLost = Random.Range(4, 8);
                displayPopup?.Invoke("Shiver me timbers, a whirlpool! We lost " + seedsLost + " seeds in the chaos!", 4);

                for (int i = 0; i < gamers.Length; i++)
                {
                    gamers[i].seeds -= Math.Min(gamers[i].seeds, 5);
                }

                Invoke("NextTurnPhase", 2);

                break;
            case (2):
                displayPopup?.Invoke("A storm hit us, seems we all lost control!", 4);

                MapManager.instance.swapPlayers(0, 3);
                MapManager.instance.swapPlayers(1, 2);
                MapManager.instance.swapPlayers(0, Random.Range(1, 3));

                Invoke("NextTurnPhase", 2);

                break;
            case (3):
                int seedsStolen = Random.Range(4, 6);
                int victim = MapManager.instance.FindClosestPlayer(0);

                displayPopup?.Invoke("Well look here, someone dropped something! You took " + seedsStolen + " seeds from player", 4);

                Invoke("NextTurnPhase", 2);

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
                MapManager.instance.SwapTreasureTile();
                displayPopup?.Invoke("Shiver me timbers, the treasure is in a different spot!", 2);
                RemoveItem(1);
                break;
            //Magic Pouch
            case (2):
                targetingItem = 2;
                toggleUI?.Invoke(4);
                break;
            //Bottle of juice
            case (3):
                gamers[selectedGamer].protection = 4;
                displayPopup?.Invoke("You are now protected from the other player's item's effects!", 2);
                RemoveItem(3);
                toggleUI?.Invoke(-1);
                UpdateUI?.Invoke(0);
                break;
            //Golden Dice
            case (4):
                MapManager.instance.doubleDice = true;
                RemoveItem(4);
                toggleUI?.Invoke(-1);
                UpdateUI?.Invoke(0);
                break;
            //Old Mysterious Map
            case (5):
                targetingItem = 5; 
                toggleUI?.Invoke(4);
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

                RemoveItem(0);

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

                RemoveItem(2);

                toggleUI?.Invoke(-1);
                UpdateUI?.Invoke(0);
                break;
            //Map
            case (5):
                MapManager.instance.swapPlayers(selectedGamer, target);

                displayPopup?.Invoke("You swapped positions with player " + target + "!", 3);

                RemoveItem(5);

                toggleUI?.Invoke(-1);
                UpdateUI?.Invoke(0);
                break;
            //Cannon
            case (6):
                int itemDestroyed = Random.Range(0, gamers[target].numItems);

                Debug.Log(" destroyed item: " + gamers[target].items[itemDestroyed].name);
                displayPopup?.Invoke("You destroyed player " + target + "'s " + gamers[target].items[itemDestroyed].name + "!", 3);

                gamers[target].items.RemoveAt(itemDestroyed);

                RemoveItem(6);

                toggleUI?.Invoke(-1);
                UpdateUI?.Invoke(0);
                break;
        }
    }

    private void StartMinigame()
    {
        MinigameManager.instance.ChangeMinigame((Minigame)Random.Range(0, Enum.GetValues(typeof(Minigame)).Length));
        ScenesManager.instance.NextScene();
    }

    void RemoveItem(int itemID)
    {
        for (int i = 0; i < gamers[selectedGamer].items.Count; i++)
        {
            if (gamers[selectedGamer].items[i].id == itemID)
            {
                gamers[selectedGamer].items.RemoveAt(i);
                break;
            }
        }
    }



    public void NPCTakeTurn()
    {

        if (gamers[selectedGamer].numItems > 0)
        {
            bool willUseItem = Random.Range(0, 1) == 0;

            int itemToUse = Random.Range(0, gamers[selectedGamer].numItems);

            if (willUseItem)
            {
                NPCUseItem(itemToUse);
            }
        }

        Invoke("NPCRollDice", 2);
    }


    void NPCUseItem(int slot)
    {

        int item = gamers[selectedGamer].items[slot].id;

        switch (item)
        {
            //Grappling Hook
            case (0):
                NPCUseTargetedItem(0);
                break;
            //Spyglass
            case (1):
                MapManager.instance.SwapTreasureTile();
                displayPopup?.Invoke("Shiver me timbers, the treasure is in a different spot!", 2);
                RemoveItem(1);
                break;
            //Magic Pouch
            case (2):
                NPCUseTargetedItem(2);

                break;
            //Bottle of juice
            case (3):
                gamers[selectedGamer].protection = 4;
                displayPopup?.Invoke("You are now protected from the other player's item's effects!", 2);
                RemoveItem(3);
                toggleUI?.Invoke(-1);
                UpdateUI?.Invoke(0);
                break;
            //Golden Dice
            case (4):
                MapManager.instance.doubleDice = true;
                RemoveItem(4);
                toggleUI?.Invoke(-1);
                UpdateUI?.Invoke(0);
                break;
            //Old Mysterious Map
            case (5):
                NPCUseTargetedItem(5);

                break;
            //Cannon
            case (6):
                NPCUseTargetedItem(6);

                break;
        }
    }

    public void NPCUseTargetedItem(int targetingItem)
    {

        List<int> playersTargetable = new List<int>();
        int target = 0;

        switch (targetingItem)
        {
            //Grappling Hook
            case (0):

                for (int i = 0; i < 4; i++)
                {
                    if (gamers[i].treasure > 0 && gamers[i].protection < 1 && selectedGamer != i)
                        playersTargetable.Add(i);
                }

                if (playersTargetable.Count > 0)
                {
                    target = playersTargetable[Random.Range(0, playersTargetable.Count)];

                    gamers[target].treasure--;
                    gamers[selectedGamer].treasure++;

                    if (target != 0)
                    {
                        displayPopup?.Invoke("Player" + selectedGamer + " stole player " + target + "'s treasure! Shiver me timbers!", 3);
                    }
                    else
                    {
                        displayPopup?.Invoke("Player" + selectedGamer + " stole your treasure! Me timbers have never been shiverin' like this!", 3);
                    }

                    RemoveItem(0);
                }

                toggleUI?.Invoke(-1);
                UpdateUI?.Invoke(0);
                break;
            //Magic Pouch
            case (2):
                for (int i = 0; i < 4; i++)
                {
                    if (gamers[i].seeds > 0 && gamers[i].protection < 1 && selectedGamer != i)
                        playersTargetable.Add(i);
                }

                if (playersTargetable.Count > 0)
                {
                    target = playersTargetable[Random.Range(0, playersTargetable.Count)];
                    int stealableCoins = Math.Min(15, gamers[target].seeds);
                    int minStolenCoins = Math.Max(1, (stealableCoins / 2));
                    int coinsStolen = Random.Range(minStolenCoins, stealableCoins);

                    if (target != 0)
                    {
                        displayPopup?.Invoke("Player " + selectedGamer + " stole " + coinsStolen + " coins from player " + target + "!", 3);
                    }
                    else
                    {
                        displayPopup?.Invoke("Player" + selectedGamer + " stole " + coinsStolen + " from you!", 3);
                    }

                    gamers[target].seeds -= coinsStolen;
                    gamers[selectedGamer].seeds += coinsStolen;

                    RemoveItem(2);
                }

                toggleUI?.Invoke(-1);
                UpdateUI?.Invoke(0);
                break;
            //Map
            case (5):

                for (int i = 0; i < 4; i++)
                {
                    if (gamers[i].protection < 1 && selectedGamer != i)
                        playersTargetable.Add(i);
                }

                if (playersTargetable.Count > 0)
                {
                    target = playersTargetable[Random.Range(0, playersTargetable.Count)];

                    MapManager.instance.swapPlayers(selectedGamer, target);

                    if (target != 0)
                    {
                        displayPopup?.Invoke("Player " + selectedGamer + " swapped positions with player " + target + "!", 3);
                    }
                    else
                    {
                        displayPopup?.Invoke("Player" + selectedGamer + " swapped positions with you!", 3);
                    }

                    RemoveItem(5);
                }

                toggleUI?.Invoke(-1);
                UpdateUI?.Invoke(0);
                break;
            //Cannon
            case (6):

                for (int i = 0; i < 4; i++)
                {
                    if (gamers[i].numItems > 0 && gamers[i].protection < 1 && selectedGamer != i)
                        playersTargetable.Add(i);
                }

                if (playersTargetable.Count > 0)
                {
                    target = playersTargetable[Random.Range(0, playersTargetable.Count)];

                    int itemDestroyed = Random.Range(0, gamers[target].numItems);


                    if (target != 0)
                    {
                        displayPopup?.Invoke("Player " + selectedGamer + " destroyed player " + target + "'s " + gamers[target].items[itemDestroyed].name + " using a cannon!", 4);
                    }
                    else
                    {
                        displayPopup?.Invoke("Player " + selectedGamer + " destroyed your " + gamers[target].items[itemDestroyed].name + " using a cannon! arrr!", 4);
                    }

                    gamers[target].items.RemoveAt(itemDestroyed);

                    RemoveItem(6);
                }

                toggleUI?.Invoke(-1);
                UpdateUI?.Invoke(0);
                break;
        }
    }

    void NPCRollDice()
    {
        MapManager.instance.MoveNPC(turnPhase);
    }

}
