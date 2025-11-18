using System.Drawing;
using UnityEngine;
using UnityEngine.UI;

public class ColorPanel : MonoBehaviour
{
    public Image Note1, Note2, Note3, Note4;
    public Image String1, String2, String3, String4;

    public GameObject ColorDisplay;
    public MusicPlay musicPlay;

    public int color = 0;

    void Start()
    {
        String1.gameObject.SetActive(false);
        String2.gameObject.SetActive(false);
        String3.gameObject.SetActive(false);
        String4.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (musicPlay == null)
        {
            return; // no reference to MusicPlay
        }

        color = musicPlay.color;

        if (color == 1)
        {
            String1.gameObject.SetActive(true);
            String2.gameObject.SetActive(false);
            String3.gameObject.SetActive(false);
            String4.gameObject.SetActive(false);
        }
        else if (color == 2)
        {
            String1.gameObject.SetActive(false);
            String2.gameObject.SetActive(true);
            String3.gameObject.SetActive(false);
            String4.gameObject.SetActive(false);
        }
        else if (color == 3)
        {
            String1.gameObject.SetActive(false);
            String2.gameObject.SetActive(false);
            String3.gameObject.SetActive(true);
            String4.gameObject.SetActive(false);
        }
        else if (color == 4)
        {
            String1.gameObject.SetActive(false);
            String2.gameObject.SetActive(false);
            String3.gameObject.SetActive(false);
            String4.gameObject.SetActive(true);
        } else
        {
            String1.gameObject.SetActive(false);
            String2.gameObject.SetActive(false);
            String3.gameObject.SetActive(false);
            String4.gameObject.SetActive(false);
        }
    }
}
