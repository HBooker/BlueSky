using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileGen : MonoBehaviour
{
    public Camera mainCamera;
    public GameObject firstTile;
    public int maxTiles = 5;
    public float scrollSpeed = 0.01f;

    float baseScrollSpeed = 0.01f;
    List<GameObject> tiles;
    string tileName;
    int tileCount = 1;
    bool gameOver = false;

    // Start is called before the first frame update
    void Start()
    {
        tiles = new List<GameObject>();
        tiles.Add(firstTile);
        GameObject tile = firstTile;
        tileName = tile.name;

        while (tiles.Count < maxTiles)
        {
            tile = GenerateTile(tile);
            tiles.Add(tile);
        }
    }

    // Update is called once per frame
    void Update()
    {
        ScrollToLeft();

        GameObject lastTile = tiles[tiles.Count - 1];
        SpriteRenderer sr = lastTile.GetComponent<SpriteRenderer>();

        // When the newest tile enters the view, delete the oldest tile and generate a new tile 
        if (lastTile.transform.position.x + sr.bounds.size.x / 2.0f <= (mainCamera.transform.position.x + (mainCamera.orthographicSize * 2.0f * mainCamera.aspect)) / 2.0f)
        {
            tiles.Add(GenerateTile(lastTile));
            GameObject toDestroy = tiles[0];
            tiles.RemoveAt(0);
            GameObject.Destroy(toDestroy);
        }
    }

    public void ScrollToLeft()
    {
        foreach (GameObject tile in tiles)
        {
            tile.transform.position = Vector3.MoveTowards(tile.transform.position, tile.transform.position + new Vector3(-1.0f, 0.0f), this.scrollSpeed * this.baseScrollSpeed);
        }
    }

    public GameObject GenerateTile(GameObject original)
    {
        Vector3 pos = original.transform.position;
        float width = original.GetComponent<SpriteRenderer>().bounds.size.x;
        Vector3 newPos = pos + new Vector3(width * 0.99f, 0.0f);
        GameObject newObj = Object.Instantiate(original, newPos, Quaternion.identity, this.transform);
        newObj.name = string.Format("{0}{1}", tileName, ++tileCount);
        return newObj;
    }

    public void GameOver()
    {
        this.gameOver = true;
    }
}