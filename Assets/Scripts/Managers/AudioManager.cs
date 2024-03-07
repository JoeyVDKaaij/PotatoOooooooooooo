using UnityEngine;
using UnityEngine.UIElements;

[RequireComponent(typeof(AudioSource))]
public class AudioManager : MonoBehaviour
{
    public static AudioManager instance { get; private set; }
    
    private AudioSource AS = null;
    
    [Header("Audio Settings")]
    [SerializeField, Tooltip("Volume of the game."), Range(0,100)]
    private float _volume = 100;

    private bool muteMusic = false;
    private bool muteSFX = false;
    
    private void Start()
    {
        AS = GetComponent<AudioSource>();
        AS.volume = _volume / 100;
        // AS.Play();
    }

    private void Update()
    {
        if (_volume / 100 != AS.volume)
            AS.volume = _volume / 100;
        
        if (muteMusic) AS.Stop();
        // else if (!muteMusic && !AS.isPlaying) AS.Play();
    }

    public void PlaySound(AudioClip pClip)
    {
        if (!muteSFX)
            AS.PlayOneShot(pClip);
    }

    public void ToggleMusic(bool pMute)
    {
        muteMusic = pMute;
    }

    public void ToggleSFX(bool pMute)
    {
        muteSFX = pMute;
    }
}
