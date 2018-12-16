using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Ghosts
{
    class PathFindUtils
    {
        public static void Navigate(Ghost ghost, bool ConsiderWarps, Vector2Int goal, ref Vector2[] positions, ref int currentPos)
        {
            var start = Vector2Int.FloorToInt(ghost.GetBody().position / 16);

            var cellToBlock = start - ghost.direction;

            GameManager.BlockCell(ghost.gridTiles.grid, cellToBlock);
            if (ConsiderWarps)
            {
                var warpInDist = GameManager.GetPath(ghost.gridTiles.grid, start, Vector2Int.FloorToInt(ghost.warpInPositionVector2 / 16));
                var warpOutDist = GameManager.GetPath(ghost.gridTiles.grid, start, Vector2Int.FloorToInt(ghost.warpOutPositionVector2 / 16));
                var goalDist = GameManager.GetPath(ghost.gridTiles.grid, start, goal);
                positions = null;

                if (warpInDist != null && ((goalDist != null && warpInDist.Count() * 1.5f < goalDist.Count()) || goalDist == null))
                {
                    positions = warpInDist;
                }
                else if (warpOutDist != null && ((goalDist != null && warpOutDist.Count() * 1.5f < goalDist.Count()) || goalDist == null))
                {
                    positions = warpOutDist;
                }
                else
                {
                    positions = goalDist;
                }
            }
            else
            {
                positions = GameManager.GetPath(ghost.gridTiles.grid, start, goal);
            }
            GameManager.UnblockCell(ghost.gridTiles.grid, cellToBlock);

            currentPos = 0;
            if (positions != null && positions.Count() > 0)
            {
                ghost.target = positions[currentPos];
            }
        }

        public static bool WarpGhost(Ghost ghost)
        {
            if (ghost.GetBody().position == ghost.warpInPositionVector2)
            {
                ghost.GetBody().position = ghost.warpOutPositionVector2 + Vector2Int.left * 16;
                return true;
            }
            else if (ghost.GetBody().position == ghost.warpOutPositionVector2)
            {
                ghost.GetBody().position = ghost.warpInPositionVector2 + Vector2Int.right * 16;
                return true;
            }
            return false;
        }
    }
}
