using UnityEngine;
using UnityEngine.SceneManagement;
using System.Xml;
using TMPro;


public class HPBehaviour : MonoBehaviour
{
    public GameObject player;
    private TextMeshProUGUI tmp;

    private void Start()
    {
        tmp = GetComponent<TextMeshProUGUI>();
    }

    void FixedUpdate()
    {
        tmp.text = "HP: " + player.GetComponent<PlayerInteractions>().hp.ToString();
    }
}
