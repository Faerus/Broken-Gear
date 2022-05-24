using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveWaypoints : MonoBehaviour
{
    [field: SerializeField]
    private Vector3[] Waypoints { get; set; }
    private int Index { get; set; }
    private IMovePosition Move { get; set; }

    private void Start()
    {
        this.Move = this.GetComponent<IMovePosition>();
    }

    private void Update()
    {
        this.Move.TargetPosition = this.Waypoints[this.Index];
        if(this.Move.IsAtTargetPosition())
        {
            // Reached position
            this.Index = (this.Index + 1) % this.Waypoints.Length;
        }
    }
}