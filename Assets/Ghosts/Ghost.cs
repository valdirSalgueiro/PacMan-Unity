using RoyT.AStar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets
{
    public class Ghost : MonoBehaviour
    {
        public GameObject player;
        public Position[] positions;
        private Rigidbody2D body;
        private Rigidbody2D playerBody;

        private Vector2Int goal;
        int currentPos = 0;

        private float speed = 2f;
        private Animator animator;

        // Use this for initialization
        void Start()
        {
            body = GetComponent<Rigidbody2D>();
            animator = GetComponent<Animator>();
            playerBody = player.GetComponent<Rigidbody2D>();
        }

        void FixedUpdate()
        {
            if (positions == null)
            {
                chasePlayer();
            }

            if (positions != null)
            {
                var target = GameManager.ConvertPosition(positions[currentPos]);
                if (body.position == target)
                {
                    if (currentPos < 3 && currentPos < positions.Length - 1)
                    {
                        currentPos++;
                    }
                    else
                    {
                        chasePlayer();
                    }
                }

                var direction = (target - body.position).normalized;
                if (direction == Vector2Int.right)
                {
                    animator.SetTrigger("right");
                }
                else if (direction == Vector2Int.down)
                {
                    animator.SetTrigger("down");
                }
                else if (direction == Vector2Int.up)
                {
                    animator.SetTrigger("up");
                }
                else if (direction == Vector2Int.left)
                {
                    Debug.Log(direction);
                    animator.SetTrigger("left");
                }


                var nextP = Vector2.MoveTowards(body.position, target, speed);
                body.MovePosition(nextP);
            }

        }

        private void chasePlayer()
        {
            var start = Vector2Int.FloorToInt(body.position / 16);
            goal = Vector2Int.FloorToInt(playerBody.position / 16);
            positions = GameManager.GetPath(start, goal);
            currentPos = 0;
        }
    }
}
