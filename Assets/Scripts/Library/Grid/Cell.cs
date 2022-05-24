using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell
{
    public Grid<Cell> Grid { get; private set; }
    public int X { get; private set; }
    public int Y { get; private set; }

    public GameObject Building { get; set; }

    public Cell(Grid<Cell> grid, int x, int y)
    {
        this.Grid = grid;
        this.X = x;
        this.Y = y;
    }

    public Cell Left(int distance = 1)
    {
        return this.Grid[this.X - distance, this.Y];
    }
    public Cell Right(int distance = 1)
    {
        return this.Grid[this.X + distance, this.Y];
    }
    public Cell Up(int distance = 1)
    {
        return this.Grid[this.X, this.Y + distance];
    }
    public Cell Down(int distance = 1)
    {
        return this.Grid[this.X, this.Y - distance];
    }
    public Cell UpRight(int distanceX = 1, int distanceY = 1)
    {
        return this.Grid[this.X + distanceX, this.Y + distanceY];
    }

    public bool CanLeft(int distance = 1)
    {
        return (this.X - distance) >= 0;
    }
    public bool CanRight(int distance = 1)
    {
        return (this.X + distance) < this.Grid.Width;
    }
    public bool CanUp(int distance = 1)
    {
        return (this.Y + distance) < this.Grid.Height;
    }
    public bool CanDown(int distance = 1)
    {
        return (this.Y - distance) >= 0;
    }

    public Vector3 GetWorldPosition()
    {
        return this.Grid.GetWorldPosition(this.X, this.Y);
    }

    public Vector3 GetRandomWorldPosition()
    {
        float halfCellWidth = this.Grid.CellWidth * .2f;
        float halfCellHeight = this.Grid.CellHeight * .2f;

        Vector3 centerPosition = this.GetWorldPosition();
        Vector3 randomPosition = new Vector3(
            Random.Range(-halfCellWidth, halfCellWidth),
            Random.Range(-halfCellHeight, halfCellHeight),
            0
        );

        return centerPosition + randomPosition;
    }

    public override string ToString()
    {
        return $"({this.X}, {this.Y})";
    }
}
