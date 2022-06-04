using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Building : Actor
{
    public abstract BuildingTypes BuildingType { get; }
}

public enum BuildingTypes
{
    None,
    Drill,
    Factory,
    Turret
}
