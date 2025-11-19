using UnityEngine;

public class Note : MonoBehaviour
{
    public GameObject ColorDisplay;

    public GameObject circle;
    public GameObject square;

    public SpriteRenderer spCircle;
    public SpriteRenderer spSquare;

    public int color;

    private int idle = 0;
    private int blue = 1;
    private int green = 2;
    private int yellow = 3;
    private int red = 4;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (color == idle)
        {
            spCircle.color = Color.black;
            spSquare.color = Color.black;
        }
        else if (color == yellow)
        {
            spCircle.color = Color.yellow;
            spSquare.color = Color.yellow;
        }
        else if (color == green)
        {
            spCircle.color = Color.green;
            spSquare.color = Color.green;
        }
        else if (color == blue)
        {
            spCircle.color = Color.blue;
            spSquare.color = Color.blue;
        }
        else if (color == red)
        {
            spCircle.color = Color.red;
            spSquare.color = Color.red;
        }
    }
}
