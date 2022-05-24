using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePositionDirect : MonoBehaviour, IMovePosition
{
    [field: SerializeField]
    public float ReachTargetDistance { get; set; }
    public Vector3 TargetPosition { get; set; }
    private IMoveVelocity MoveVelocity { get; set; }

    public bool IsAtTargetPosition()
    {
        return Vector3.Distance(this.TargetPosition, transform.position) <= this.ReachTargetDistance;
    }

    private void Start()
    {
        this.MoveVelocity = this.GetComponent<IMoveVelocity>();
    }

    private void Update()
    {
        // Stop when reaching destination
        if (this.IsAtTargetPosition())
        {
            this.MoveVelocity.Velocity = Vector3.zero;
        }
        else
        {
            this.MoveVelocity.Velocity = (this.TargetPosition - transform.position).normalized;
        }
    }
}
