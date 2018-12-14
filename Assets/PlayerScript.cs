using System.Collections;
using System.Collections.Generic;
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

    // can al
    bool turn;

    // Use this for initialization
    void Start()
    {
        direction = Vector2Int.left;
        desiredDirection = direction;
        body = GetComponent<Rigidbody2D>();
        destination = body.position;
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
            Vector2Int bodyPosition = Vector2Int.FloorToInt(body.position / 16);
            if (direction != desiredDirection)
            {
                if (desiredDirection == Vector2.left)
                {
                    if (!isWall(bodyPosition + Vector2Int.left))
                    {
                        direction = Vector2Int.left;
                    }
                }
                else if (desiredDirection == Vector2.right)
                {
                    if (!isWall(bodyPosition + Vector2Int.right))
                    {
                        direction = Vector2Int.right;
                    }
                }
                else if (desiredDirection == Vector2.up)
                {
                    if (!isWall(bodyPosition + Vector2Int.up))
                    {
                        direction = Vector2Int.up;
                    }
                }
                else if (desiredDirection == Vector2.down)
                {
                    if (!isWall(bodyPosition + Vector2Int.down))
                    {
                        direction = Vector2Int.down;
                    }
                }
            }
            var nextPosition = bodyPosition + direction;
            Debug.Log(nextPosition);
            if (!isWall(nextPosition))
            {
                destination = nextPosition * 16;
            }
        }
        else
        {
            var nextP = Vector2.MoveTowards(body.position, destination, speed);
            body.MovePosition(nextP);
        }
    }

    bool isWall(Vector2Int pos)
    {

        return TileMap.GetTile(Vector3Int.FloorToInt(new Vector3(pos.x, pos.y, 0))) != null || isGhostArea(pos);
    }

    bool isGhostArea(Vector2Int pos)
    {
        return (pos.x > -4 && pos.x < 3 && pos.y > -3 && pos.y < 2);
    }
}
