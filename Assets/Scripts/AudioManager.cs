using UnityEngine;

public enum SfxType
{
    tinyClick,
}

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;
    [SerializeField] private AudioSource music;
    [SerializeField] private AudioSource tinyClick;
    public bool canPlayMusic, canPlaySFX;
    
    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        if(canPlayMusic)
            music.Play();   
    }

    public void PlaySFX(SfxType type)
    {
        if (!canPlaySFX)
            return;
        switch (type)
        {
            case SfxType.tinyClick:
                tinyClick.Play();
                tinyClick.pitch = Random.Range(.90f, 1.1f);
                tinyClick.volume = Random.Range(.90f, 1f);
                break;
        }
    }
    
    public void SetMusic(bool canPlay)
    {
        canPlayMusic = canPlay;
        canPlaySFX = canPlay;
        if (canPlayMusic)
            music.Play();
        else
            music.Stop();
    }
}