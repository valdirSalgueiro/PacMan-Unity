using RoyT.AStar;
using UnityEngine;

namespace Assets.Ghosts.State
{
    class DeadState : IGhostState
    {
        private Vector2[] positions;
        private int currentPos = 0;
        private float speed = 2f;

        private float deadTimer;

        public DeadState(Ghost ghost, float deadTimer)
        {
            positions = new Vector2[3];
            positions[0] = ghost.SpawningLocation + Vector2.up * 16;
            positions[1] = ghost.SpawningLocation;
            positions[2] = ghost.SpawningLocation + Vector2.down * 16;
            this.deadTimer = deadTimer;
        }

        public void Start(Ghost ghost)
        {
            Debug.Log(ghost.name + " dead state");
        }

        public IGhostState Update(Ghost ghost)
        {
            ghost.target = positions[currentPos];
            if (ghost.body.position == ghost.target)
            {
                if (deadTimer > 0)
                {
                    if (currentPos < positions.Length - 1)
                    {
                        currentPos++;
                    }
                    else
                    {
                        currentPos = 0;
                    }
                }
                else
                {
                    return new ChaseState();
                }
            }

            var nextP = Vector2.MoveTowards(ghost.body.position, ghost.target, speed);
            ghost.body.MovePosition(nextP);

            deadTimer -= Time.deltaTime;

            return null;
        }
    }
}
