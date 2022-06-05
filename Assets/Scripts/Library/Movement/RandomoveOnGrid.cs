using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RandomoveOnGrid : MonoBehaviour
{
    private Cell Cell { get; set; }
    private Cell TargetCell { get; set; }
    private Vector2 Direction { get; set; }

    private SpriteRenderer SpriteRenderer { get; set; }
    public IMovePosition Move { get; private set; }
    public ITargetCellProvider TargetCellProvider { get; set; }

    public Animator Animator { get; set; }

    // Start is called before the first frame update
    private void Awake()
    {
        this.SpriteRenderer = this.GetComponentInChildren<SpriteRenderer>();
        this.Animator = this.GetComponentInChildren<Animator>();
        this.Move = this.GetComponent<IMovePosition>();
        this.TargetCellProvider = this.GetComponent<ITargetCellProvider>();
        this.Cell = GameManager.Grid.GetNearest(transform.position);
    }

    private void Start()
    {
        this.MoveToNextTargetCell();
    }

    // Update is called once per frame
    private void Update()
    {
        if (this.Move.IsAtTargetPosition())
        {
            // Arrived at cell destination
            this.Cell = this.TargetCell;

            // Move to next cell
            this.MoveToNextTargetCell();
        }
    }

    private void MoveToNextTargetCell()
    {
        if (this.TargetCellProvider != null && this.TargetCellProvider.GetTargetPosition(this.Cell, out Vector3 targetPosition, out Vector2 direction))
        {
            this.Direction = direction;
            this.TargetCell = GameManager.Grid.GetNearest(targetPosition);
            this.Move.TargetPosition = targetPosition;
            this.LookProperSide();
        }
        else
        {
            // Select next target randomly
            Vector2 randomDirection = this.GetRandomDirection(this.Cell, this.Direction);
            this.MoveToDirection(randomDirection);
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

    public void MoveToDirection(Vector2 direction)
    {
        // Select next target
        this.Direction = direction;
        Cell targetCell = this.Cell.UpRight((int)this.Direction.x, (int)this.Direction.y);
        this.MoveTo(targetCell);
    }

    public void MoveTo(Cell cell)
    {
        this.TargetCell = cell;
        this.Move.TargetPosition = this.TargetCell.GetRandomWorldPosition();
        //Debug.Log($"[{this.name}] Moving from {this.Cell} to {this.TargetCell}");
        this.LookProperSide();
    }

    public void MoveTo(Vector3 targetPosition)
    {
        Vector3 distance = targetPosition - transform.position;
        this.Direction = new Vector2(
            distance.x / Mathf.Abs(distance.x),
            distance.y / Mathf.Abs(distance.y)
        );
        this.TargetCell = GameManager.Grid.GetNearest(targetPosition);
        this.Move.TargetPosition = targetPosition;
        this.LookProperSide();
    }

    private void LookProperSide()
    {
        // Look to right direction
        if (this.Direction.x != 0)
        {
            this.SpriteRenderer.flipX = this.Direction.x < 0;
        }
    }

    public void TurnBack()
    {
        if (!this.enabled)
            return;

        //this.MoveToDirection(this.Direction * -1);
        Cell previousOrigin = this.Cell;
        this.Cell = this.TargetCell;
        this.Direction *= -1; // make sure to recomplute flipX
        this.MoveTo(previousOrigin);
    }

    public void Disable()
    {
        this.enabled = false;
        this.Move.TargetPosition = transform.position;
        //this.Animator.Play("Idle");
    }

    public void Enable()
    {
        this.Cell = GameManager.Grid.GetNearest(transform.position);
        this.MoveToNextTargetCell();

        //this.MoveToDirection(Vector2.zero);
        this.Animator.Play("Walk");
        this.enabled = true;
    }
}
