using RoyT.AStar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Ghosts.ChaseStrategies
{
    public class WandererStrategy : IChaseStrategy
    {
        private Vector2[] positions;
        private int currentPos = 0;
        private Vector2Int goal;


        public void Start(Ghost ghost)
        {
            Debug.Log(ghost.name + " wandering");
            getRandomWalkableTile(ghost);
        }

        public bool Chase(Ghost ghost)
        {
            if (positions != null)
            {
                ghost.target = positions[currentPos];
                if (ghost.body.position == ghost.target)
                {
                    if (currentPos < positions.Length - 1)
                    {
                        currentPos++;
                    }
                    else
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        private void getRandomWalkableTile(Ghost ghost)
        {
            currentPos = 0;
            var start = Vector2Int.FloorToInt(ghost.body.position / 16);
            goal = GameManager.GetRandomEdgeTile(start);
            positions = GameManager.GetPath(start, goal);
        }
    }
}
