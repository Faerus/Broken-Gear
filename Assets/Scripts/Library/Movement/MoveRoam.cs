using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveRoam : MonoBehaviour
{
    private Vector3 StartPosition { get; set; }
    private IMovePosition Move { get; set; }

    private void Start()
    {
        this.StartPosition = transform.position;
        this.Move = this.GetComponent<IMovePosition>();
        this.Move.TargetPosition = this.GetRandomPosition();
    }

    private void Update()
    {
        if(this.Move.IsAtTargetPosition())
        {
            this.Move.TargetPosition = this.GetRandomPosition();
        }
    }

    private Vector3 GetRandomPosition()
    {
        return this.StartPosition + Utils.GetRandomDir() * Random.Range(30f, 100f);
    }
}
