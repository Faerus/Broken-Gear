using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

//public class MovePositionPathfinding : MonoBehaviour, IMovePosition
//{
//    public float ReachTargetDistance { get; set; }
//    private IMoveVelocity MoveVelocity { get; set; }
//    private List<Vector3> PathVectors { get; set; }
//    private int PathIndex { get; set; }

//    private void Start()
//    {
//        this.MoveVelocity = this.GetComponent<IMoveVelocity>();
//        this.PathIndex = -1;
//    }

//    public void SetTargetPosition(Vector3 targetPosition)
//    {
//        //this.PathVectors = GridPathfinding.instance.GetPathRouteWithShortcuts(transform.position, targetPosition).pathVectorList;

//        // Remove first position so he doesn't go backwards
//        if (this.PathVectors.Any())
//        {
//            this.PathVectors.RemoveAt(0);
//        }

//        this.PathIndex = this.PathVectors.Any() ? 0 : -1;
//    }

//    public bool IsAtTargetPosition()
//    {
//        return Vector3.Distance(this.TargetPosition, transform.position) <= this.ReachTargetDistance;
//    }

//    private void Update()
//    {
//        if(this.PathIndex < 0)
//        {
//            // Idle
//            this.MoveVelocity.Velocity = Vector3.zero;
//            return;
//        }

//        // Move to next path position
//        Vector3 nextPathPosition = this.PathVectors[this.PathIndex];
//        this.MoveVelocity.Velocity = (nextPathPosition - transform.position).normalized;

//        if (Vector3.Distance(transform.position, nextPathPosition) < this.ReachTargetDistance)
//        {
//            ++this.PathIndex;

//            // End of path
//            if (this.PathIndex >= this.PathVectors.Count)
//            {
//                this.PathIndex = -1;
//            }
//        }
//    }

//}
