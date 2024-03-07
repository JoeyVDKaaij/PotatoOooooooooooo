using UnityEngine;

public class RestartGameButtonScript : MonoBehaviour
{
    public void RestartGame()
    {
        ScenesManager.instance.ChangeScene(0);
    }
}
