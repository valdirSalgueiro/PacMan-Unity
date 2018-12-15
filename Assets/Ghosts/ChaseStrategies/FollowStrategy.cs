using RoyT.AStar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Ghosts.ChaseStrategies
{
    public class FollowStrategy : IChaseStrategy
    {
        private Vector2[] positions;
        private int currentPos = 0;

        private Vector2Int lastTilePosition;

        
        public void Start(Ghost ghost)
        {
            Debug.Log(ghost.name + " following");
            chasePosition(ghost);
        }

        public bool Chase(Ghost ghost)
        {
            if (ghost.body.position == ghost.warpInPositionVector2)
            {
                ghost.body.position = ghost.warpOutPositionVector2;
                chasePosition(ghost);
            }
            else if (ghost.body.position == ghost.warpOutPositionVector2)
            {
                ghost.body.position = ghost.warpInPositionVector2;
                chasePosition(ghost);
            }

            if (positions != null && ghost.body.position == ghost.target && ghost.playerBody.position != ghost.body.position)
            {
                var ghostTilePosition = Vector2Int.FloorToInt(ghost.body.position / 16);
                if (lastTilePosition != ghostTilePosition && GameManager.isEdgeTile(ghostTilePosition))
                {
                    chasePosition(ghost);
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
                        chasePosition(ghost);
                    }
                }
            }
            return false;
        }

        private void chasePosition(Ghost ghost)
        {
            Vector2Int goal;
            var warpInDist = Vector2.Distance(ghost.body.position, ghost.warpInPositionVector2);
            var warpOutDist = Vector2.Distance(ghost.body.position, ghost.warpOutPositionVector2);
            if (warpInDist > 0 && warpInDist < Vector2.Distance(ghost.body.position, ghost.playerBody.position))
            {
                goal = Vector2Int.FloorToInt(ghost.warpInPositionVector2 / 16);
            }
            else if (warpOutDist > 0 && warpOutDist < Vector2.Distance(ghost.body.position, ghost.playerBody.position))
            {
                goal = Vector2Int.FloorToInt(ghost.warpOutPositionVector2 / 16);
            }
            else
            {
                goal = Vector2Int.FloorToInt(ghost.playerBody.position / 16);
            }

            var start = Vector2Int.FloorToInt(ghost.body.position / 16);

            positions = GameManager.GetPath(start, goal);
            currentPos = 0;
            if (positions != null && positions.Count() > 0)
            {
                ghost.target = positions[currentPos];
            }
        }
    }
}
