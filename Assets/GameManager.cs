using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GameManager : MonoBehaviour {

    public Tilemap TileMap;
    public GameObject Player;

    // Use this for initialization
    void Start () {
        for (int i = -15; i < 15; i++)
        {
            for (int j = -15; j < 15; j++)
            {
                var tile = TileMap.GetTile(new Vector3Int(i, j, 0));
                if (tile == null)
                {
                    Debug.Log(i + ", " + j + " null");
                }
                else
                {
                    Debug.Log(i + ", " + j + " "+tile.name);
                }
            }
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
