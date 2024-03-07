using System;
using System.Linq;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class MemoryCheckerScript : MonoBehaviour
{
    [SerializeField]
    private GameObject[] spriteChanges = null;
    [SerializeField]
    private float lookAtAnswerTime = 5;
    [SerializeField]
    private Button[] buttons = null;
    [SerializeField]
    private AudioClip correctSoundEffect = null;
    [SerializeField]
    private AudioClip incorrectSoundEffect = null;
    
    private int[] solution = { 0, 0, 0, 0 };
    
    void Start()
    {
        if (spriteChanges != null)
        {
            RandomizeShip();
        }
    }
    
    public void CheckParts()
    {
        Debug.Log("Checking");
        bool[] spriteCorrect = { false, false, false, false };
        for (int i = 0; i < spriteChanges.Length; i++)
        {
            for (int j = 0; j < spriteChanges[i].transform.childCount; j++)
            {
                if (spriteChanges[i].transform.GetChild(j).gameObject.activeSelf && j == solution[i])
                {
                    spriteCorrect[i] = true;
                }
            }
        }

        if (spriteCorrect.All(value => value))
        {
            MinigameManager.instance.CorrectShipCombo();
            RandomizeShip();
            AudioManager.instance.PlaySound(correctSoundEffect);
        }
        else
        {
            AudioManager.instance.PlaySound(incorrectSoundEffect);
            RandomizeShip();
        }
    }

    private void RandomizeShip()
    {
        StartCoroutine(RandomizeShipParts());
    }

    IEnumerator  RandomizeShipParts()
    {
        Debug.Log("Showing");
        foreach (var button in buttons)
        {
            button.interactable = false;
        }
        
        for (int i = 0; i < spriteChanges.Length; i++)
        {
            solution[i] = Random.Range(0, spriteChanges[i].transform.childCount);
        }
        
        for (int i = 0; i < spriteChanges.Length; i++)
        {
            for (int j = 0; j < spriteChanges[i].transform.childCount; j++)
            {
                spriteChanges[i].transform.GetChild(j).gameObject.SetActive(j == solution[i]);
            }
        }
        
        yield return new WaitForSeconds(lookAtAnswerTime);
        
        Debug.Log("Changing parts");

        int[] randomizeParts = new int[4];
        
        for (int i = 0; i < spriteChanges.Length; i++)
        {
            randomizeParts[i] = Random.Range(0, spriteChanges[i].transform.childCount);
        }
        
        for (int i = 0; i < spriteChanges.Length; i++)
        {
            for (int j = 0; j < spriteChanges[i].transform.childCount; j++)
            {
                spriteChanges[i].transform.GetChild(j).gameObject.SetActive(j == randomizeParts[i]);
            }
            spriteChanges[i].GetComponent<SwapSprites>().UpdateChildId(randomizeParts[i]);
        }
        
        foreach (var button in buttons)
        {
            button.interactable = true;
        }
    }
}
