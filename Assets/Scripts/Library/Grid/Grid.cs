using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid<T>
{
    public event EventHandler<OnGridObjectChangedEventArgs> OnGridObjectChanged;
    public class OnGridObjectChangedEventArgs : EventArgs
    {
        public int x;
        public int y;
    }

    public int Width { get; private set; }
    public int Height { get; private set; }
    public float CellWidth { get; private set; }
    public float CellHeight { get; private set; }
    private Vector3 OriginPosition { get; set; }
    private T[,] Cells { get; set; }

    public Grid(int width, int height, float cellWidth, float cellHeight, Vector3 originPosition, Func<Grid<T>, int, int, T> fCreateCell)
    {
        this.Width = width;
        this.Height = height;
        this.CellWidth = cellWidth;
        this.CellHeight = cellHeight;
        this.OriginPosition = originPosition;

        this.Cells = new T[this.Width, this.Height];
        for (int x = 0; x < this.Width; ++x)
        {
            for (int y = 0; y < this.Height; ++y)
            {
                this.Cells[x, y] = fCreateCell(this, x, y);
            }
        }

        bool showDebug = true;
        if (showDebug)
        {
            TextMesh[,] debugTextArray = new TextMesh[width, height];
            Color lineColor = Color.green;
            float duration = 100f;
            for (int x = 0; x < this.Width; ++x)
            {
                for (int y = 0; y < this.Height; ++y)
                {
                    //debugTextArray[x, y] = Utils.CreateWorldText(this.Cells[x, y]?.ToString(), null, GetWorldPosition(x, y) + new Vector3(cellWidth, cellHeight) * .5f, 40, lineColor, TextAnchor.MiddleCenter);
                    //debugTextArray[x, y].transform.localScale *= 0.1f;
                    Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x, y + 1), lineColor, duration);
                    Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x + 1, y), lineColor, duration);
                }
            }
            Debug.DrawLine(GetWorldPosition(0, height), GetWorldPosition(width, height), lineColor, duration);
            Debug.DrawLine(GetWorldPosition(width, 0), GetWorldPosition(width, height), lineColor, duration);

            this.OnGridObjectChanged += (object sender, OnGridObjectChangedEventArgs eventArgs) => {
                debugTextArray[eventArgs.x, eventArgs.y].text = Cells[eventArgs.x, eventArgs.y]?.ToString();
            };
        }
    }

    public Vector3 GetWorldPosition(int x, int y)
    {
        return new Vector3(x * this.CellWidth, y * this.CellHeight) + this.OriginPosition;
    }

    public bool TryGetXY(Vector3 worldPosition, out int x, out int y)
    {
        x = Mathf.FloorToInt((worldPosition - this.OriginPosition).x / this.CellWidth);
        y = Mathf.FloorToInt((worldPosition - this.OriginPosition).y / this.CellHeight);

        return x > -1 && y > -1 && x < this.Width && y < this.Height;
    }

    public T GetNearest(Vector3 worldPosition)
    {
        int x, y;
        if(!TryGetXY(worldPosition, out x, out y))
        {
            if (x < 0)
                x = 0;
            if (x > this.Width - 1)
                x = this.Width - 1;
            if (y < 0)
                y = 0;
            if (y > this.Height - 1)
                y = this.Height - 1;
        }

        return this[x, y];
    }

    public T this[int x, int y]
    {
        get
        {
            return this.Cells[x, y];
        }
        set
        {
            this.Cells[x, y] = value;
            this.TriggerGridObjectChanged(x, y);
        }
    }

    public T this[Vector3 worldPosition]
    {
        get
        {
            int x, y;
            this.TryGetXY(worldPosition, out x, out y);
            return this[x, y];
        }
        set
        {
            int x, y;
            this.TryGetXY(worldPosition, out x, out y);
            this[x, y] = value;
        }
    }

    public void TriggerGridObjectChanged(int x, int y)
    {
        if (this.OnGridObjectChanged != null)
        {
            this.OnGridObjectChanged(this, new OnGridObjectChangedEventArgs { x = x, y = y });
        }
    }
}