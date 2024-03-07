using UnityEngine;

public class AIShootingScript : MonoBehaviour
{
    private bool anyTargetActive = false;

    private float timer;

    [SerializeField]
    private float shootingDelay = 2;

    // Update is called once per frame
    void Update()
    {
        if (!MinigameManager.instance.StopMinigame)
        {
            if (anyTargetActive) timer += Time.deltaTime;

            int targetedTarget = Random.Range(0, transform.childCount);
            for (int i = 0; i < transform.childCount; i++)
            {
                TargetScript target = transform.GetChild(i).GetComponent<TargetScript>();
                if (target.TargetDown && targetedTarget == i)
                {
                    if (targetedTarget + 1 == transform.childCount)
                        targetedTarget = 0;
                    else targetedTarget++;
                }
                else if (timer >= shootingDelay && targetedTarget == i)
                {
                    target.TargetHit(Random.Range(1, 4));
                    timer = 0;
                    anyTargetActive = false;
                }

                if (!target.TargetDown) anyTargetActive = true;
            }
        }
    }
}
