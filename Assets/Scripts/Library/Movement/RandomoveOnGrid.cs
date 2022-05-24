using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RandomoveOnGrid : MonoBehaviour
{
    [field: SerializeField]
    public Vector3 OriginPosition { get; set; }

    private Cell Cell { get; set; }
    private Cell TargetCell { get; set; }
    private Vector2 Direction { get; set; }

    private SpriteRenderer SpriteRenderer { get; set; }
    private IMovePosition Move { get; set; }

    // Start is called before the first frame update
    void Start()
    {
        this.SpriteRenderer = this.GetComponentInChildren<SpriteRenderer>();
        this.Move = this.GetComponent<IMovePosition>();

        this.Cell = GameManager.Grid.GetNearest(transform.position);
        this.MoveToNextTargetCell();
    }

    // Update is called once per frame
    void Update()
    {
        if (this.Move.IsAtTargetPosition()) // test
        {
            // Arrived at cell destination
            this.Cell = this.TargetCell;

            // Move to next cell
            this.MoveToNextTargetCell();
        }
    }

    private void MoveToNextTargetCell()
    {
        // Select next target
        this.Direction = this.GetRandomDirection(this.Cell, this.Direction);
        this.TargetCell = this.Cell.UpRight((int)this.Direction.x, (int)this.Direction.y);

        // Move to target
        this.Move.TargetPosition = this.TargetCell.GetRandomWorldPosition() + this.OriginPosition;
        //Debug.Log($"[{this.name}] Moving from {this.Cell} to {this.TargetCell}");

        // Look to right direction
        if (this.Direction.x != 0)
        {
            this.SpriteRenderer.flipX = this.Direction.x < 0;
        }
    }

    private Vector2 GetRandomDirection(Cell startingCell, Vector2 previousDirection)
    {
        // Check next possible choices
        List<Vector2> choices = new();
        if (startingCell.CanUp() && previousDirection.y > -1)
        {
            choices.Add(Vector2.up);
        }
        if (startingCell.CanDown() && previousDirection.y < 1)
        {
            choices.Add(Vector2.down);
        }
        if (startingCell.CanLeft() && previousDirection.x < 1)
        {
            choices.Add(Vector2.left);
        }
        if (startingCell.CanRight() && previousDirection.x > -1)
        {
            choices.Add(Vector2.right);
        }

        // Make a decision
        return choices.OrderBy(x => Guid.NewGuid()).First();
    }

    public void TurnBack()
    {
        this.TargetCell = this.Cell;
        this.Direction *= -1;

        // Move to target
        this.Move.TargetPosition = this.TargetCell.GetRandomWorldPosition() + this.OriginPosition;
        //Debug.Log($"[{this.name}] Moving from {this.Cell} to {this.TargetCell}");

        // Look to right direction
        if (this.Direction.x != 0)
        {
            this.SpriteRenderer.flipX = this.Direction.x < 0;
        }
    }
}
