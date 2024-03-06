using UnityEngine;

public class MinigameIntroductionScript : MonoBehaviour
{
    [SerializeField, Tooltip("The text that explains the minigames (in the same order as the scenes.)")]
    private GameObject[] explanationText = null;

    private void Start()
    {
        if (explanationText != null && explanationText.Length == 5)
        {
            switch (MinigameManager.instance._Minigame)
            {
                case Minigame.sweptbythewind:
                    for (int i = 0; i < explanationText.Length; i++)
                        explanationText[i].SetActive(i == 0);
                    break;
                case Minigame.spreadyoursails:
                    for (int i = 0; i < explanationText.Length; i++)
                        explanationText[i].SetActive(i == 1);
                    break;
                case Minigame.hungrycrew:
                    for (int i = 0; i < explanationText.Length; i++)
                        explanationText[i].SetActive(i == 2);
                    break;
                case Minigame.escapethemonster:
                    for (int i = 0; i < explanationText.Length; i++)
                        explanationText[i].SetActive(i == 3);
                    break;
                case Minigame.knowyourenemies:
                    for (int i = 0; i < explanationText.Length; i++)
                        explanationText[i].SetActive(i == 4);
                    break;
            }
        }
    }

    public void StartMinigame()
    {
        switch (MinigameManager.instance._Minigame)
        {
            case Minigame.sweptbythewind:
                ScenesManager.instance.ChangeScene(0);
                break;
            case Minigame.spreadyoursails:
                ScenesManager.instance.ChangeScene(0);
                break;
            case Minigame.hungrycrew:
                ScenesManager.instance.ChangeScene(0);
                break;
            case Minigame.escapethemonster:
                ScenesManager.instance.ChangeScene(0);
                break;
            case Minigame.knowyourenemies:
                ScenesManager.instance.ChangeScene(0);
                break;
        }
    }
    
    
}
