using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance {  get; private set; }
    

    [Tooltip("Set the GameObject that are the gamers.")] 
    public GameObject[] gamers;

    private int selectedGamer = 0;

    public int SelectedGamer
    {
        get { return selectedGamer; }
    }
    
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            // if (transform.parent.gameObject != null) DontDestroyOnLoad(transform.parent.gameObject);
            // else
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        
        TurnManager.ChangeTurn += ChangeSelectedGamer;
    }

    private void OnDestroy()
    {
        if (instance == this)
        {
            instance = null;
        }
        
        TurnManager.ChangeTurn -= ChangeSelectedGamer;
    }

    private void ChangeSelectedGamer(int nextPlayer)
    {
        selectedGamer = nextPlayer;
    }
}
