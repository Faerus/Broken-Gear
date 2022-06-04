using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Factory : Building
{
    public override BuildingTypes BuildingType { get { return BuildingTypes.Factory; } }

    private BoxCollider2D Collider { get; set; }

    [field: SerializeField]
    public ParticleSystem DeadParticleSystem { get; set; }

    protected override void Awake()
    {
        base.Awake();

        this.Collider = this.GetComponent<BoxCollider2D>();
    }
    protected override void Start()
    {
        base.Start();

        this.HealthSystem.OnDead += this.HealthSystem_OnDead;
    }

    private void HealthSystem_OnDead(object sender, EventArgs e)
    {
        this.Collider.enabled = false;
        this.DeadParticleSystem.Play();
        GameManager.AddGearsToOtherTeam(this.TeamColor, GameManager.PRICE_PER_BUILDING_KILLED);
        Destroy(this.gameObject, 1);
    }
}
