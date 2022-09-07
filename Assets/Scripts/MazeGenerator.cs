using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MazeGenerator : MonoBehaviour
{
    public Tilemap tilemap;
    public List<MazeTile> tiles;
    public Vector3Int startPos;
    public int chunkHeight;
    public int chunkWidth;
    public FloorGenerator floorGen;
    MazeCell[,] chunk;
    public const int NORTH = 0b1000, WEST = 0b0100, SOUTH = 0b0010, EAST = 0b0001;

    void Start() {
        Generate();    
    }
    public void Generate()
    {
        chunk = new MazeCell[chunkWidth,chunkHeight];
        var random = new System.Random();
        for (int x=0;x<chunk.GetLength(0);x++){
            for (int y=0;y<chunk.GetLength(1);y++){
                MazeCell cell = new MazeCell();
                cell.x = x;
                cell.y = y;
                chunk[x,y] = cell;
            }
        }
        MazeCell current = chunk[startPos.x,startPos.y];
        current.visited = true;
        var stack = new List<MazeCell>();
        stack.Add(current); 
        for (int i = 0; i < 200;i++) {
            var neighbors = new List<int[]>(); 
            if (current.x > 0){
                MazeCell westCell = chunk[current.x-1, current.y];
                if (!westCell.visited && !stack.Contains(westCell)){
                    neighbors.Add(new int[2]{-1,0});
                }
            }
            if (current.y > 0){
                MazeCell northCell = chunk[current.x, current.y-1];
                if (!northCell.visited && !stack.Contains(northCell)){
                    neighbors.Add(new int[2]{0,-1});
                }
            }
            if (current.x < chunkWidth-1){
                MazeCell eastCell = chunk[current.x+1, current.y];
                if (!eastCell.visited && !stack.Contains(eastCell)){
                    neighbors.Add(new int[2]{1,0});
                }
            }
            if (current.y < chunkHeight-1){
                MazeCell southCell = chunk[current.x, current.y+1];
                if (!southCell.visited && !stack.Contains(southCell)){
                    neighbors.Add(new int[2]{0,1});
                }
            }
            if (neighbors.Count == 0){
                current = stack[stack.Count-1];
                current.visited = true;
                stack.RemoveAt(stack.Count-1);
                if(stack.Count == 0){
                    break;
                }
                continue;
            }
            var posDiff = neighbors[random.Next(neighbors.Count)];
            int xd = posDiff[0];
            int yd = posDiff[1];
            MazeCell chosenCell = chunk[current.x+xd,current.y+yd];
            if (xd == 1){
                current.walls += EAST;
                chosenCell.walls += WEST;
            }
            else if (xd == -1){
                current.walls += WEST;
                chosenCell.walls += EAST;
            }
            if (yd == -1){
                current.walls += NORTH;
                chosenCell.walls += SOUTH;
            }
            else if (yd == 1){
                current.walls += SOUTH;
                chosenCell.walls += NORTH;
            }
            //Debug.Log(current.x.ToString()+":"+current.y.ToString()+" -> "+current.walls.ToString());
            stack.Add(chosenCell);
            current = chosenCell;
        }

        chunk[random.Next(chunkWidth-1),0].walls += NORTH;
        chunk[0,random.Next(chunkHeight-1)].walls += WEST;
        chunk[random.Next(chunkWidth-1),chunkHeight-1].walls += SOUTH;
        chunk[chunkWidth-1,random.Next(chunkHeight-1)].walls += EAST;

        foreach (MazeCell cell in chunk){
            DrawMazeTile(cell);
        }
    }

    void DrawMazeTile(MazeCell cell){
        foreach (MazeTile t in tiles){
                if (cell.walls == t.GetBinaryWalls()){
                    tilemap.SetTile(new Vector3Int(cell.x-chunkWidth/2, -cell.y-1+chunkHeight/2, 0), t.sprite);
                }
            }
    }
}

public class MazeCell
{
    public bool visited = false;
    public int walls = 0b0000;
    public int x;
    public int y;
}

[System.Serializable]
public class MazeTile
{
    public Tile sprite;
    public bool northOpened = false;
    public bool westOpened = false;
    public bool southOpened = false;
    public bool eastOpened = false;

    public int GetBinaryWalls(){
        int bin = 0b0000;
        if (northOpened){
            bin += MazeGenerator.NORTH;
        } if (westOpened){
            bin += MazeGenerator.WEST;
        } if (southOpened){
            bin += MazeGenerator.SOUTH;
        } if (eastOpened){
            bin += MazeGenerator.EAST;
        }
        return bin;
    }
}