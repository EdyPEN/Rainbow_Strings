using UnityEngine;

public class AudioSkips : MonoBehaviour
{
    public static AudioSkips Instance;
    public AudioSource SFXSource;
    public Transform player;
    public float maxDistance = 5f;

    [Header("AUDIO CLIP")]
    public AudioClip SkipsNoteBlue;
    public AudioClip SkipsNoteGreen;
    public AudioClip SkipsNoteYellow;
    public AudioClip SkipsNoteRed;

    private void Start()
    {
        Instance = this;
        player = GameObject.FindWithTag("Player").transform;
    }

    private void Update()
    {
        float distance = Vector2.Distance(player.position, transform.position);
        float volume = Mathf.Clamp01(1 - (distance / maxDistance));
        SFXSource.volume = volume;
    }

    public void PlaySFX(AudioClip clip)
    {
        // Replace instead of layering
        if (SFXSource.isPlaying && SFXSource.clip == clip) return;

        SFXSource.Stop();
        SFXSource.clip = clip;
        SFXSource.Play();
    }
}
