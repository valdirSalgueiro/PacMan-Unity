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

    const int startX = 15;
    const int startY = 17;

    const int endX = 14;
    const int endY = 14;

    static RoyT.AStar.Grid grid;

    // Use this for initialization
    void Start()
    {
        // Create a new grid and let each cell have a default traversal cost of 1.0
        grid = new RoyT.AStar.Grid(startX + endX, startY + endY, 1.0f);

        for (int i = -startX; i < endX; i++)
        {
            for (int j = -startY; j < endY; j++)
            {
                // do not spawn between walls
                if (((i >= -14 && i <= -9) || (i >= 9 && i <= 13)) && ((j >= 0 && j <= 3) || (j >= -6 && j <= -3)))
                    continue;

                if (((i >= -12 && i <= -9) || (i >= -6 && i <= -4) || (i >= 3 && i <= 5) || (i >= 9 && i <= 11)) && j == 10)
                    continue;

                // do not spawn in walls or ghost area
                if (!Utils.isWall(TileMap, new Vector2Int(i, j)))
                {
                    if (!Utils.isGhostArea(new Vector2Int(i, j)))
                    {                        
                        var pill = Instantiate(Pill, PillContainer.transform);
                        pill.transform.localPosition = new Vector3(i * 16, j * 16, 0);

                    }
                }
                else
                {
                    grid.BlockCell(new Position(i + startX, j + startY));
                }
            }
        }
    }

    public static Position[] GetPath(Vector2Int posStart, Vector2Int posEnd)
    {
        if (posEnd.x + startX > 0 && posEnd.x + startX < startX + endX && posEnd.y + startY > 0 && posEnd.y + startY < startY + endY)
            return grid.GetPath(new Position(posStart.x + startX, posStart.y + startY), new Position(posEnd.x + startX, posEnd.y + startY), MovementPatterns.LateralOnly);
        else
            return null;
    }

    public static Vector2 ConvertPosition(Position position)
    {
        return new Vector2((position.X - startX) * 16, (position.Y - startY) * 16);
    }
}
