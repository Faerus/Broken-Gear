using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Factory : MonoBehaviour
{
    private HealthSystem HealthSystem { get; set; }
    private BoxCollider2D Collider { get; set; }

    [field: SerializeField]
    public ParticleSystem DeadParticleSystem { get; set; }

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
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void HealthSystem_OnDead(object sender, EventArgs e)
    {
        this.Collider.enabled = false;
        this.DeadParticleSystem.Play();
        GameManager.AddGearsToOtherTeam(this.TeamColor, GameManager.PRICE_PER_BUILDING_KILLED);
        Destroy(this.gameObject, 1);
    }
}
