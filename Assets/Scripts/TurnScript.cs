using UnityEngine;

public class TurnScript : MonoBehaviour
{
    [Header("Turn Settings")]
    [SerializeField, Tooltip("Set which player does the actions in this turn"), Range(0, 3)]
    private int currentGamer = 0;
    
    public void Movement()
    {
        MapManager.instance.StartMoving(currentGamer);
        Destroy(gameObject);
    }
}
