using UnityEngine;

public class SwapSprites : MonoBehaviour
{
    private int childId = 0;

    private void Start()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(i == 0);
        }
    }

    public void NextSprite()
    {
        transform.GetChild(childId).gameObject.SetActive(false);
        if (childId + 1 == transform.childCount) childId = 0;
        else childId++;
        transform.GetChild(childId).gameObject.SetActive(true);
    }

    public void PreviousSprite()
    {
        transform.GetChild(childId).gameObject.SetActive(false);
        if (childId - 1 == -1) childId = transform.childCount - 1;
        else childId--;
        transform.GetChild(childId).gameObject.SetActive(true);
    }

    public int ChildInt
    {
        get { return childId; }
    }
}
