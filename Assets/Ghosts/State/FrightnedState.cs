using RoyT.AStar;
using UnityEngine;
using System.Linq;
using System.Collections.Generic;

namespace Assets.Ghosts.State
{
    class FrightnedState : IGhostState
    {
        private Vector2[] positions;
        private int currentPosition = 0;
        private float speed = 1f;

        private float timer = 8f;

        int flashCount = 0;
        private Shader shaderGUItext;
        private Shader shaderSpritesDefault;

        public FrightnedState(Ghost ghost)
        {
            shaderGUItext = Shader.Find("GUI/Text Shader");
            shaderSpritesDefault = Shader.Find("Sprites/Default");
        }

        public void Start(Ghost ghost)
        {
            Debug.Log(ghost.name + " frightned state ");
            getNextRandomPath(ghost);
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
                            getNextRandomPath(ghost);
                        }
                    }
                    else
                    {
                        ghost.spriteRenderer.material.shader = shaderSpritesDefault;
                        return new ChaseState();
                    }
                }
            }

            var nextP = Vector2.MoveTowards(ghost.body.position, ghost.target, speed);
            ghost.body.MovePosition(nextP);

            timer -= Time.deltaTime;

            return null;
        }

        private void getNextRandomPath(Ghost ghost)
        {
            var start = Vector2Int.FloorToInt(ghost.body.position / 16);
            positions = GameManager.GetPath(start, GameManager.GetRandomEdgeTile(start));
            currentPosition = 0;
            if (positions != null && positions.Count() > 0)
            {
                ghost.target = positions[currentPosition];
            }
        }
    }
}
