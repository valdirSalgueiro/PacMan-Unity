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
        private Position[] positions;
        private int currentPos = 0;

        public void Start(Ghost ghost)
        {
            chasePlayer(ghost);
        }

        public void Chase(Ghost ghost)
        {
            if (positions != null)
            {
                ghost.target = GameManager.ConvertPosition(positions[currentPos]);
                if (ghost.body.position == ghost.target)
                {
                    if (currentPos < 3 && currentPos < positions.Length - 1)
                    {
                        currentPos++;
                    }
                    else
                    {
                        chasePlayer(ghost);
                    }
                }
            }
        }

        private void chasePlayer(Ghost ghost)
        {
            var start = Vector2Int.FloorToInt(ghost.body.position / 16);
            var goal = Vector2Int.FloorToInt(ghost.playerBody.position / 16);
            positions = GameManager.GetPath(start, goal);
            currentPos = 0;
        }
    }
}
