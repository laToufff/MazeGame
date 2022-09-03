using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MazeGenerator : MonoBehaviour
{
    public Tilemap tilemap;
    public List<MazeTile> tiles;
    public int startPosX;
    public int startPosY;
    MazeCell[,] chunk = new MazeCell[6, 4];

    // Start is called before the first frame update
    void Start()
    {
        var random = new System.Random();
        for (int x=0;x<chunk.GetLength(0);x++){
            for (int y=0;y<chunk.GetLength(1);y++){
                MazeCell cell = new MazeCell();
                cell.x = x;
                cell.y = y;
                chunk[x,y] = cell;
            }
        }
        MazeCell current = chunk[startPosX,startPosY];
        current.visited = true;
        var stack = new List<MazeCell>();
        stack.Add(current); 
        for (int i = 0; i < 100;i++) {
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
            if (current.x < chunk.GetLength(0)-1){
                MazeCell eastCell = chunk[current.x+1, current.y];
                if (!eastCell.visited && !stack.Contains(eastCell)){
                    neighbors.Add(new int[2]{1,0});
                }
            }
            if (current.y < chunk.GetLength(1)-1){
                MazeCell southCell = chunk[current.x, current.y+1];
                if (!southCell.visited && !stack.Contains(southCell)){
                    neighbors.Add(new int[2]{0,1});
                }
            }
            if (neighbors.Count == 0){
                current.visited = true;
                current = stack[stack.Count-1];
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
                current.walls += 0b0001;
                chosenCell.walls += 0b0100;
            }
            else if (xd == -1){
                current.walls += 0b0100;
                chosenCell.walls += 0b0001;
            }
            if (yd == 1){
                current.walls += 0b1000;
                chosenCell.walls += 0b0010;
            }
            else if (yd == -1){
                current.walls += 0b0010;
                chosenCell.walls += 0b1000;
            }
            foreach (MazeTile t in tiles){
                if (current.walls == t.GetBinaryWalls()){
                    tilemap.SetTile(new Vector3Int(current.x, current.y, 0), t.sprite);
                }
            }
            Debug.Log(current.x.ToString()+":"+current.y.ToString()+" -> "+current.walls.ToString());
            stack.Add(chosenCell);
            current = chosenCell;
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
            bin += 0b1000;
        } if (westOpened){
            bin += 0b0100;
        } if (southOpened){
            bin += 0b0010;
        } if (eastOpened){
            bin += 0b0001;
        }
        return bin;
    }
}