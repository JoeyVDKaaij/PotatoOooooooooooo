using System;
using UnityEngine;

public class TouchIdentifierScript : MonoBehaviour
{
    Vector3 touchPosWorld;
    [SerializeField, Tooltip("Have the GameObject wait until it respawns")]
    private AudioClip hitSoundEffect = null;
    [SerializeField, Tooltip("Have the GameObject wait until it respawns")]
    private AudioClip missSoundEffect = null;

    void Update() {
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began && !MinigameManager.instance.StopMinigame)
        {

            Ray ray = Camera.main.ScreenPointToRay(Input.touches[0].position);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit)) {
                if (hit.collider.CompareTag("Target"))
                {
                    AudioManager.instance.PlaySound(hitSoundEffect);
                    hit.collider.GetComponent<TargetScript>().TargetHit();
                }
                else AudioManager.instance.PlaySound(missSoundEffect);
            }
        }
    }
}