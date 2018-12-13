using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerScript : MonoBehaviour
{

    public Tilemap TileMap;

    private float Speed = 3f;
    private Vector2 dir;
    private Rigidbody2D body;

    // Use this for initialization
    void Start()
    {
        dir = Vector2.left;
        body = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            dir = Vector2.left;
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            dir = Vector2.right;
        }
        else if (Input.GetKey(KeyCode.UpArrow))
        {
            dir = Vector2.up;
        }
        else if (Input.GetKey(KeyCode.DownArrow))
        {
            dir = Vector2.down;
        }

        var nextPosition = body.position + dir;
        var tile = TileMap.GetTile(Vector3Int.FloorToInt(nextPosition));
        if (tile == null)
        {
            var dest = body.position + dir * Time.deltaTime * Speed;
            var nextP = Vector2.MoveTowards(body.position, dest, 1f);
            body.MovePosition(nextP);
        }
    }
}
