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
        public Player player;

        private Animator animator;

        protected IGhostState state;
        public Vector2 SpawningLocation;

        protected float DeadTimer;

        public GameObject WarpIn;
        public GameObject WarpOut;

        public List<IChaseStrategy> chaseStrategies = new List<IChaseStrategy>();

        public Vector2 warpInPositionVector2;
        public Vector2 warpOutPositionVector2;


        // Use this for initialization
        void Start()
        {
            body = GetComponent<Rigidbody2D>();
            animator = GetComponent<Animator>();
            playerBody = playerGameObject.GetComponent<Rigidbody2D>();
            player = playerGameObject.GetComponent<Player>();


            warpInPositionVector2 = new Vector2(WarpIn.transform.position.x, WarpIn.transform.position.y);
            warpOutPositionVector2 = new Vector2(WarpOut.transform.position.x, WarpOut.transform.position.y);

            SpawningLocation = body.position;
            state = new DeadState(this, DeadTimer);
        }

        void FixedUpdate()
        {
            var nextState = state.Update(this);
            if (nextState != null)
            {
                state = nextState;
                state.Start(this);
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
            else
            {
                animator.SetTrigger("left");
            }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Player"))
            {
                //Destroy(gameObject);
                // incrementa score
            }
        }
    }
}
