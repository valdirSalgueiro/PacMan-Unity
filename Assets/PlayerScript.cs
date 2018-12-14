using Assets;
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
            if (body.position == new Vector2Int(-240, -16) && direction == Vector2Int.left)
            {
                body.position = new Vector2(224, -16);
            }
            else if (body.position == new Vector2Int(224, -16) && direction == Vector2Int.right)
            {
                body.position = new Vector2(-240, -16);
            }

            Vector2Int bodyPosition = Vector2Int.FloorToInt(body.position / 16);            

            if (direction != desiredDirection)
            {
                if (desiredDirection == Vector2.left)
                {
                    if (!Utils.isWall(TileMap, bodyPosition + Vector2Int.left))
                    {
                        direction = Vector2Int.left;
                    }
                }
                else if (desiredDirection == Vector2.right)
                {
                    if (!Utils.isWall(TileMap, bodyPosition + Vector2Int.right))
                    {
                        direction = Vector2Int.right;
                    }
                }
                else if (desiredDirection == Vector2.up)
                {
                    if (!Utils.isWall(TileMap, bodyPosition + Vector2Int.up))
                    {
                        direction = Vector2Int.up;
                    }
                }
                else if (desiredDirection == Vector2.down)
                {
                    if (!Utils.isWall(TileMap, bodyPosition + Vector2Int.down))
                    {
                        direction = Vector2Int.down;
                    }
                }
            }
            var nextPosition = bodyPosition + direction;
            if (!Utils.isWall(TileMap, nextPosition))
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
}
