using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drill : MonoBehaviour
{
    private HealthSystem HealthSystem { get; set; }
    private BoxCollider2D Collider { get; set; }

    [field: SerializeField]
    public ParticleSystem DeadParticleSystem { get; set; }

    [field: SerializeField]
    public float Frequency { get; set; }

    [field: SerializeField]
    public int Gears { get; set; }

    public Color TeamColor { get; set; }

    // Start is called before the first frame update
    void Start()
    {
        this.Collider = this.GetComponent<BoxCollider2D>();
        this.TeamColor = this.GetComponent<SpriteRenderer>().color;
        if (HealthSystem.TryGetHealthSystem(this.gameObject, out HealthSystem healthSystem))
        {
            this.HealthSystem = healthSystem;
            this.HealthSystem.OnDead += this.HealthSystem_OnDead;
        }

        InvokeRepeating("GenerateGears", 1, this.Frequency);
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
