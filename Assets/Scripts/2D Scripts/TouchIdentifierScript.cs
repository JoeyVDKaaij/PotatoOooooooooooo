using System;
using UnityEngine;

public class TouchIdentifierScript : MonoBehaviour
{
    Vector3 touchPosWorld;

    void Update() {
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began && !MinigameManager.instance.StopMinigame)
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
}