using UnityEngine;
using UnityEngine.UI;

public class ToggleManager : MonoBehaviour
{
    private Toggle _toggle;

    [SerializeField] private AudioClip soundEffect = null;

    private void Start()
    {
        _toggle = GetComponent<Toggle>();
        
        _toggle.onValueChanged.AddListener(OnToggleClick);
    }

    public void OnToggleClick(bool onClick)
    {
        if (soundEffect != null)
            AudioManager.instance.PlaySound(soundEffect);
        else 
            Debug.LogError("No sound effect is attached to the button");
    }
}