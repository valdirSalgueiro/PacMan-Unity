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

        private Vector2 warpInPosition;
        private Vector2 warpOutPosition;

        private Vector2 warpInPositionVector2;
        private Vector2 warpOutPositionVector2;

        public void Start(Ghost ghost)
        {
            Debug.Log(ghost.name + " following");

            warpInPositionVector2 = new Vector2(ghost.WarpIn.transform.position.x, ghost.WarpIn.transform.position.y);
            warpOutPositionVector2 = new Vector2(ghost.WarpOut.transform.position.x, ghost.WarpOut.transform.position.y);

            chasePosition(ghost, ghost.playerBody.position / 16);
        }

        public bool Chase(Ghost ghost)
        {
            if (positions == null)
            {
                chasePosition(ghost, ghost.playerBody.position / 16);
            }
            else if (ghost.body.position == ghost.target && ghost.playerBody.position != ghost.body.position)
            {
                var ghostTilePosition = Vector2Int.FloorToInt(ghost.body.position / 16);
                if (lastTilePosition != ghostTilePosition && GameManager.isEdgeTile(ghostTilePosition))
                {
                    if (!positions.Contains(warpInPositionVector2) && !positions.Contains(warpInPositionVector2))
                    {
                        chasePosition(ghost, ghost.playerBody.position / 16);
                    }

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
                        if (ghost.body.position == warpInPositionVector2)
                        {
                            ghost.body.position = warpOutPositionVector2;
                        }
                        else if (ghost.body.position == warpOutPositionVector2)
                        {
                            ghost.body.position = warpInPositionVector2;
                        }
                        chasePosition(ghost, ghost.playerBody.position / 16);                        
                    }
                }
            }
            return false;
        }

        private void chasePosition(Ghost ghost, Vector2 chasePosition)
        {
            var start = Vector2Int.FloorToInt(ghost.body.position / 16);
            var goal = Vector2Int.FloorToInt(chasePosition);
            positions = GameManager.GetPath(start, goal);
            currentPos = 0;
            if (positions != null && positions.Count() > 0)
            {
                ghost.target = positions[currentPos];
            }
        }
    }
}
