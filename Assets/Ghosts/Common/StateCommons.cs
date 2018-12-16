using UnityEngine;

namespace Assets.Ghosts.ChaseStrategies
{
    public abstract class StateCommons
    {
        protected float speed;
        protected float timer;

        protected void UpdateCommons(Ghost ghost)
        {
            if(ghost.player.IsDead)
            {
                return;
            }

            if (timer > 0)
            {
                timer -= Time.deltaTime;
            }

            var nextP = Vector2.MoveTowards(ghost.GetBody().position, ghost.target, speed);
            ghost.GetBody().MovePosition(nextP);
        }
    }
}
