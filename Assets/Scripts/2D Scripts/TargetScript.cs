using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class TargetScript : MonoBehaviour
{
    [Header("Target Settings")] 
    [SerializeField, Tooltip("Have the GameObject wait until it respawns")]
    private float waitUntilSpawn = 2;

    private float timer = 0;

    private bool targetDown = false;

    private SpriteRenderer sr;

    private void Start()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if (targetDown)
        {
            timer += Time.deltaTime;

            if (timer >= waitUntilSpawn)
            {
                targetDown = false;
                sr.color = Color.white;
                timer = 0;
            }
        }
    }

    public void TargetHit(int pPlayer = 0)
    {
        if (!targetDown)
        {
            targetDown = true;
            sr.color = Color.clear;
            MinigameManager.instance.ShotTarget();
        }
    }

    public bool TargetDown
    {
        get { return targetDown; }
    }
}
