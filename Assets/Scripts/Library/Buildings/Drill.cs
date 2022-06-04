using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drill : Building
{
    public override BuildingTypes BuildingType { get { return BuildingTypes.Drill; } }

    private BoxCollider2D Collider { get; set; }

    [field: SerializeField]
    public ParticleSystem DeadParticleSystem { get; set; }

    [field: SerializeField]
    public float Frequency { get; set; }

    [field: SerializeField]
    public int Gears { get; set; }

    protected override void Awake()
    {
        base.Awake();

        this.Collider = this.GetComponent<BoxCollider2D>();
        InvokeRepeating("GenerateGears", 1, this.Frequency);
        
    }
    protected override void Start()
    {
        base.Start();

        this.HealthSystem.OnDead += this.HealthSystem_OnDead;
    }

    private void GenerateGears()
    {
        GameManager.AddGears(this.TeamColor, this.Gears);
    }

    private void HealthSystem_OnDead(object sender, EventArgs e)
    {
        this.Collider.enabled = false;
        this.DeadParticleSystem.Play();
        GameManager.AddGearsToOtherTeam(this.TeamColor, GameManager.PRICE_PER_BUILDING_KILLED);
        Destroy(this.gameObject, 1);
    }
}