using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Actor : MonoBehaviour
{
    public Color TeamColor { get; set; }

    public HealthSystem HealthSystem { get; private set; }

    public Vector3 Position { get { return transform.position; } }

    protected virtual void Awake()
    {
        GameManager.Actors.Add(this);
    }

    protected virtual void Start()
    {
        this.HealthSystem = this.GetComponent<IGetHealthSystem>().HealthSystem;
        this.TeamColor = this.GetComponentInChildren<SpriteRenderer>().color;
    }

    protected virtual void OnDestroy()
    {
        GameManager.Actors.Remove(this);
    }
}