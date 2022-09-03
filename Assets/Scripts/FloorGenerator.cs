using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class FloorGenerator : MonoBehaviour
{
    public Tilemap tilemap;
    public List<Tile> tiles;
    public Vector3Int startPos;
    public Vector3Int endPos;

    // Start is called before the first frame update
    void Start()
    {
        System.Random random = new System.Random();
        for(int x = startPos.x; x <= endPos.x; x++){
            for(int y = startPos.y; y <= endPos.y; y++){
                Vector3Int position = new Vector3Int(x, y, 0);
                Tile tile = tiles[random.Next(tiles.Count)];
                tilemap.SetTile(position, tile);
            }
        }
    }
}
