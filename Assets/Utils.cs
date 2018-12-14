using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Assets
{
    class Utils
    {
        public static bool isWall(Tilemap tilemap, Vector2Int pos)
        {
            return tilemap.GetTile(Vector3Int.FloorToInt(new Vector3(pos.x, pos.y, 0))) != null || isGhostArea(pos);
        }

        public static bool isGhostArea(Vector2Int pos)
        {
            return (pos.x > -4 && pos.x < 3 && pos.y > -3 && pos.y < 2);
        }
    }
}
