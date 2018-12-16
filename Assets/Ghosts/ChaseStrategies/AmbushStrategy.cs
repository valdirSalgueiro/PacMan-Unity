using System.Linq;
using UnityEngine;

namespace Assets.Ghosts.ChaseStrategies
{
    public class AmbushStrategy : ChaseCommons, IChaseStrategy
    {
        public void Start(Ghost ghost, Ghost blinky)
        {
            Debug.Log(ghost.name + " ambushing");
            ambushPlayer(ghost);
        }

        public bool Chase(Ghost ghost)
        {
            return base.ChaseCommon(ghost);
        }

        private void ambushPlayer(Ghost ghost)
        {
            base.SetTarget(ghost);
        }

        protected override Vector2Int GetGoal(Ghost ghost)
        {
            int lookAhead = 4;
            Vector2Int goal = Vector2Int.zero;
            do
            {
                var start = Vector2Int.FloorToInt(ghost.body.position / 16);
                goal = Vector2Int.FloorToInt(ghost.playerBody.position / 16 + ghost.player.direction * lookAhead);
                positions = GameManager.GetPath(ghost.grid, start, goal);
                lookAhead -= 1;
            }
            while (positions == null || positions.Count() == 0);
            return goal;
        }
    }
}
