using RoyT.AStar;
using UnityEngine;

namespace Assets.Ghosts.State
{
    class ChaseState : GhostState
    {
        private Position[] positions;
        private int currentPos = 0;
        private float speed = 3f;


        public void update(Ghost ghost)
        {
            if (positions == null)
            {
                chasePlayer(ghost);
            }

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

                var nextP = Vector2.MoveTowards(ghost.body.position, ghost.target, speed);
                ghost.body.MovePosition(nextP);
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
