using UnityEngine;
using System.Linq;
using System.Collections.Generic;
using Assets.Ghosts.ChaseStrategies;

namespace Assets.Ghosts.State
{
    class ScatterState : StateCommons, IGhostState
    {
        private Vector2[] positions;
        private List<Vector2> ScatterPositions;
        private int currentPosition = 0;
        private int currentScatterPosition = 0;

        public void Exit(Ghost ghost)
        {
        }

        public ScatterState(Ghost ghost)
        {
            speed = 2f;
            ScatterPositions = ghost.GetScatterSpawns().GetComponentsInChildren<Transform>().Select(gameobject => new Vector2(gameobject.transform.position.x, gameobject.transform.position.y)).ToList();
            ScatterPositions = ScatterPositions.Skip(1).ToList();
            this.timer = ghost.GetScatterTime();
            getNextScatter(ghost);
        }

        public void Start(Ghost ghost)
        {
            Debug.Log(ghost.name + " scatter state " + this.timer);
        }

        public IGhostState Update(Ghost ghost)
        {
            if (PathFindUtils.WarpGhost(ghost))
            {
                getNextScatter(ghost);
            }

            if (positions != null)
            {
                if (ghost.GetBody().position == ghost.target)
                {
                    if (timer > 0)
                    {
                        if (currentPosition < positions.Length - 1)
                        {
                            currentPosition++;
                            ghost.target = positions[currentPosition];
                        }
                        else
                        {
                            if (currentScatterPosition < ScatterPositions.Count - 1)
                            {
                                currentScatterPosition++;
                            }
                            else
                            {
                                currentScatterPosition = 0;
                            }
                            getNextScatter(ghost);
                        }
                    }
                    else
                    {
                        return new ChaseState();
                    }
                }
            }

            base.UpdateCommons(ghost);

            return null;
        }

        private void getNextScatter(Ghost ghost)
        {
            PathFindUtils.Navigate(ghost, true, Vector2Int.FloorToInt(ScatterPositions[currentScatterPosition] / 16), ref positions, ref currentPosition);
        }
    }
}
