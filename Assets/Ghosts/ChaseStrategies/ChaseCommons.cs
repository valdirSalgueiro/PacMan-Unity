using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Ghosts.ChaseStrategies
{
    public abstract class ChaseCommons
    {
        protected Vector2[] positions;
        protected bool Retarget = true;
        protected bool ConsiderWarps = true;
        private int currentPos = 0;
        private Vector2Int lastTilePosition;
        private Vector2Int goal;

        protected abstract Vector2Int GetGoal(Ghost ghost);

        protected bool ChaseCommon(Ghost ghost)
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

            if (positions != null)
            {
                if (ghost.GetBody().position == goal * 16)
                {
                    return true;
                }
                else if (ghost.GetBody().position == ghost.target)
                {
                    var ghostTilePosition = Vector2Int.FloorToInt(ghost.GetBody().position / 16);
                    if (Retarget && lastTilePosition != ghostTilePosition)
                    {
                        SetTarget(ghost);
                        lastTilePosition = ghostTilePosition;
                    }
                    else
                    {
                        if (currentPos < positions.Length - 1)
                        {
                            currentPos++;
                            ghost.target = positions[currentPos];
                        }
                        else
                        {
                            SetTarget(ghost);
                        }
                    }
                }
            }
            else
            {
                SetTarget(ghost);
            }
            return false;
        }

        protected void SetTarget(Ghost ghost)
        {
            var start = Vector2Int.FloorToInt(ghost.GetBody().position / 16);

            var cellToBlock = start - ghost.direction;

            GameManager.BlockCell(ghost.gridTiles.grid, cellToBlock);
            if (ConsiderWarps)
            {
                var warpInDist = GameManager.GetPath(ghost.gridTiles.grid, start, Vector2Int.FloorToInt(ghost.warpInPositionVector2 / 16));
                var warpOutDist = GameManager.GetPath(ghost.gridTiles.grid, start, Vector2Int.FloorToInt(ghost.warpOutPositionVector2 / 16));
                goal = GetGoal(ghost);
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
                goal = GetGoal(ghost);
                positions = GameManager.GetPath(ghost.gridTiles.grid, start, goal);
            }
            GameManager.UnblockCell(ghost.gridTiles.grid, cellToBlock);

            currentPos = 0;
            if (positions != null && positions.Count() > 0)
            {
                ghost.target = positions[currentPos];
            }
        }
    }
}
