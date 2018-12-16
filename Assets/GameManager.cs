using Assets;
using RoyT.AStar;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System.Linq;
using Grid = RoyT.AStar.Grid;

public class GameManager : MonoBehaviour
{

    public Tilemap TileMap;
    public GameObject Pill;
    public GameObject BigPill;
    public GameObject PillContainer;

    static int startX = 16;
    static int startY = 17;

    static int endX = 16;
    static int endY = 14;

    private static List<Vector2Int> edgeTiles = new List<Vector2Int>();
    private static List<Vector2Int> nonWallTiles = new List<Vector2Int>();
    private List<Vector2> BigPillPositions;
    public GameObject BigPillGameObject;

    // Use this for initialization
    void Start()
    {

    }

    static public RoyT.AStar.Grid InitGrid(Tilemap TileMap, bool isGhost)
    {
        RoyT.AStar.Grid grid = new RoyT.AStar.Grid(startX + endX, startY + endY, 1.0f);

        for (int i = -startX + 1; i < endX - 1; i++)
        {
            for (int j = -startY; j < endY; j++)
            {
                if (isGhost)
                {
                    if (GameManager.isWall(TileMap, new Vector2Int(i, j)))
                    {
                        grid.BlockCell(new Position(i + startX, j + startY));
                    }
                }
                else
                {
                    if (GameManager.isWallOrGhostArea(TileMap, new Vector2Int(i, j)))
                    {
                        grid.BlockCell(new Position(i + startX, j + startY));
                    }
                }
            }
        }
        return grid;
    }

    static public void BlockCell(Grid grid, Vector2Int position)
    {
        grid.BlockCell(new Position(position.x + startX, position.y + startY));
    }

    static public void UnblockCell(Grid grid, Vector2Int position)
    {
        grid.UnblockCell(new Position(position.x + startX, position.y + startY));
    }

    public static Vector2Int getNearestNonWallTile(Vector2 target)
    {
        float min = 1000;
        Vector2Int result = Vector2Int.zero;
        foreach (var tile in nonWallTiles)
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


    public static Vector2[] GetPath(RoyT.AStar.Grid grid, Vector2Int posStart, Vector2Int posEnd)
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
