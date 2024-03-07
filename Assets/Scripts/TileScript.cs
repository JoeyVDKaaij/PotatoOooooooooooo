using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileScript : MonoBehaviour
{
    
    public enum TileType
    {
        PlusSeeds,
        MinusSeeds,
        Minigame,
        Action,
        Random,
        Treasure,
        Shop,
        Shop1
    }

    public TileType type;

}
