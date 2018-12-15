using UnityEngine;

namespace Assets.Ghosts.State
{
    class ChaseState : IGhostState
    {
        private float speed = 3f;
        private int strategyIndex = 0;

        public void Start(Ghost ghost)
        {
            Debug.Log(ghost.name + " chase state");
            ghost.chaseStrategies[strategyIndex].Start(ghost);
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
                ghost.chaseStrategies[strategyIndex].Start(ghost);
            }

            var nextP = Vector2.MoveTowards(ghost.body.position, ghost.target, speed);
            ghost.body.MovePosition(nextP);

            return null;
        }
    }
}
