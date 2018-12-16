using UnityEngine;

namespace Assets.Ghosts.ChaseStrategies
{
    public abstract class StateCommons
    {
        protected float speed;
        protected float timer;

        protected void UpdateCommons(Ghost ghost)
        {
            if (timer > 0)
            {
                timer -= Time.deltaTime;
            }

            var nextP = Vector2.MoveTowards(ghost.body.position, ghost.target, speed);
            ghost.body.MovePosition(nextP);
        }
    }
}
