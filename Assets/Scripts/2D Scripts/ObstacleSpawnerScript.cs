using UnityEngine;

public class ObstacleSpawnerScript : MonoBehaviour
{
    [Header("Spawner Settings")] 
    [SerializeField, Tooltip("Set the obstacles that spawn.")]
    private GameObject[] obstacles = null;

    [SerializeField, Tooltip("Set the cooldown after it spawned one or two obstacles.")]
    private float spawnCooldown = 5;

    private float timer = 0;
    
    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= spawnCooldown)
        {
            int random = Random.Range(0,6);

            switch (random)
            {
                case 0:
                    Instantiate(obstacles[random]);
                    break;
                case 1:
                    Instantiate(obstacles[random]);
                    break;
                case 2:
                    Instantiate(obstacles[random]);
                    break;
                case 3:
                    Instantiate(obstacles[0]);
                    Instantiate(obstacles[1]);
                    break;
                case 4:
                    Instantiate(obstacles[1]);
                    Instantiate(obstacles[2]);
                    break;
                case 5:
                    Instantiate(obstacles[0]);
                    Instantiate(obstacles[2]);
                    break;
            }

            timer = 0;
        }
    }
}
