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
            if (PathFindUtils.WarpGhost(ghost))
                return true;

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
            goal = GetGoal(ghost);
            PathFindUtils.Navigate(ghost, ConsiderWarps, goal, ref positions, ref currentPos);
        }
    }
}
