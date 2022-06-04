using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotAI : MonoBehaviour
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

    private void HealthSystem_OnDead(object sender, System.EventArgs e)
    {
        this.enabled = false;
    }

    // Update is called once per frame
    private void Update()
    {
        switch(this.Strategy)
        {
            case Strategies.ClosestActor:
                this.TargetActor = GameManager.GetClosestEnemy<Actor>(this.Robot.Position, 100, this.Robot.TeamColor);
                break;

            case Strategies.ClosestBuilding:
                this.TargetActor = GameManager.GetClosestEnemy<Building>(this.Robot.Position, 100, this.Robot.TeamColor);
                break;

            case Strategies.WeakestActor:
                this.TargetActor = GameManager.GetWeakestEnemy<Actor>(this.Robot.Position, 100, this.Robot.TeamColor);
                break;
        }

        
        if (this.TargetActor != null)
        {
            this.Robot.RandoMove.Disable();
            this.MoveTo(this.TargetActor);
        }
        else if(!this.Robot.RandoMove.enabled)
        {
            this.Robot.RandoMove.Enable();
        }
    }

    private void MoveTo(Actor actor)
    {
        this.Robot.RandoMove.Move.TargetPosition = actor.Position;
    }
}
