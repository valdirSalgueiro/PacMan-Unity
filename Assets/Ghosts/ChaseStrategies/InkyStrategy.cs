using RoyT.AStar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Ghosts.ChaseStrategies
{
    public class InkyStrategy : ChaseCommons, IChaseStrategy
    {
        private Ghost blinky;

        public void Start(Ghost ghost, Ghost blinky)
        {
            this.blinky = blinky;
            Debug.Log(ghost.name + " inking");
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
            Vector2 pacmanPosition = ghost.playerBody.position / 16;
            Vector2 blinkyPosition = blinky.GetBody().position / 16;
            Vector2 blinkyToPacman = pacmanPosition - blinkyPosition;

            Vector2 target = pacmanPosition + blinkyToPacman;
            Vector2Int nearestTile = GameManager.getNearestWallkableTile(ghost.gridTiles, target);

            return nearestTile;
        }
    }
}
