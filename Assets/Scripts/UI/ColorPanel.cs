using System.Drawing;
using UnityEngine;
using UnityEngine.UI;
using static MusicPlay;

public class ColorPanel : MonoBehaviour
{
    public Image Note1, Note2, Note3, Note4;
    public Image String1, String2, String3, String4;

    public GameObject ColorDisplay;
    public MusicKey key;

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
        key = ColorDisplay.GetComponent<MusicPlay>().key;

        ColorLogic(); 
    }
    void ColorLogic()
    {
        if (key == MusicKey.Blue)
        {
            String1.gameObject.SetActive(true);
            String2.gameObject.SetActive(false);
            String3.gameObject.SetActive(false);
            String4.gameObject.SetActive(false);
        }
        else if (key == MusicKey.Green)
        {
            String1.gameObject.SetActive(false);
            String2.gameObject.SetActive(true);
            String3.gameObject.SetActive(false);
            String4.gameObject.SetActive(false);
        }
        else if (key == MusicKey.Yellow)
        {
            String1.gameObject.SetActive(false);
            String2.gameObject.SetActive(false);
            String3.gameObject.SetActive(true);
            String4.gameObject.SetActive(false);
        }
        else if (key == MusicKey.Red)
        {
            String1.gameObject.SetActive(false);
            String2.gameObject.SetActive(false);
            String3.gameObject.SetActive(false);
            String4.gameObject.SetActive(true);
        }
        else
        {
            String1.gameObject.SetActive(false);
            String2.gameObject.SetActive(false);
            String3.gameObject.SetActive(false);
            String4.gameObject.SetActive(false);
        }
    }
}
