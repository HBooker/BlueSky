using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class judgement : MonoBehaviour
{
    public float score = 0.0f;
    public float scoreIncrement = 1.0f;
    public GameObject scoreboard;

    private UnityEngine.UI.Text scoreboardText;
    private bool gameStart = false;
    private bool gameOver = false;

    // Start is called before the first frame update
    void Start()
    {
        scoreboardText = scoreboard.GetComponent<UnityEngine.UI.Text>();
    }

    // Update is called once per frame
    void Update()
    {
        // Press Enter to start the game
        if (!gameStart && Input.GetKeyDown(KeyCode.Return))
        {
            gameStart = true;
            GameObject.Find("Island_tiles").BroadcastMessage("StartGame");
            GameObject.Find("the baller").BroadcastMessage("StartGame");
            Object.Destroy(GameObject.Find("instructions").gameObject);
        }

        // Press R to restart the level
        if (gameOver)
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                UnityEngine.SceneManagement.SceneManager.LoadScene("SampleScene");
            }

            return;
        }

        // Accumulate score after the game starts
        if (gameStart)
        {
            score += Time.deltaTime + scoreIncrement;
            scoreIncrement *= 1.0003f;
            scoreboardText.text = string.Format("Score: {0}", (int)score);
        }
    }

    public void GameOver()
    {
        gameOver = true;
    }

    public void ApplyScoreMultiplier()
    {
        score *= 1.05f;
    }
}
