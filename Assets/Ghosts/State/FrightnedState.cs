﻿using RoyT.AStar;
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
                        return new ChaseState();
                    }
                }
            }

            base.UpdateCommons(ghost);

            return null;
        }

        private void getNextRandomPath(Ghost ghost)
        {
            var start = Vector2Int.FloorToInt(ghost.body.position / 16);
            positions = GameManager.GetPath(ghost.grid, start, GameManager.GetRandomEdgeTile(start));
            currentPosition = 0;
            if (positions != null && positions.Count() > 0)
            {
                ghost.target = positions[currentPosition];
            }
        }
    }
}