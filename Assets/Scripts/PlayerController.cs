using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class balling : MonoBehaviour
{
    Rigidbody2D body;
    public float rollForce = 4.0f;
    public float jumpForce = 20.0f;
    public float jumpCooldown = 2.0f;
    public float maxVelocityX = 5.0f;
    public float ySpin = 1.0f;

    private float jumpCooldownRemaining = 0;
    private bool gameOver = true;

    // Start is called before the first frame update
    void Start()
    {
        body = this.GetComponent<Rigidbody2D>();
    }

    public void OnCollisionEnter2D(Collision2D other)
    {
        if (other.collider.name.StartsWith("Island_tile"))
        {
            GameObject.Find("Scoreboard").BroadcastMessage("ApplyScoreMultiplier");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (gameOver)
        {
            return;
        }

        if (jumpCooldownRemaining > 0.0f)
        {
            jumpCooldownRemaining -= Time.deltaTime;
        }

        if (jumpCooldownRemaining <= 0.0f && Input.GetKeyDown(KeyCode.W))
        {
            body.AddForce(new Vector2(0.0f, jumpForce));
            jumpCooldownRemaining = jumpCooldown;
        }

        if (Input.GetKeyDown(KeyCode.D) && body.velocity.x <= maxVelocityX)
        {
            body.AddForce(new Vector2(jumpForce, -ySpin));
        }

        if (Input.GetKeyDown(KeyCode.A) && body.velocity.x >= -maxVelocityX)
        {
            body.AddForce(new Vector2(-jumpForce, ySpin));
        }
    }

    public void GameOver()
    {
        this.gameOver = true;
    }

    public void StartGame()
    {
        this.gameOver = false;
    }
}
