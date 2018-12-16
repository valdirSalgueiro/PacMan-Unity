using Assets.Ghosts;
using Assets.Ghosts.ChaseStrategies;
using Assets.Ghosts.State;
using RoyT.AStar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Assets
{
    public class Ghost : MonoBehaviour
    {
        public Vector2 target;
        public Rigidbody2D playerBody;
        public SpriteRenderer spriteRenderer;
        public Player player;
        public Tilemap TileMap;
        public Vector2 SpawningLocation;

        public Vector2Int direction;

        private Animator animator;

        protected IGhostState state;

        protected float DeadTimer { get; set; }
        protected float ScatterTimer { get; set; }

        private GameObject WarpIn;
        private GameObject WarpOut;

        public List<IChaseStrategy> chaseStrategies = new List<IChaseStrategy>();

        public Vector2 warpInPositionVector2;
        public Vector2 warpOutPositionVector2;

        private GameObject ScatterSpawns;

        public GridTiles gridTiles;

        private Rigidbody2D body;

        public GameObject GetScatterSpawns()
        {
            return ScatterSpawns;
        }

        public void SetScatterSpawns(GameObject ScatterSpawns)
        {
            this.ScatterSpawns = ScatterSpawns;
        }

        public Rigidbody2D GetBody()
        {
            return body;
        }

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
        protected virtual void Start()
        {
            var playerGameObject = GameObject.FindGameObjectWithTag("Player");
            WarpIn = GameObject.Find("WarpIn");
            WarpOut = GameObject.Find("WarpOut");

            gridTiles = GameManager.InitGrid(TileMap, true);

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

            direction = Vector2Int.FloorToInt((target - body.position).normalized);
            if (state is FrightnedState)
            {
                animator.SetTrigger("frightned");
            }
            else if (state is GoingHomeState)
            {
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
                    player.IsDead = true;
                }
            }
        }
    }
}
