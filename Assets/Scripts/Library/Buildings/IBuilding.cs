using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IBuilding
{
    BuildingTypes BuildingType { get; }
    HealthSystem HealthSystem { get; }
    Vector3 Position { get; }
    Color TeamColor { get; }
}

