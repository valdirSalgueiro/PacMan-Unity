using RoyT.AStar;
using UnityEngine;
using System.Linq;
using System.Collections.Generic;
using Assets.Ghosts.ChaseStrategies;

namespace Assets.Ghosts.State
{
    class FrightnedState : StateCommons, IGhostState
    {
        private Vector2[] positions;
        private int currentPosition = 0;


        int flashCount = 0;
        private Shader shaderGUItext;
        private Shader shaderSpritesDefault;

        public void Exit(Ghost ghost)
        {
            ghost.spriteRenderer.material.shader = shaderSpritesDefault;
        }

        public FrightnedState(Ghost ghost)
        {
            speed = 1f;
            timer = 8f;
            shaderGUItext = Shader.Find("GUI/Text Shader");
            shaderSpritesDefault = Shader.Find("Sprites/Default");
        }

        public void Start(Ghost ghost)
        {
            Debug.Log(ghost.name + " frightned state ");
            getNextRandomPath(ghost);
            ghost.spriteRenderer.material.shader = shaderSpritesDefault;
        }

        public IGhostState Update(Ghost ghost)
        {
            if (timer < 2f)
            {
                flashCount++;

                if (Mathf.Floor(flashCount / 5) % 2 == 0)
                {
                    //make it flash white and blue every 30 frames
                    ghost.spriteRenderer.material.shader = shaderGUItext;
                }
                else
                {
                    ghost.spriteRenderer.material.shader = shaderSpritesDefault;
                }
            }

            if (PathFindUtils.WarpGhost(ghost))
            {
                getNextRandomPath(ghost);
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
                            getNextRandomPath(ghost);
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

        private void getNextRandomPath(Ghost ghost)
        {
            var start = Vector2Int.FloorToInt(ghost.GetBody().position / 16);
            PathFindUtils.Navigate(ghost, true, GameManager.instance.GetRandomTile(ghost.gridTiles, start), ref positions, ref currentPosition);
        }
    }
}
