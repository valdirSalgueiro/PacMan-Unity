using RoyT.AStar;
using UnityEngine;
using System.Linq;
using System.Collections.Generic;

namespace Assets.Ghosts.State
{
    class GoingHomeState : IGhostState
    {
        private Vector2[] positions;
        private int currentPosition = 0;
        private float speed = 2f;

        public GoingHomeState(Ghost ghost)
        {
        }

        public void Start(Ghost ghost)
        {
            Debug.Log(ghost.name + " going home state ");
            getHomePath(ghost);
        }

        public IGhostState Update(Ghost ghost)
        {
            if (positions != null)
            {
                if (ghost.body.position == ghost.target)
                {

                    if (currentPosition < positions.Length - 1)
                    {
                        currentPosition++;
                        ghost.target = positions[currentPosition];
                    }
                    else
                    {
                        return new DeadState(ghost);
                    }
                }
            }

            var nextP = Vector2.MoveTowards(ghost.body.position, ghost.target, speed);
            ghost.body.MovePosition(nextP);

            return null;
        }

        private void getHomePath(Ghost ghost)
        {
            var start = Vector2Int.FloorToInt(ghost.body.position / 16);
            positions = GameManager.GetPath(start, Vector2Int.FloorToInt(ghost.SpawningLocation / 16));
            currentPosition = 0;
            if (positions != null && positions.Count() > 0)
            {
                ghost.target = positions[currentPosition];
            }
        }
    }
}
