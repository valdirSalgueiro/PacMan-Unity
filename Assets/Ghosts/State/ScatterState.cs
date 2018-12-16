using RoyT.AStar;
using UnityEngine;
using System.Linq;
using System.Collections.Generic;

namespace Assets.Ghosts.State
{
    class ScatterState : IGhostState
    {
        private Vector2[] positions;
        private List<Vector2> ScatterPositions;
        private int currentPosition = 0;
        private int currentScatterPosition = 0;
        private float speed = 2f;

        private float timer;

        public ScatterState(Ghost ghost)
        {
            ScatterPositions = ghost.ScatterSpawns.GetComponentsInChildren<Transform>().Select(gameobject => new Vector2(gameobject.transform.position.x, gameobject.transform.position.y)).ToList();
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
            if (positions != null)
            {
                if (ghost.body.position == ghost.target)
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

            var nextP = Vector2.MoveTowards(ghost.body.position, ghost.target, speed);
            ghost.body.MovePosition(nextP);

            timer -= Time.deltaTime;

            return null;
        }

        private void getNextScatter(Ghost ghost)
        {
            var start = Vector2Int.FloorToInt(ghost.body.position / 16);
            positions = GameManager.GetPath(start, Vector2Int.FloorToInt(ScatterPositions[currentScatterPosition] / 16));
            currentPosition = 0;
            if (positions != null && positions.Count() > 0)
            {
                ghost.target = positions[currentPosition];
            }
        }
    }
}
