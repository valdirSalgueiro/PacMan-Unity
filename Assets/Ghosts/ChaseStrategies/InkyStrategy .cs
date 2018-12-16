using RoyT.AStar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Ghosts.ChaseStrategies
{
    public class InkyStrategy : IChaseStrategy
    {
        private Vector2[] positions;
        private int currentPos = 0;
        private Vector2Int goal;
        private Ghost blinky;

        private Vector2Int lastTilePosition;

        public void Start(Ghost ghost, Ghost blinky)
        {
            this.blinky = blinky;
            Debug.Log(ghost.name + " inking");
            ambushPlayer(ghost);
        }

        public bool Chase(Ghost ghost)
        {
            if (positions != null)
            {
                ghost.target = positions[currentPos];
                if (ghost.body.position == ghost.target)
                {
                    var ghostTilePosition = Vector2Int.FloorToInt(ghost.body.position / 16);
                    if (lastTilePosition != ghostTilePosition && GameManager.isEdgeTile(ghostTilePosition))
                    {
                        ambushPlayer(ghost);
                        lastTilePosition = ghostTilePosition;
                    }
                    else
                    {
                        if (currentPos < positions.Length - 1)
                        {
                            currentPos++;
                        }
                        else
                        {
                            if (Vector2Int.FloorToInt(ghost.body.position / 16) != goal)
                            {
                                ambushPlayer(ghost);
                            }
                            else
                            {
                                return true;
                            }
                        }
                    }
                }
            }
            return false;
        }

        private void ambushPlayer(Ghost ghost)
        {
            currentPos = 0;
            Vector2 pacmanPosition = ghost.playerBody.position / 16;
            Vector2 blinkyPosition = blinky.body.position / 16;
            Vector2 blinkyToPacman = pacmanPosition - blinkyPosition;

            Vector2 target = pacmanPosition + blinkyToPacman;
            Vector2Int nearestTile = GameManager.getNearestNonWallTile(target);

            var start = Vector2Int.FloorToInt(ghost.body.position / 16);
            positions = GameManager.GetPath(start, nearestTile);
        }
    }
}
