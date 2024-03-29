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
                MinigameManager.instance.StartAnimation();
                ScenesManager.instance.ChangeScene(7);
                break;
            case Minigame.spreadyoursails:
                MinigameManager.instance.StartAnimation();
                ScenesManager.instance.ChangeScene(6);
                break;
            case Minigame.hungrycrew:
                MinigameManager.instance.StartAnimation();
                ScenesManager.instance.ChangeScene(4);
                break;
            case Minigame.escapethemonster:
                MinigameManager.instance.StartAnimation();
                ScenesManager.instance.ChangeScene(3);
                break;
            case Minigame.knowyourenemies:
                MinigameManager.instance.StartAnimation();
                ScenesManager.instance.ChangeScene(5);
                break;
        }
    }
    
    
}
