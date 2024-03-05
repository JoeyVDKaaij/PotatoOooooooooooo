using System.Linq;
using UnityEngine;

public class MemoryCheckerScript : MonoBehaviour
{
    [SerializeField]
    private GameObject[] spriteChanges = null;
    
    private int[] solution = { 0, 0, 0, 0 };

    private bool checking = false;
    
    void Start()
    {
        if (spriteChanges != null)
        {
            for (int i = 0; i < spriteChanges.Length; i++)
            {
                solution[i] = Random.Range(0, spriteChanges[i].transform.childCount);
            }
        }
    }
    
    void Update()
    {
        if (checking)
        {
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
                // Code for if the guess is correct.
            }
        }
    }
}
