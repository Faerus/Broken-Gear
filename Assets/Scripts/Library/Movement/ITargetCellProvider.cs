using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ITargetCellProvider
{
    bool GetTargetPosition(Cell currentCell, out Vector3 targetPosition, out Vector2 direction);
}
