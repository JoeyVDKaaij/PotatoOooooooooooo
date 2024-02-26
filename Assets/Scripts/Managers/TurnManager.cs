using UnityEngine;

public class TurnManager : MonoBehaviour
{
    public static TurnManager instance {  get; private set; }
    
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            if (transform.parent.gameObject != null) DontDestroyOnLoad(transform.parent.gameObject);
            else DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnDestroy()
    {
        if (instance == this)
        {
            instance = null;
        }
    }
}
