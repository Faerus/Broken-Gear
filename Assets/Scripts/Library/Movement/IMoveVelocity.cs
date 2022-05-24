using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IMoveVelocity
{
    float Speed { get; set; }

    Vector3 Velocity { get; set; }
}
