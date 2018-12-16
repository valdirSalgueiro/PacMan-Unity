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
        private int currentPos = 0;
        private Vector2Int lastTilePosition;
        private Vector2Int goal;

        protected abstract Vector2Int GetGoal(Ghost ghost);

        protected bool ChaseCommon(Ghost ghost)
        {
            if (ghost.body.position == ghost.warpInPositionVector2)
            {
                ghost.body.position = ghost.warpOutPositionVector2;
                SetTarget(ghost);
            }
            else if (ghost.body.position == ghost.warpOutPositionVector2)
            {
                ghost.body.position = ghost.warpInPositionVector2;
                SetTarget(ghost);
            }

            if (positions != null)
            {
                if (ghost.body.position == goal)
                {
                    return true;
                }
                else if (ghost.body.position == ghost.target)
                {
                    var ghostTilePosition = Vector2Int.FloorToInt(ghost.body.position / 16);
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
            var start = Vector2Int.FloorToInt(ghost.body.position / 16);

            GameManager.BlockCell(ghost.grid, start - ghost.direction);
            var warpInDist = GameManager.GetPath(ghost.grid, start, Vector2Int.FloorToInt(ghost.warpInPositionVector2 / 16));
            var warpOutDist = GameManager.GetPath(ghost.grid, start, Vector2Int.FloorToInt(ghost.warpOutPositionVector2 / 16));
            goal = GetGoal(ghost);
            var goalDist = GameManager.GetPath(ghost.grid, start, goal);
            positions = null;

            if (warpInDist != null && ((goalDist != null && warpInDist.Count() * 2 < goalDist.Count()) || goalDist == null))
            {
                positions = warpInDist;
            }
            if (warpOutDist != null && ((goalDist != null && warpOutDist.Count() * 2 < goalDist.Count()) || goalDist == null))
            {
                positions = warpOutDist;
            }
            else
            {
                positions = goalDist;
            }

            GameManager.UnblockCell(ghost.grid, start - ghost.direction);
            currentPos = 0;
            if (positions != null && positions.Count() > 0)
            {
                ghost.target = positions[currentPos];
            }
        }
    }
}
