using UnityEngine;

public class SelfDestructionScript : MonoBehaviour
{
    [Header("Self Destruct Settings")] 
    [SerializeField]
    private float timeUntilDestruction;

    private float timer;
    
    void Update()
    {
        timer += Time.deltaTime;
        
        if (timer >= timeUntilDestruction) Destroy(gameObject);
    }
}
