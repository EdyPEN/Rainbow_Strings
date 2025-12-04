using UnityEngine;

public class Note : MonoBehaviour
{
    public GameObject circle;
    public GameObject square;

    public SpriteRenderer spCircle;
    public SpriteRenderer spSquare;

    public MusicPlay.MusicKey key;

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (key == MusicPlay.MusicKey.Idle)
        {
            spCircle.color = Color.white;
            spSquare.color = Color.white;
        }
        else if (key == MusicPlay.MusicKey.Yellow)
        {
            spCircle.color = Color.yellow;
            spSquare.color = Color.yellow;
        }
        else if (key == MusicPlay.MusicKey.Green)
        {
            spCircle.color = Color.green;
            spSquare.color = Color.green;
        }
        else if (key == MusicPlay.MusicKey.Blue)
        {
            spCircle.color = Color.deepSkyBlue;
            spSquare.color = Color.deepSkyBlue;
        }
        else if (key == MusicPlay.MusicKey.Red)
        {
            spCircle.color = Color.red;
            spSquare.color = Color.red;
        }
    }
}
