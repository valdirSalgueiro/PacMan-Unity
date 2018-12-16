using Assets.Ghosts.ChaseStrategies;
using UnityEngine;

namespace Assets.Ghosts.State
{
    class ChaseState : StateCommons, IGhostState
    {
        private int strategyIndex = 0;
        private Ghost blinky;

        public void Exit(Ghost ghost)
        {
        }

        public void Start(Ghost ghost)
        {
            speed = 2f;
            timer = 20f;

            Debug.Log(ghost.name + " chase state");
            blinky = GameObject.FindObjectOfType<Blinky>();
            ghost.chaseStrategies[strategyIndex].Start(ghost, blinky);
        }

        public IGhostState Update(Ghost ghost)
        {
            if (ghost.chaseStrategies[strategyIndex].Chase(ghost))
            {
                //cycle through strategies
                if (strategyIndex < ghost.chaseStrategies.Count - 1)
                {
                    strategyIndex++;
                }
                else
                {
                    strategyIndex = 0;
                }
                ghost.chaseStrategies[strategyIndex].Start(ghost, blinky);
            }

            if (timer <= 0)
            {
                return new ScatterState(ghost);
            }

            base.UpdateCommons(ghost);

            return null;
        }
    }
}
