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
    [SerializeField, Tooltip("Set how much chance the ai has to move while CheckPhase is asleep."), Range(0,3)]
    private int playerNumber = 0;
    [SerializeField, Tooltip("Set the sound effect for sailing.")]
    private AudioClip soundEffect = null;

    private float timer;
    private bool stopMoving = false;
    private CheckPhase _checkPhase = CheckPhase.sleep;
    
    void Update()
    {
        if (Input.touchCount > 0 && !stopMoving && !controlledByAI  && !MinigameManager.instance.StopMinigame)
        {
            transform.position += new Vector3(0, movementSpeed * Time.deltaTime);
            if (soundEffect != null && AudioManager.instance != null && timer >= soundEffect.length)
            {
                AudioManager.instance.PlaySound(soundEffect);
            }
        }
        else if (!stopMoving && controlledByAI  && !MinigameManager.instance.StopMinigame)
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
        MinigameManager.instance.ReachedTheEnd(playerNumber);
    }
}
