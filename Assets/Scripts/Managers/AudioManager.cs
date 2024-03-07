using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AudioManager : MonoBehaviour
{
    public static AudioManager instance { get; private set; }
    
    [SerializeField]
    private AudioSource ASMusic = null;
    [SerializeField]
    private AudioSource ASSFX = null;
    
    [Header("Audio Settings")]
    [SerializeField, Tooltip("Volume of the game."), Range(0,100)]
    private float _volume = 100;

    private bool muteMusic = false;
    private bool muteSFX = false;
    
    private void Awake()
    {
        if (ASMusic != null)
            ASMusic = GetComponent<AudioSource>();
        ASMusic.volume = _volume / 100;
        ASSFX.volume = _volume / 100;
        ASSFX.Play();
        if (instance == null)
        {
            instance = this;
            // if (transform.parent.gameObject != null) DontDestroyOnLoad(transform.parent.gameObject);
            // else 
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnDestroy()
    {
        if (instance == this)
        {
            instance = null;
        }
    }

    private void Update()
    {
        if (_volume / 100 != ASMusic.volume)
            ASMusic.volume = _volume / 100;
        if (_volume / 100 != ASSFX.volume)
            ASSFX.volume = _volume / 100;
        
        if (muteMusic) ASMusic.Stop();
        else if (!muteMusic && !ASMusic.isPlaying) ASMusic.Play();
        
        if (muteSFX) ASSFX.Stop();
    }

    public void PlaySound(AudioClip pClip)
    {
        if (!muteSFX)
            ASSFX.PlayOneShot(pClip);
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
