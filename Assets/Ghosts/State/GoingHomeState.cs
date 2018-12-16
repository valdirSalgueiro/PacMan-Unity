using UnityEngine;
using System.Linq;
using Assets.Ghosts.ChaseStrategies;

namespace Assets.Ghosts.State
{
    class GoingHomeState : StateCommons, IGhostState
    {
        private Vector2[] positions;
        private int currentPosition = 0;

        public GoingHomeState(Ghost ghost)
        {
            timer = 0;
            speed = 2f;
        }

        public void Exit(Ghost ghost)
        {
        }

        public void Start(Ghost ghost)
        {
            Debug.Log(ghost.name + " going home state ");
            getHomePath(ghost);
        }

        public IGhostState Update(Ghost ghost)
        {
            if (positions != null)
            {
                if (ghost.body.position == ghost.target)
                {

                    if (currentPosition < positions.Length - 1)
                    {
                        currentPosition++;
                        ghost.target = positions[currentPosition];
                    }
                    else
                    {
                        return new DeadState(ghost);
                    }
                }
            }

            base.UpdateCommons(ghost);

            return null;
        }

        private void getHomePath(Ghost ghost)
        {
            var start = Vector2Int.FloorToInt(ghost.body.position / 16);
            positions = GameManager.GetPath(ghost.grid, start, Vector2Int.FloorToInt(ghost.SpawningLocation / 16));
            currentPosition = 0;
            if (positions != null && positions.Count() > 0)
            {
                ghost.target = positions[currentPosition];
            }
        }
    }
}
