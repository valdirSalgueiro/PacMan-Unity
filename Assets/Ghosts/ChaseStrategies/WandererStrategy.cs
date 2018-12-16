using RoyT.AStar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Ghosts.ChaseStrategies
{
    public class WandererStrategy : ChaseCommons, IChaseStrategy
    {
        private Vector2Int tile;

        public void Start(Ghost ghost, Ghost blinky)
        {
            Debug.Log(ghost.name + " wandering");
            tile = GameManager.GetRandomEdgeTile(Vector2Int.FloorToInt(ghost.body.position / 16));
            chasePosition(ghost);
        }

        public bool Chase(Ghost ghost)
        {
            return base.ChaseCommon(ghost);
        }

        private void chasePosition(Ghost ghost)
        {
            base.SetTarget(ghost);
        }

        protected override Vector2Int GetGoal(Ghost ghost)
        {
            return Vector2Int.FloorToInt(tile);
        }
    }
}
