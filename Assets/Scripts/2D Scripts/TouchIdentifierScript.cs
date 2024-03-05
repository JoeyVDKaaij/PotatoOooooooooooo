using System;
using UnityEngine;

public class TouchIdentifierScript : MonoBehaviour
{
    Vector3 touchPosWorld;

    private bool endMinigame;
    
    private void Start()
    {
        MinigameManager.EndMinigame += EndMinigame;
    }

    private void OnDestroy()
    {
        MinigameManager.EndMinigame -= EndMinigame;
    }

    void Update() {
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {

            Ray ray = Camera.main.ScreenPointToRay(Input.touches[0].position);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit)) {
                if (hit.collider.CompareTag("Target"))
                {
                    hit.collider.GetComponent<TargetScript>().TargetHit();
                }
            }
        }
    }

    private void EndMinigame()
    {
        endMinigame = true;
    }
}