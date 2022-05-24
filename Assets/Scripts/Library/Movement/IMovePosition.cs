using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IMovePosition
{
    float ReachTargetDistance { get; set; }

    Vector3 TargetPosition { get; set; }

    bool IsAtTargetPosition();
}
