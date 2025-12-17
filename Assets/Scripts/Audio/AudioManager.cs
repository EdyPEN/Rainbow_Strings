using UnityEngine;
using static PauseMenu;

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

    private void Update()
    {
        musicSource.volume = musicVolume / 10f;
    }

    public void PlaySFX(AudioClip clip)
    {
        SFXSource.PlayOneShot(clip, sfxVolume / 10f);
    }
}
