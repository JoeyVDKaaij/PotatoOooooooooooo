using Unity.VisualScripting;
using UnityEngine;

public class Movement2DScript : MonoBehaviour
{
    [Header("Movement Settings")] 
    [SerializeField, Tooltip("Set the movement speed in pixel per second")]
    private float movementSpeed = 1;
    [SerializeField, Tooltip("Automate the movement.")]
    private bool controlledByAI = false;
    [SerializeField, Tooltip("Set how much chance the ai has to move while CheckPhase is awake."), Range(0,100)]
    private int chanceOfMovementAwake = 0;
    [SerializeField, Tooltip("Set how much chance the ai has to move while CheckPhase is awakening."), Range(0,100)]
    private int chanceOfMovementAwakening = 50;
    [SerializeField, Tooltip("Set how much chance the ai has to move while CheckPhase is asleep."), Range(0,100)]
    private int chanceOfMovementSleep = 100;

    private bool stopMoving = false;
    private CheckPhase _checkPhase = CheckPhase.sleep;

    private bool endMinigame = false;
    
    private void Start()
    {
        MinigameManager.EndMinigame += EndMinigame;
    }

    private void OnDestroy()
    {
        MinigameManager.EndMinigame -= EndMinigame;
    }
    
    void Update()
    {
        if (Input.touchCount > 0 && !stopMoving && !controlledByAI && !endMinigame)
        {
            transform.position += new Vector3(0, movementSpeed * Time.deltaTime);
        }
        else if (!stopMoving && controlledByAI && !endMinigame)
        {
            int random = Random.Range(1, 101);
            switch (_checkPhase)
            {
                case CheckPhase.awake:
                    if (chanceOfMovementAwake > 0 && random < chanceOfMovementAwake)
                        transform.position += new Vector3(0, movementSpeed * Time.deltaTime);
                    break;
                case CheckPhase.awakening:
                    if (chanceOfMovementAwakening > 0 && random < chanceOfMovementAwakening)
                        transform.position += new Vector3(0, movementSpeed * Time.deltaTime);
                    break;
                case CheckPhase.sleep:
                    if (chanceOfMovementSleep > 0 && random < chanceOfMovementSleep)
                        transform.position += new Vector3(0, movementSpeed * Time.deltaTime);
                    break;
            }
        }
    }

    public void SyncAIWithCheckPhase(CheckPhase pCheckPhase)
    {
        _checkPhase = pCheckPhase;
    }
    
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("Hit the finish line!!!");
        stopMoving = true;
    }

    private void EndMinigame()
    {
        endMinigame = true;
    }
}
