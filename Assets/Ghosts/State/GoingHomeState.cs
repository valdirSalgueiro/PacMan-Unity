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
            if (PathFindUtils.WarpGhost(ghost))
            {
                getHomePath(ghost);
            }

            if (positions != null)
            {
                if (ghost.GetBody().position == ghost.target)
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
            PathFindUtils.Navigate(ghost, true, Vector2Int.FloorToInt(ghost.SpawningLocation / 16), ref positions, ref currentPosition);
        }
    }
}
