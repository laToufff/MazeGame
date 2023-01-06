using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chunk
{
    public int height;
    public int width;
    public Vector2Int coords;
    public MazeCell[,] grid;
    public Vector2Int startPos = new Vector2Int(0,0);
    public MazeGenerator mg;
    public int northExit;
    public int southExit;
    public int eastExit;
    public int westExit;
    public Chunk (Vector2Int c, int w, int h) {
        height = h;
        width = w;
        coords = c;
        grid = new MazeCell[width,height];
        mg = GameObject.Find("MazeGenerator").GetComponent<MazeGenerator>() as MazeGenerator;
        Generate();
    }

    public void Generate()
    {
        for (int x=0;x<grid.GetLength(0);x++){
            for (int y=0;y<grid.GetLength(1);y++){
                MazeCell cell = new MazeCell(x,y);
                grid[x,y] = cell;
            }
        }
        MazeCell current = grid[startPos.x,startPos.y];
        current.visited = true;
        var stack = new List<MazeCell>();
        stack.Add(current); 
        for (int i = 0; i < 200;i++) {
            var neighbors = new List<int[]>(); 
            if (current.x > 0){
                MazeCell westCell = grid[current.x-1, current.y];
                if (!westCell.visited && !stack.Contains(westCell)){
                    neighbors.Add(new int[2]{-1,0});
                }
            }
            if (current.y > 0){
                MazeCell northCell = grid[current.x, current.y-1];
                if (!northCell.visited && !stack.Contains(northCell)){
                    neighbors.Add(new int[2]{0,-1});
                }
            }
            if (current.x < width-1){
                MazeCell eastCell = grid[current.x+1, current.y];
                if (!eastCell.visited && !stack.Contains(eastCell)){
                    neighbors.Add(new int[2]{1,0});
                }
            }
            if (current.y < height-1){
                MazeCell southCell = grid[current.x, current.y+1];
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
            var posDiff = neighbors[Random.Range(0,neighbors.Count)];
            int xd = posDiff[0];
            int yd = posDiff[1];
            MazeCell chosenCell = grid[current.x+xd,current.y+yd];
            if (xd == 1){
                current.openings |= Cardinal.EAST;
                chosenCell.openings |= Cardinal.WEST;
            }
            else if (xd == -1){
                current.openings |= Cardinal.WEST;
                chosenCell.openings |= Cardinal.EAST;
            }
            if (yd == -1){
                current.openings |= Cardinal.SOUTH;
                chosenCell.openings |= Cardinal.NORTH;
            }
            else if (yd == 1){
                current.openings |= Cardinal.NORTH;
                chosenCell.openings |= Cardinal.SOUTH;
            }
            stack.Add(chosenCell);
            current = chosenCell;
        }

        GenerateRoom();
        GenerateExits();
    }

    public void GenerateExits(){
        if (mg.chunks.ContainsKey(coords+Vector2Int.up)){
            Chunk northChunk = mg.chunks[coords+Vector2Int.up];
            northExit = northChunk.southExit;
        } 
        else {
            northExit = Random.Range(0,width);
        }
        if (mg.chunks.ContainsKey(coords+Vector2Int.down)){
            Chunk southChunk = mg.chunks[coords+Vector2Int.down];
            southExit = southChunk.northExit;
        } 
        else {
            southExit = Random.Range(0,width);
        }
        if (mg.chunks.ContainsKey(coords+Vector2Int.left)){
            Chunk westChunk = mg.chunks[coords+Vector2Int.left];
            westExit = westChunk.eastExit;
        } 
        else {
            westExit = Random.Range(0,height);
        }
        if (mg.chunks.ContainsKey(coords+Vector2Int.right)){
            Chunk eastChunk = mg.chunks[coords+Vector2Int.right];
            eastExit = eastChunk.westExit;
        } 
        else {
            eastExit = Random.Range(0,height);
        }
        grid[northExit,height-1].openings |= Cardinal.NORTH;
        grid[southExit,0].openings |= Cardinal.SOUTH;
        grid[0,westExit].openings |= Cardinal.WEST;
        grid[width-1,eastExit].openings |= Cardinal.EAST;

    }

    public void GenerateRoom(){
        int rng = Random.Range(0,4);
        if (rng == 0){
            int w;
            int h = w = Mathf.FloorToInt(Random.Range(1,5)/4f) + 3;
            Vector2Int roomPos = new Vector2Int(Random.Range(1,width-w),Random.Range(1,height-h));
            Room room = new Room(w,h);
            foreach (MazeCell cell in room.cells){
                int x = cell.x + roomPos.x;
                int y = cell.y + roomPos.y;
                grid[x,y] = cell;
                if (cell.x == 0){
                    if (cell.y == 0){
                        grid[x-1,y].openings &= ~Cardinal.EAST;
                        grid[x,y-1].openings &= ~Cardinal.NORTH;
                    } else if (cell.y == h-1){
                        grid[x-1,y].openings &= ~Cardinal.EAST;
                        grid[x,y+1].openings &= ~Cardinal.SOUTH;
                    } else {
                        cell.openings |= (grid[x-1,y].openings & Cardinal.EAST) * 0b0100;
                    }
                } 
                else if (cell.x == w-1){
                    if (cell.y == 0){
                        grid[x+1,y].openings &= ~Cardinal.WEST;
                        grid[x,y-1].openings &= ~Cardinal.NORTH;
                    } else if (cell.y == h-1){
                        grid[x+1,y].openings &= ~Cardinal.WEST;
                        grid[x,y+1].openings &= ~Cardinal.SOUTH;
                    } else {
                        cell.openings |= (grid[x+1,y].openings & Cardinal.WEST) / 0b0100;
                    }
                } 
                else if (cell.y == 0){
                    cell.openings |= (grid[x,y-1].openings & Cardinal.NORTH) / 0b0100;
                } 
                else if (cell.y == h-1){
                    cell.openings |= (grid[x,y+1].openings & Cardinal.SOUTH) * 0b0100;
                }
                cell.x = x;
                cell.y = y;
            }
        }
    }

    public void Draw(){
        foreach (MazeCell cell in grid) {
            mg.tilemap.SetTile(new Vector3Int(cell.x+coords.x*width, cell.y+coords.y*height, 0), cell.GetTile(mg.tiles).sprite);
            mg.floorTilemap.SetTile(new Vector3Int(cell.x+coords.x*width, cell.y+coords.y*height, 0), mg.floorTiles[Random.Range(0,mg.floorTiles.Count)]);
        }
    }
}

public class MazeCell
{
    public bool visited = false;
    public int openings = 0b0000;
    public int walls = 0b1111;
    public int x;
    public int y;
    public MazeCell(int _x, int _y){
        x = _x;
        y = _y;
    }

    public MazeTile GetTile(List<MazeTile> tiles){
        foreach (MazeTile t in tiles){
                if (openings == t.GetBinaryOpenings() && walls == t.GetBinaryWalls()){
                    return t;
                }
            }
        return tiles[15];
    }
}
