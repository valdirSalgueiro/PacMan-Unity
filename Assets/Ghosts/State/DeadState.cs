using RoyT.AStar;
using UnityEngine;

namespace Assets.Ghosts.State
{
    class DeadState : IGhostState
    {
        private Vector2[] positions;
        private int currentPos = 0;
        private float speed = 2f;

        private float timer;

        public DeadState(Ghost ghost)
        {
            positions = new Vector2[3];
            positions[0] = ghost.SpawningLocation + Vector2.up * 16;
            positions[1] = ghost.SpawningLocation;
            positions[2] = ghost.SpawningLocation + Vector2.down * 16;
            this.timer = ghost.GetDeadTimer();
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
                if (timer > 0)
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
                    return new ScatterState(ghost);
                }
            }

            var nextP = Vector2.MoveTowards(ghost.body.position, ghost.target, speed);
            ghost.body.MovePosition(nextP);

            timer -= Time.deltaTime;

            return null;
        }
    }
}
