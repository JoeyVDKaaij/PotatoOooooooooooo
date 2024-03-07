using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class ButtonManager : MonoBehaviour
{
    private Button _button;

    [SerializeField] private AudioClip soundEffect = null;

    private void Start()
    {
        _button = GetComponent<Button>();
        _button.onClick.AddListener(OnClickHandler);
    }

    public void OnClickHandler()
    {
        if (soundEffect != null)
            AudioManager.instance.PlaySound(soundEffect);
        else 
            Debug.LogError("No sound effect is attached to the button");
    }
}