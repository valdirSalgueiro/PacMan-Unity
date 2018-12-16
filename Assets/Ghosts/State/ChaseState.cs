using UnityEngine;

namespace Assets.Ghosts.State
{
    class ChaseState : IGhostState
    {
        private float speed = 2f;
        private int strategyIndex = 0;
        private float timer = 20f;

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

            if (timer <= 0)
            {
                return new ScatterState(ghost);
            }
            timer -= Time.deltaTime;

            var nextP = Vector2.MoveTowards(ghost.body.position, ghost.target, speed);
            ghost.body.MovePosition(nextP);

            return null;
        }
    }
}
