using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour
{
    private Vector3 ShootPosition { get; set; }

    [field: SerializeField]
    public float Range { get; set; }

    [field: SerializeField]
    public int Power { get; set; }

    [field: SerializeField]
    public float ShootDelay { get; set; }
    private float ShootDelayCurrent { get; set; }

    [field: SerializeField]
    public float ArrowSpeed { get; set; }

    private HealthSystem HealthSystem { get; set; }
    private BoxCollider2D Collider { get; set; }
    public SpriteRenderer SpriteRenderer { get; set; }
    public Color TeamColor
    {
        get { return this.SpriteRenderer.color; }
        set { this.SpriteRenderer.color = value; }
    }

    [field: SerializeField]
    public ParticleSystem DeadParticleSystem { get; set; }

    [field: SerializeField]
    private GameObject Arrow { get; set; }

    // Start is called before the first frame update
    void Awake()
    {
        this.ShootPosition = transform.Find("ShootPosition").position;
        //this.Range = 5;
        //this.Power = 15;
        //this.ShootDelay = .4f;
        //this.ArrowSpeed = 5f;

        this.SpriteRenderer = this.GetComponentInChildren<SpriteRenderer>();
        this.Collider = this.GetComponent<BoxCollider2D>();
        if (HealthSystem.TryGetHealthSystem(this.gameObject, out HealthSystem healthSystem))
        {
            this.HealthSystem = healthSystem;
            this.HealthSystem.OnDead += this.HealthSystem_OnDead;
        }
    }

    // Update is called once per frame
    void Update()
    {
        this.ShootDelayCurrent -= Time.deltaTime;
        if (this.ShootDelayCurrent <= 0f)
        {
            this.ShootDelayCurrent = this.ShootDelay;

            Robot enemy = GameManager.GetClosestEnemy(transform.position, this.Range, this.TeamColor);
            if (enemy != null)
            {
                GameObject arrowGameObject = Instantiate(this.Arrow, this.ShootPosition, Quaternion.identity);
                Arrow arrow = arrowGameObject.GetComponent<Arrow>();
                arrow.Target = enemy;
                arrow.Power = this.Power;
                arrow.Speed = this.ArrowSpeed;
            }
        }
    }

    private void HealthSystem_OnDead(object sender, EventArgs e)
    {
        this.Collider.enabled = false;
        this.DeadParticleSystem.Play();
        Destroy(this.gameObject, 1);
    }

    private void OnMouseEnter()
    {
        TurretOverlay.ShowStatic(this);
    }
    private void OnMouseExit()
    {
        TurretOverlay.HideStatic();
    }
    private void OnDestroy()
    {
        TurretOverlay.HideStatic();
    }
}
