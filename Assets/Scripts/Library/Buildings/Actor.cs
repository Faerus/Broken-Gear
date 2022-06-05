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

    public virtual void Heal(int amount)
    {
        if (this.HealthSystem.IsDamaged())
        {
            this.HealthSystem.Heal(amount);
        }
    }

    public virtual void Damage(int amount, Vector3 sourcePosition)
    {
        this.HealthSystem.Damage(amount);
    }

    protected virtual void OnDestroy()
    {
        GameManager.Actors.Remove(this);
    }
}