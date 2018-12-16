using RoyT.AStar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Ghosts.ChaseStrategies
{
    public class ChickenStrategy : ChaseCommons, IChaseStrategy
    {
        private Vector2 corner;

        public void Start(Ghost ghost, Ghost blinky)
        {
            Debug.Log(ghost.name + " chickening");

            chasePosition(ghost);

            var gameObjects = ghost.GetScatterSpawns().GetComponentsInChildren<Transform>().Select(gameobject => new Vector2(gameobject.transform.position.x, gameobject.transform.position.y));
            corner = gameObjects.Skip(2).Take(1).FirstOrDefault();
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
            var playerDist = Vector2.Distance(ghost.GetBody().position, ghost.playerBody.position);
            Retarget = true;
            if (playerDist < 16 * 8)
            {
                Retarget = false;
                return Vector2Int.FloorToInt(corner / 16);
            }
            return Vector2Int.FloorToInt(ghost.playerBody.position / 16);
        }
    }
}
