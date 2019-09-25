using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Island_gen : MonoBehaviour
{
    public Camera mainCamera;
    public GameObject firstColumn;
    public GameObject islandPrefab;
    public int maxColumns = 5;
    public float scrollSpeed = 0.01f;
    public float scrollSpeedRampMultiplier = 1.00007f;
    public float colSpacing = 5.0f;
    public float islandSpacing = 1.0f;

    float baseScrollSpeed = 0.01f;
    List<GameObject> columns;
    string tileName = "Island_tile";
    string colName = "Island_col";
    int tileCount = 1;
    int colCount = 1;
    private bool gameOver = true;
    
    // Start is called before the first frame update
    void Start()
    {
        columns = new List<GameObject>() { firstColumn };
    }

    // Update is called once per frame
    void Update()
    {
        GameObject lastColumn = columns[columns.Count - 1];
        SpriteRenderer sr = lastColumn.GetComponentInChildren<SpriteRenderer>();

        // When the newest island column enters the view, generate a new column and delete the oldest column 
        if (lastColumn.transform.position.x - sr.bounds.size.x / 2.0f <= (mainCamera.transform.position.x + (mainCamera.orthographicSize * 2.0f * mainCamera.aspect)) / 2.0f)
        {
            GameObject newColumn = GenerateColumn(lastColumn);
            columns.Add(newColumn);
            
            if (columns.Count > maxColumns)
            {
                GameObject toDestroy = columns[0];
                columns.RemoveAt(0);
                GameObject.Destroy(toDestroy);
            }
        }

        if (gameOver)
        {
            return;
        }

        ScrollToLeft();
    }

    public void ScrollToLeft()
    {
        foreach (GameObject column in columns)
        {
            column.transform.position = Vector3.MoveTowards(column.transform.position, column.transform.position + new Vector3(-1.0f, 0.0f), this.scrollSpeed);

            // Scroll speed ramps up with time
            this.scrollSpeed *= scrollSpeedRampMultiplier;
        }
    }

    // Generate a new column of islands with a random number of islands and random spacing
    public GameObject GenerateColumn(GameObject original)
    {
        int numIslands = (int)Random.Range(1.0f, 4.0f - float.Epsilon);
        GameObject newColumn = new GameObject(string.Format("{0}{1}", colName, ++colCount));
        newColumn.transform.parent = original.transform.parent;
        newColumn.transform.localPosition = Vector3.zero;

        foreach (Transform t in newColumn.transform)
        {
            // Workaround - refactor later
            GameObject.Destroy(t.gameObject);
        }
        
        Vector3 pos = original.transform.position;
        float width = original.GetComponentInChildren<SpriteRenderer>().bounds.size.x;
        float newXPosition = pos.x + width + colSpacing;
        newColumn.transform.position = new Vector3(newXPosition, pos.y);

        // Determine Y positions of new islands
        float[] newYPositions;
        float middleY = 0.0f;

        switch (numIslands)
        {
            case 1:
                newYPositions = new float[1] { middleY };
                break;
            case 2:
                newYPositions = new float[2] { middleY - islandSpacing / 2.0f, middleY + islandSpacing / 2.0f };
                break;
            case 3:
                newYPositions = new float[3] { middleY - islandSpacing, middleY, middleY + islandSpacing };
                break;
            default:
                throw new System.Exception(string.Format("GenerateColumn: invalid numIslands ({0})", numIslands));
        }

        // Generate X positions and instantiate the new islands
        for (int i = 0; i < numIslands; ++i)
        {
            Vector3 newPos = pos + new Vector3(width + colSpacing + Random.Range(-3.0f, 3.0f), newYPositions[i]);
            GameObject newObj = Object.Instantiate(islandPrefab, newPos, Quaternion.identity, newColumn.transform);
            newObj.name = string.Format("{0}{1}", tileName, ++tileCount);
        }

        return newColumn;
    }

    public void GameOver()
    {
        this.gameOver = true;
    }

    public void Mazeltov()
    {
        this.gameOver = false;
    }
}