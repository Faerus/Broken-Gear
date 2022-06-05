using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotAI : MonoBehaviour, ITargetCellProvider
{
    public enum Strategies
    {
        None,
        ClosestActor,
        ClosestBuilding,
        WeakestActor
    }

    public Robot Robot { get; private set; }
    public Strategies Strategy { get; set; }
    private Actor TargetActor { get; set; }

    // Start is called before the first frame update
    private void Awake()
    {
        this.Robot = this.GetComponent<Robot>();
    }

    private void Start()
    {
        this.Robot.HealthSystem.OnDead += this.HealthSystem_OnDead;
    }

    private void HealthSystem_OnDead(object sender, EventArgs e)
    {
        this.enabled = false;
    }

    public bool GetTargetPosition(Cell currentCell, out Vector3 targetPosition, out Vector2 direction)
    {
        Actor targetActor = null;
        float viewDistance = 100;
        float minLifePercentBeforeLookingForHelp = 0.2f;

        if (this.Robot.HealthSystem != null && this.Robot.HealthSystem.GetHealthPercent() < minLifePercentBeforeLookingForHelp)
        {
            targetActor = GameManager.GetClosest<Robot>(this.Robot.Position, viewDistance, this.Robot.TeamColor, this.Robot);
        }

        if (targetActor == null)
        {
            switch (this.Strategy)
            {
                case Strategies.ClosestActor:
                    targetActor = GameManager.GetClosestEnemy<Actor>(this.Robot.Position, viewDistance, this.Robot.TeamColor);
                    break;

                case Strategies.ClosestBuilding:
                    targetActor = GameManager.GetClosestEnemy<Building>(this.Robot.Position, viewDistance, this.Robot.TeamColor);
                    break;

                case Strategies.WeakestActor:
                    targetActor = GameManager.GetWeakestEnemy<Actor>(this.Robot.Position, viewDistance, this.Robot.TeamColor);
                    break;
            }
        }

        if (targetActor == null)
        {
            targetPosition = Vector3.zero;
            direction = Vector2.zero;
            return false;
        }

        Vector3 distance = targetActor.Position - transform.position;
        float absDistanceX = Mathf.Abs(distance.x);
        float absDistanceY = Mathf.Abs(distance.y);

        // Compute direction
        int directionX = 0, directionY = 0;
        if (absDistanceX > absDistanceY)
        {
            // Move horizontally
            directionX = (int)(distance.x / absDistanceX);
        }
        else
        {
            // Move vertically
            directionY = (int)(distance.y / absDistanceY);
        }
        direction = new Vector2(directionX, directionY);

        if (absDistanceX < GameManager.Grid.CellWidth || absDistanceY < GameManager.Grid.CellHeight)
        {
            // Focus on target if on same cell
            targetPosition = targetActor.Position;
        }
        else
        {
            // Get to next closer cell if too far
            targetPosition = currentCell.UpRight(directionX, directionY).GetRandomWorldPosition();
        }

        this.TargetActor = targetActor;
        return true;
    }
}
