using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance {  get; private set; }
    

    [Tooltip("Set the GameObject that are the gamers.")] 
    public GameObject[] gamers;
    
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


        //foreach (GameObject gamer in instance.gamers)
        //    gamer.transform.position = MapManager.instance.MoveTo(0, 0);

    }

    private void OnDestroy()
    {
        if (instance == this)
        {
            instance = null;
        }
    }
}
