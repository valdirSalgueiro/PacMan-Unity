using Assets.Ghosts.ChaseStrategies;
using RoyT.AStar;
using UnityEngine;

namespace Assets.Ghosts.State
{
    class ChaseState : IGhostState
    {
        private float speed = 3f;

        public void Start(Ghost ghost)
        {
            ghost.chaseStrategy.Start(ghost);
        }

        public IGhostState Update(Ghost ghost)
        {
            ghost.chaseStrategy.Chase(ghost);

            var nextP = Vector2.MoveTowards(ghost.body.position, ghost.target, speed);
            ghost.body.MovePosition(nextP);

            return null;
        }
    }
}
