using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class deathzone : MonoBehaviour
{
    public float score = 0.0f;
    public float scoreIncrement = 1.0f;
    public GameObject scoreboard;
    public GameObject god;

    private UnityEngine.UI.Text scoreboardText;

    // Start is called before the first frame update
    void Start()
    {
        scoreboardText = scoreboard.GetComponent<UnityEngine.UI.Text>();
    }

    // Update is called once per frame
    void Update()
    {}

    private void OnTriggerEnter2D(Collider2D other)
    {
        Object.Destroy(other.gameObject);
        GameOver();
    }

    private void GameOver()
    {
        GameObject.Find("Background").BroadcastMessage("GameOver");
        GameObject.Find("The Baller").BroadcastMessage("GameOver");
        GameObject.Find("Scoreboard").BroadcastMessage("GameOver");
        GameObject.Find("GameOverText").GetComponent<UnityEngine.UI.Text>().enabled = true;
    }
}
