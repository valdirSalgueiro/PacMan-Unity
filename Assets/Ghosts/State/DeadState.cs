using Assets.Ghosts.ChaseStrategies;
using UnityEngine;

namespace Assets.Ghosts.State
{
    class DeadState : StateCommons, IGhostState
    {
        private Vector2[] positions;
        private int currentPos = 0;

        public void Exit(Ghost ghost)
        {
        }

        public DeadState(Ghost ghost)
        {
            speed = 2f;
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
            if (ghost.GetBody().position == ghost.target)
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
                    //return new ScatterState(ghost);
                    return new ChaseState();
                }
            }

            base.UpdateCommons(ghost);

            return null;
        }
    }
}
