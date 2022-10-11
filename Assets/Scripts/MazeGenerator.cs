using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MazeGenerator : MonoBehaviour
{
    public Tilemap tilemap;
    public Tilemap floorTilemap;
    public List<MazeTile> tiles;
    public List<Tile> floorTiles;
    public GameObject player;
    public int chunkWidth;
    public int chunkHeight;
    public Dictionary<Vector2Int,Chunk> chunks = new Dictionary<Vector2Int,Chunk>();
    

    void Start() {
        CreateMaze();
    }

    private void Update() {
        CreateMaze();
    }

    public void CreateMaze(bool reset = false) {
        Vector3 pos = player.transform.position/3;
        Vector2Int chunkPos = new Vector2Int(Mathf.FloorToInt(pos.x/chunkWidth),Mathf.FloorToInt(pos.y/chunkHeight));
        GenerateChunks(chunkPos, reset);
    }

    public void GenerateChunks(Vector2Int coords, bool reset=false) {
        for (int x = -1; x < 2; x++){
            for (int y = -1; y < 2; y++){
                Vector2Int pos = new Vector2Int(coords.x+x,coords.y+y);
                if (chunks.ContainsKey(pos) && !reset){
                    continue;
                }
                Chunk chunk = new Chunk(pos,chunkWidth,chunkHeight);
                chunk.Draw();
                chunks[pos] = chunk;
            }
        }
    }

    public void UpdateMaze(){
        foreach (var c in chunks.Values){
            c.Draw();
        }
    }
    
}



[System.Serializable]
public class MazeTile
{
    public Tile sprite;
    public bool northOpened = false;
    public bool westOpened = false;
    public bool southOpened = false;
    public bool eastOpened = false;

    public bool northWall = false;
    public bool westWall = false;
    public bool southWall = false;
    public bool eastWall = false;
    public int GetBinaryOpenings(){
        int bin = 0b0000;
        if (northOpened){
            bin += Cardinal.NORTH;
        } if (westOpened){
            bin += Cardinal.WEST;
        } if (southOpened){
            bin += Cardinal.SOUTH;
        } if (eastOpened){
            bin += Cardinal.EAST;
        }
        return bin;
    }
    public int GetBinaryWalls(){
        int bin = 0b0000;
        if (northWall){
            bin += Cardinal.NORTH;
        } if (westWall){
            bin += Cardinal.WEST;
        } if (southWall){
            bin += Cardinal.SOUTH;
        } if (eastWall){
            bin += Cardinal.EAST;
        }
        return bin;
    }
}
public class Cardinal {
    public const int NORTH = 0b1000, WEST = 0b0100, SOUTH = 0b0010, EAST = 0b0001;
}