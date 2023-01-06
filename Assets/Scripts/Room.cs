using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Room
{
    public MazeCell[,] cells;
    public int width;
    public int height;

    public Room (int w=3, int h=3) {
        width = w;
        height = h;
        cells = Generate();
    }

    public MazeCell[,] Generate() {
        MazeCell[,] cells = new MazeCell[width,height];
        for (int x = 0; x < width; x++) {
            for (int y = 0; y < height; y++) {
                MazeCell cell = new MazeCell(x, y);
                cell.walls = 0b0000;
                if (x == 0) {
                    cell.walls |= Cardinal.WEST;
                } else if (x == width - 1) {
                    cell.walls |= Cardinal.EAST;
                }
                if (y == 0) {
                    cell.walls |= Cardinal.SOUTH;
                } else if (y == height - 1) {
                    cell.walls |= Cardinal.NORTH;
                }
                cells[x,y] = cell;
            }
        }
        /*
        int horizontalOpening = Random.Range(1, width-1);
        int horizontalOpeningDirection = new int[2]{Cardinal.NORTH,Cardinal.SOUTH}[Random.Range(0, 2)];
        int verticalOpening = Random.Range(1, height-1);
        int verticalOpeningDirection = new int[2]{Cardinal.EAST,Cardinal.WEST}[Random.Range(0, 2)];
        if (horizontalOpeningDirection == Cardinal.NORTH) {
            cells[horizontalOpening, height-1].openings |= Cardinal.NORTH;
        } else {
            cells[horizontalOpening, 0].openings |= Cardinal.SOUTH;
        }
        if (verticalOpeningDirection == Cardinal.EAST) {
            cells[width-1, verticalOpening].openings |= Cardinal.EAST;
        } else {
            cells[0, verticalOpening].openings |= Cardinal.WEST;
        }
        */
        return cells;
    }

}