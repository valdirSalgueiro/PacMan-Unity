using Assets;
using RoyT.AStar;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System.Linq;
using Grid = RoyT.AStar.Grid;
using Assets.Ghosts.State;
using UnityEngine.UI;
using System;
using Random = UnityEngine.Random;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public Tilemap TileMap;
    public GameObject Pill;
    public GameObject BigPill;
    public GameObject PillContainer;

    public Text ScoreText;
    public Text StatusText;

    static int startX = 15;
    static int startY = 17;

    static int endX = 15;
    static int endY = 14;

    private List<Vector2> BigPillPositions;
    public GameObject BigPillGameObject;

    public static GameManager instance = null;

    private GameObject[] ghostsGameObjects;

    bool isPaused;

    int score;
    int pillsEaten;
    int pillsTotal;
    

    void FixedUpdate()
    {
        if (Time.frameCount % 10 == 0)
        {
            CheckFrightned();
        }
    }

    void CheckFrightned()
    {
        bool frightned = false;
        foreach (GameObject ghostsGameObject in ghostsGameObjects)
        {
            var ghost = ghostsGameObject.GetComponent<Ghost>();
            if (ghost.GetState() is FrightnedState)
                frightned = true;
        }
        if (!frightned)
        {
            SoundManager.instance.Stop(3);
        }
    }

    public void PauseGame(float timer)
    {
        isPaused = true;
        Invoke("UnpauseGame", timer);
    }

    public void UnpauseGame()
    {
        isPaused = false;
    }

    public bool IsPaused()
    {
        return isPaused;
    }

    public void AddScore(int score)
    {
        if (score == 10 || score == 50)
        {
            pillsEaten++;
            if (pillsEaten == pillsTotal)
            {
                PauseGame(10f);
                Victory();
            }
        }

        this.score += score;
        ScoreText.text = "Score: " + this.score;
    }

    public void HideStatus()
    {
        StatusText.enabled = false;
        ScoreText.enabled = true;
    }

    public void GameOver()
    {
        StatusText.text = "game over";
        StatusText.enabled = true;
        Invoke("ResetLevel", 3f);
    }

    void ResetLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    void Victory()
    {
        StatusText.text = "you win!";
        StatusText.enabled = true;
        Invoke("ResetLevel", 3f);
    }

    // Use this for initialization
    void Awake()
    {
        //Check if there is already an instance of SoundManager
        if (instance == null)
            //if not, set it to this.
            instance = this;
        //If instance already exists:
        else if (instance != this)
            //Destroy this, this enforces our singleton pattern so there can only be one instance of SoundManager.
            Destroy(gameObject);

        pillsEaten = 0;
        pillsTotal = 0;
        score = 0;
        ScoreText.text = "Score: " + score;
        StatusText.text = "get ready!";
        ScoreText.enabled = false;
        StatusText.enabled = true;
        PauseGame(3f);
        Invoke("HideStatus", 3f);

        SoundManager.instance.StartMusic();

        ghostsGameObjects = GameObject.FindGameObjectsWithTag("Enemy");

        BigPillPositions = BigPillGameObject.GetComponentsInChildren<Transform>().Select(gameobject => new Vector2(gameobject.transform.position.x / 16, gameobject.transform.position.y / 16)).ToList();

        for (int i = -startX + 1; i < endX - 1; i++)
        {
            for (int j = -startY; j < endY; j++)
            {
                // do not spawn in walls or ghost area
                if (!GameManager.instance.isWallOrGhostArea(TileMap, new Vector2Int(i, j)))
                {
                    // do not spawn between walls
                    if (((i >= -14 && i <= -9) || (i >= 9 && i <= 13)) && ((j >= 0 && j <= 3) || (j >= -6 && j <= -3)))
                        continue;

                    if (((i >= -12 && i <= -9) || (i >= -6 && i <= -4) || (i >= 3 && i <= 5) || (i >= 9 && i <= 11)) && j == 10)
                        continue;

                    if (!BigPillPositions.Contains(new Vector2(i, j)))
                    {
                        var pill = Instantiate(Pill, PillContainer.transform);
                        pill.transform.localPosition = new Vector3(i * 16, j * 16, 0);
                    }
                    else
                    {
                        var pill = Instantiate(BigPill, PillContainer.transform);
                        pill.transform.localPosition = new Vector3(i * 16, j * 16, 0);
                        BigPillPositions.Remove(new Vector2(i, j));
                    }
                    pillsTotal++;
                }
            }
        }
    }

    public GridTiles InitGrid(Tilemap TileMap, bool isGhost)
    {
        GridTiles tiles = new GridTiles();
        tiles.walkableTiles = new List<Vector2Int>();
        RoyT.AStar.Grid grid = new RoyT.AStar.Grid(startX + endX, startY + endY, 1.0f);

        for (int i = -startX; i < endX; i++)
        {
            for (int j = -startY; j < endY; j++)
            {
                if (isGhost)
                {
                    if (GameManager.instance.isWall(TileMap, new Vector2Int(i, j)))
                    {
                        grid.BlockCell(new Position(i + startX, j + startY));
                    }
                    else if(!GameManager.instance.isGhostArea(new Vector2Int(i, j)))
                    {
                        tiles.walkableTiles.Add(new Vector2Int(i, j));
                    }
                }
                else
                {
                    if (GameManager.instance.isWallOrGhostArea(TileMap, new Vector2Int(i, j)))
                    {
                        grid.BlockCell(new Position(i + startX, j + startY));
                    }
                }
            }
        }
        tiles.grid = grid;
        return tiles;
    }

    public void BlockCell(Grid grid, Vector2Int position)
    {
        grid.BlockCell(new Position(position.x + startX, position.y + startY));
    }

    public void UnblockCell(Grid grid, Vector2Int position)
    {
        grid.UnblockCell(new Position(position.x + startX, position.y + startY));
    }

    public Vector2Int getNearestWallkableTile(GridTiles tiles, Vector2 target)
    {
        float min = 1000;
        Vector2Int result = Vector2Int.zero;
        foreach (var tile in tiles.walkableTiles)
        {
            var dist = Vector2.Distance(tile, target);
            if (dist < min)
            {
                //if its the current closest to target
                min = dist;
                result = tile;
            }
        }
        return result;
    }


    public Vector2[] GetPath(RoyT.AStar.Grid grid, Vector2Int posStart, Vector2Int posEnd)
    {
        if (posEnd.x + startX >= 0 && posEnd.x + startX <= startX + endX && posEnd.y + startY >= 0 && posEnd.y + startY <= startY + endY)
        {
            var positions = grid.GetPath(new Position(posStart.x + startX, posStart.y + startY), new Position(posEnd.x + startX, posEnd.y + startY), MovementPatterns.LateralOnly);
            if (positions.Length > 1)
            {
                Vector2[] vectorPositions = new Vector2[positions.Length - 1];
                for (int i = 1; i < positions.Length; i++)
                {
                    vectorPositions[i - 1] = ConvertPosition(positions[i]);
                }
                return vectorPositions;
            }
        }
        return null;
    }

    public Vector2 ConvertPosition(Position position)
    {
        return new Vector2((position.X - startX) * 16, (position.Y - startY) * 16);
    }

    public Vector2Int GetRandomTile(GridTiles tiles, Vector2Int position)
    {
        Vector2Int result = Vector2Int.zero;
        do
        {
            int index = Random.Range(0, tiles.walkableTiles.Count);
            result = tiles.walkableTiles[index];
        }
        while (result == position);
        return result;
    }

    public bool isWall(Tilemap tilemap, Vector2Int pos)
    {
        return tilemap.GetTile(Vector3Int.FloorToInt(new Vector3(pos.x, pos.y, 0))) != null;
    }

    public bool isWallOrGhostArea(Tilemap tilemap, Vector2Int pos)
    {
        return isWall(tilemap, pos) || isGhostArea(pos);
    }

    public bool isGhostArea(Vector2Int pos)
    {
        return (pos.x > -4 && pos.x < 3 && pos.y > -3 && pos.y < 2);
    }
}
