using UnityEngine;
using UnityEngine.UI;

public class MainMenuScript : MonoBehaviour
{
    public Toggle music;
    public Toggle sFX;
    public Toggle vibration;

    private void Start()
    {
        // Subscribe to the onValueChanged event
        music.onValueChanged.AddListener(ToggleMusic);
        sFX.onValueChanged.AddListener(ToggleSFX);
        vibration.onValueChanged.AddListener(ToggleVibration);
    }

    // Don't forget to unsubscribe from the event when the object is destroyed
    private void OnDestroy()
    {
        music.onValueChanged.RemoveListener(ToggleMusic);
        sFX.onValueChanged.RemoveListener(ToggleSFX);
        vibration.onValueChanged.RemoveListener(ToggleVibration);
    }
    
    public void PlayGame()
    {
        ScenesManager.instance.NextScene();
    }

    public void ToggleMusic(bool isOn)
    {
        AudioManager.instance.ToggleMusic(isOn);
    }

    public void ToggleSFX(bool isOn)
    {
        AudioManager.instance.ToggleSFX(isOn);
    }

    public void ToggleVibration(bool isOn)
    {
        Debug.Log("Vibration is " + isOn + " ( \u0361\u00b0 \u035cʖ \u0361\u00b0)");
    }
}
