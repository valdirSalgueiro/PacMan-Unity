using RoyT.AStar;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blinky : MonoBehaviour
{

    public GameObject player;
    public Position[] positions;
    private Rigidbody2D body;
    private Rigidbody2D playerBody;

    private Vector2Int goal;
    int currentPos = 0;

    private float speed = 2f;

    // Use this for initialization
    void Start()
    {
        body = GetComponent<Rigidbody2D>();
        playerBody = player.GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        if (positions == null)
        {
            var start = Vector2Int.FloorToInt(body.position / 16);
            goal = Vector2Int.FloorToInt(playerBody.position / 16);
            positions = GameManager.GetPath(start, goal);
        }

        var target = GameManager.ConvertPosition(positions[currentPos]);
        if (body.position == target)
        {
            var currentPlayerPosition = Vector2Int.FloorToInt(playerBody.position / 16);

            if (currentPos < positions.Length - 1)
            {
                currentPos++;
            }
            else
            {
                var start = Vector2Int.FloorToInt(body.position / 16);
                positions = GameManager.GetPath(start, currentPlayerPosition);
                currentPos = 0;
            }
        }
        else
        {
            var nextP = Vector2.MoveTowards(body.position, target, speed);
            body.MovePosition(nextP);
        }
    }
}
