using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveTransformVelocity : MonoBehaviour, IMoveVelocity
{
    [field: SerializeField]
    public float Speed { get; set; }
    public Vector3 Velocity { get; set; }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += this.Velocity * this.Speed * Time.deltaTime;
    }
}
