using Assets;
using RoyT.AStar;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GameManager : MonoBehaviour
{

    public Tilemap TileMap;
    public GameObject Pill;
    public GameObject PillContainer;

    static int startX = 15;
    static int startY = 17;

    static int endX = 15;
    static int endY = 14;

    private static RoyT.AStar.Grid grid;

    private static List<Vector2Int> edgeTiles = new List<Vector2Int>();

    // Use this for initialization
    void Start()
    {
        // Create a new grid and let each cell have a default traversal cost of 1.0
        grid = new RoyT.AStar.Grid(startX + endX, startY + endY, 1.0f);

        for (int i = -startX + 1; i < endX - 1; i++)
        {
            for (int j = -startY; j < endY; j++)
            {
                // do not spawn in walls or ghost area
                if (!GameManager.isWall(TileMap, new Vector2Int(i, j)))
                {
                    if (!GameManager.isGhostArea(new Vector2Int(i, j)))
                    {
                        // do not spawn between walls
                        if (((i >= -14 && i <= -9) || (i >= 9 && i <= 13)) && ((j >= 0 && j <= 3) || (j >= -6 && j <= -3)))
                            continue;

                        if (((i >= -12 && i <= -9) || (i >= -6 && i <= -4) || (i >= 3 && i <= 5) || (i >= 9 && i <= 11)) && j == 10)
                            continue;

                        var pill = Instantiate(Pill, PillContainer.transform);
                        pill.transform.localPosition = new Vector3(i * 16, j * 16, 0);

                        // this is to reverse facing on ghosts
                        if (
                            (GameManager.isWall(TileMap, new Vector2Int(i, j - 1)) || (GameManager.isWall(TileMap, new Vector2Int(i, j + 1))))
                            && (GameManager.isWall(TileMap, new Vector2Int(i + 1, j)) && GameManager.isWall(TileMap, new Vector2Int(i - 1, j + 1))))
                        {
                            edgeTiles.Add(new Vector2Int(i, j));
                        }
                    }
                }
                else
                {
                    grid.BlockCell(new Position(i + startX, j + startY));
                }
            }
        }
    }

    public static Vector2[] GetPath(Vector2Int posStart, Vector2Int posEnd)
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

    public static Vector2 ConvertPosition(Position position)
    {
        return new Vector2((position.X - startX) * 16, (position.Y - startY) * 16);
    }

    public static bool isEdgeTile(Vector2Int position)
    {
        return edgeTiles.Contains(position);
    }

    public static Vector2Int GetRandomEdgeTile(Vector2Int position)
    {
        Vector2Int result = Vector2Int.zero;
        do
        {
            int index = Random.Range(0, edgeTiles.Count);
            result = edgeTiles[index];
        }
        while (result == position);
        return result;
    }

    public static bool isWall(Tilemap tilemap, Vector2Int pos)
    {
        return tilemap.GetTile(Vector3Int.FloorToInt(new Vector3(pos.x, pos.y, 0))) != null;
    }

    public static bool isWallOrGhostArea(Tilemap tilemap, Vector2Int pos)
    {
        return isWall(tilemap, pos) || isGhostArea(pos);
    }

    public static bool isGhostArea(Vector2Int pos)
    {
        return (pos.x > -4 && pos.x < 3 && pos.y > -3 && pos.y < 2);
    }
}
