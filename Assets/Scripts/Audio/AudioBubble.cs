using UnityEngine;
using static MainMenu;

public class AudioBubble : MonoBehaviour
{
    public static AudioBubble Instance;
    public AudioSource SFXSource;
    public Transform player;
    public float maxDistance = 20f;

    [Header("AUDIO CLIP")]
    public AudioClip BubbleNoteBlue;
    public AudioClip BubbleNoteGreen;
    public AudioClip BubbleNoteYellow;
    public AudioClip BubbleNoteRed;

    private void Start()
    {
        Instance = this;
        player = GameObject.FindWithTag("Player").transform;
    }

    private void Update()
    {
        float distance = Vector2.Distance(player.position, transform.position);
        float volume = Mathf.Clamp01(1 - (distance / maxDistance));
        SFXSource.volume = volume * Mathf.Ceil(sfxVolume / 10f);
    }

    public void PlaySFX(AudioClip clip)
    {
        if (SFXSource.isPlaying && SFXSource.clip == clip)
        {
            return;
        }

        SFXSource.Stop();
        SFXSource.clip = clip;
        SFXSource.Play();
    }
}
