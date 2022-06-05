using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : Building
{
    public override BuildingTypes BuildingType { get { return BuildingTypes.Turret; } }

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

    private BoxCollider2D Collider { get; set; }
    public SpriteRenderer SpriteRenderer { get; set; }

    [field: SerializeField]
    public ParticleSystem DeadParticleSystem { get; set; }

    [field: SerializeField]
    private GameObject Arrow { get; set; }

    public AudioSource AudioShot { get; set; }

    // Start is called before the first frame update
    protected override void Awake()
    {
        base.Awake();
        //this.Range = 5;
        //this.Power = 15;
        //this.ShootDelay = .4f;
        //this.ArrowSpeed = 5f;

        this.ShootPosition = transform.Find("ShootPosition").position;
        this.SpriteRenderer = this.GetComponentInChildren<SpriteRenderer>();
        this.Collider = this.GetComponent<BoxCollider2D>();
        this.AudioShot = this.GetComponentInChildren<AudioSource>();
    }
    protected override void Start()
    {
        base.Start();

        this.HealthSystem.OnDead += this.HealthSystem_OnDead;
    }

    // Update is called once per frame
    private void Update()
    {
        this.ShootDelayCurrent -= Time.deltaTime;
        if (this.ShootDelayCurrent <= 0f)
        {
            this.ShootDelayCurrent = this.ShootDelay;

            Robot enemy = GameManager.GetClosestEnemy<Robot>(transform.position, this.Range, this.TeamColor);
            if (enemy != null)
            {
                GameObject arrowGameObject = Instantiate(this.Arrow, this.ShootPosition, Quaternion.identity);
                Arrow arrow = arrowGameObject.GetComponent<Arrow>();
                arrow.Target = enemy;
                arrow.Power = this.Power;
                arrow.Speed = this.ArrowSpeed;

                this.SpriteRenderer.flipX = this.ShootPosition.x < enemy.transform.position.x;
                this.AudioShot.Play();
            }
        }
    }

    private void HealthSystem_OnDead(object sender, EventArgs e)
    {
        this.Collider.enabled = false;
        this.DeadParticleSystem.Play();
        GameManager.AddGearsToOtherTeam(this.TeamColor, GameManager.PRICE_PER_BUILDING_KILLED);
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
    protected override void OnDestroy()
    {
        base.OnDestroy();
        TurretOverlay.HideStatic();
    }
}
