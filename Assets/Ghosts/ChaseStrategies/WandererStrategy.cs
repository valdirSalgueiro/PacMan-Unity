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

        private Vector2Int tile;


        public void Start(Ghost ghost)
        {
            Debug.Log(ghost.name + " wandering");
            tile = GameManager.GetRandomEdgeTile(Vector2Int.FloorToInt(ghost.body.position / 16));
            getRandomWalkableTile(ghost);
        }

        public bool Chase(Ghost ghost)
        {
            if (ghost.body.position == ghost.warpInPositionVector2)
            {
                ghost.body.position = ghost.warpOutPositionVector2;
                getRandomWalkableTile(ghost);
            }
            else if (ghost.body.position == ghost.warpOutPositionVector2)
            {
                ghost.body.position = ghost.warpInPositionVector2;
                getRandomWalkableTile(ghost);
            }

            if (positions != null)
            {                
                if (ghost.body.position == ghost.target)
                {
                    if (currentPos < positions.Length - 1)
                    {
                        currentPos++;
                        ghost.target = positions[currentPos];
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
            var warpInDist = Vector2.Distance(ghost.body.position, ghost.warpInPositionVector2);
            var warpOutDist = Vector2.Distance(ghost.body.position, ghost.warpOutPositionVector2);
            var tileDist = Vector2.Distance(ghost.body.position, tile * 16);
            Vector2Int goal;
            if (warpInDist > 0 && warpInDist < tileDist)
            {
                goal = Vector2Int.FloorToInt(ghost.warpInPositionVector2 / 16);
            }
            else if (warpOutDist > 0 && warpOutDist < tileDist)
            {
                goal = Vector2Int.FloorToInt(ghost.warpOutPositionVector2 / 16);
            }
            else
            {
                goal = tile;
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
