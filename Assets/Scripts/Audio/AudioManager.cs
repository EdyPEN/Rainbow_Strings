using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [Header ("AUDIO SOURCE")]
    public AudioSource musicSource;
    public AudioSource SFXSource;

    [Header("AUDIO CLIP")]
    public AudioClip background;

    public AudioClip PlayerNoteBlue;
    public AudioClip PlayerNoteGreen;
    public AudioClip PlayerNoteYellow;
    public AudioClip PlayerNoteRed;

    private void Start()
    {
        musicSource.clip = background;
        musicSource.Play();
    }

    public void PlaySFX(AudioClip clip)
    {
        SFXSource.PlayOneShot(clip);
    }
}
