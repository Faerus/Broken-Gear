using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveVelocity : MonoBehaviour, IMoveVelocity
{
    [field: SerializeField]
    public float Speed { get; set; }
    public Vector3 Velocity { get; set; }

    private Rigidbody2D RigidBody { get; set; }

    // Start is called before the first frame update
    void Start()
    {
        this.RigidBody = this.GetComponent<Rigidbody2D>();
    }

    // Update is called once per fixed frame
    void FixedUpdate()
    {
        this.RigidBody.velocity = this.Velocity * this.Speed;
    }
}
