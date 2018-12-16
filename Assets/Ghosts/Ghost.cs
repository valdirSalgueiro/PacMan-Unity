using Assets.Ghosts;
using Assets.Ghosts.ChaseStrategies;
using Assets.Ghosts.State;
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
        public GameObject playerGameObject;
        public Vector2 target;
        public Rigidbody2D body;
        public Rigidbody2D playerBody;
        public SpriteRenderer spriteRenderer;
        public Player player;

        private Animator animator;

        protected IGhostState state;
        public Vector2 SpawningLocation;

        protected float DeadTimer { get; set; }
        protected float ScatterTimer { get; set; }

        public GameObject WarpIn;
        public GameObject WarpOut;

        public List<IChaseStrategy> chaseStrategies = new List<IChaseStrategy>();

        public Vector2 warpInPositionVector2;
        public Vector2 warpOutPositionVector2;

        public GameObject ScatterSpawns;

        public float GetScatterTime()
        {
            return ScatterTimer;
        }

        public float GetDeadTimer()
        {
            return DeadTimer;
        }

        public void SetFrightened()
        {
            if (!(state is DeadState || state is DeadState))
            {
                state = new FrightnedState(this);
                state.Start(this);
            }
        }

        // Use this for initialization
        void Start()
        {
            body = GetComponent<Rigidbody2D>();
            animator = GetComponent<Animator>();
            playerBody = playerGameObject.GetComponent<Rigidbody2D>();
            player = playerGameObject.GetComponent<Player>();
            spriteRenderer = GetComponent<SpriteRenderer>();

            warpInPositionVector2 = new Vector2(WarpIn.transform.position.x, WarpIn.transform.position.y);
            warpOutPositionVector2 = new Vector2(WarpOut.transform.position.x, WarpOut.transform.position.y);

            SpawningLocation = body.position;
            state = new DeadState(this);
        }

        void FixedUpdate()
        {
            var nextState = state.Update(this);
            if (nextState != null)
            {
                state.Exit(this);
                state = nextState;
                state.Start(this);
            }

            if (state is FrightnedState)
            {
                animator.SetTrigger("frightned");
            }
            else if (state is GoingHomeState)
            {
                var direction = (target - body.position).normalized;
                if (direction == Vector2Int.right)
                {
                    animator.SetTrigger("deadright");
                }
                else if (direction == Vector2Int.down)
                {
                    animator.SetTrigger("deaddown");
                }
                else if (direction == Vector2Int.up)
                {
                    animator.SetTrigger("deadup");
                }
                else
                {
                    animator.SetTrigger("deadleft");
                }
            }
            else
            {
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
                else
                {
                    animator.SetTrigger("left");
                }
            }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Player"))
            {
                if(state is FrightnedState)
                {
                    state.Exit(this);
                    state = new GoingHomeState(this);
                    state.Start(this);
                }
                else if (!(state is GoingHomeState))
                {
                    Debug.Log("dead");
                }
            }
        }
    }
}
