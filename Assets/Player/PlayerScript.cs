using Assets;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerScript : MonoBehaviour
{
    public Tilemap TileMap;

    private float speed = 3f;
    private Vector2Int direction;
    private Vector2Int desiredDirection;
    private Vector2 destination;
    private Rigidbody2D body;

    public GameObject WarpIn;
    public GameObject WarpOut;

    private Animator animator;
    private Vector2 lastPosition;

    // Use this for initialization
    void Start()
    {
        body = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        direction = Vector2Int.left;
        destination = body.position;
        desiredDirection = direction;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            desiredDirection = Vector2Int.left;
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            desiredDirection = Vector2Int.right;
        }
        else if (Input.GetKey(KeyCode.UpArrow))
        {
            desiredDirection = Vector2Int.up;
        }
        else if (Input.GetKey(KeyCode.DownArrow))
        {
            desiredDirection = Vector2Int.down;
        }

        if (body.position == destination)
        {
            if (transform.position == WarpIn.transform.position && direction == Vector2Int.left)
            {
                transform.position = WarpOut.transform.position;
            }
            else if (transform.position == WarpOut.transform.position && direction == Vector2Int.right)
            {
                transform.position = WarpIn.transform.position;
            }

            Vector2Int bodyPosition = Vector2Int.FloorToInt(body.position / 16);

            if (direction != desiredDirection)
            {
                if (desiredDirection == Vector2.left)
                {
                    if (!Utils.isWallOrGhostArea(TileMap, bodyPosition + Vector2Int.left))
                    {
                        direction = Vector2Int.left;
                    }
                }
                else if (desiredDirection == Vector2.right)
                {
                    if (!Utils.isWallOrGhostArea(TileMap, bodyPosition + Vector2Int.right))
                    {
                        direction = Vector2Int.right;
                    }
                }
                else if (desiredDirection == Vector2.up)
                {
                    if (!Utils.isWallOrGhostArea(TileMap, bodyPosition + Vector2Int.up))
                    {
                        direction = Vector2Int.up;
                    }
                }
                else if (desiredDirection == Vector2.down)
                {
                    if (!Utils.isWallOrGhostArea(TileMap, bodyPosition + Vector2Int.down))
                    {
                        direction = Vector2Int.down;
                    }
                }
            }
            var nextPosition = bodyPosition + direction;
            if (!Utils.isWallOrGhostArea(TileMap, nextPosition))
            {
                destination = nextPosition * 16;
            }
        }

        var nextP = Vector2.MoveTowards(body.position, destination, speed);
        body.MovePosition(nextP);

        if (lastPosition == body.position)
            animator.SetTrigger("idle");
        else
            animator.SetTrigger("run");

        if (direction == Vector2Int.down)
        {
            transform.rotation = Quaternion.Euler(0, 0, 90);
        }
        else if (direction == Vector2Int.up)
        {
            transform.rotation = Quaternion.Euler(0, 0, -90);
        }
        else if (direction == Vector2Int.right)
        {
            transform.rotation = Quaternion.Euler(0, 180, 0);
        }
        else
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }


        lastPosition = body.position;
    }
}
