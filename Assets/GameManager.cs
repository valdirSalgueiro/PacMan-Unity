using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GameManager : MonoBehaviour
{

    public Tilemap TileMap;    
    public GameObject Pill;
    public GameObject PillContainer;

    

    // Use this for initialization
    void Start()
    {
        for (int i = -13; i < 13; i++)
        {
            for (int j = -16; j < 13; j++)
            {
                var tile = TileMap.GetTile(new Vector3Int(i, j, 0));
                if (tile == null)
                {
                    var pill = Instantiate(Pill, PillContainer.transform);
                    pill.transform.localPosition = new Vector3(i * 16, j * 16, 0);
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {        
    }
}
