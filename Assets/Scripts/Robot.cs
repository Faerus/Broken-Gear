using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Robot : Actor
{
    [field: SerializeField]
    public int Power { get; set; }

    [field: SerializeField]
    public ParticleSystem DamageParticleSystem { get; set; }

    [field: SerializeField]
    public ParticleSystem HealParticleSystem { get; set; }

    public SpriteRenderer SpriteRenderer { get; set; }
    public Rigidbody2D Rigidbody { get; set; }
    public Animator Animator { get; set; }
    public RandomoveOnGrid RandoMove { get; set; }

    private float InvincibleUntil { get; set; }
    private float DelaySinceLastCollision { get; set; }

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();

        this.SpriteRenderer = this.GetComponentInChildren<SpriteRenderer>();
        this.Rigidbody = this.GetComponentInChildren<Rigidbody2D>();
        this.RandoMove = this.GetComponent<RandomoveOnGrid>();
        this.Animator = this.GetComponentInChildren<Animator>();
        this.Animator.Play("Walk");

        this.HealthSystem.OnDead += this.HealthSystem_OnDead;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent<Robot>(out Robot robot))
        {
            if (robot.TeamColor == this.TeamColor)
            {
                // Heal friends
                robot.Heal(this.Power);
            }
            else
            {
                // Attack ennemies
                this.Animator.Play("Attack");
                robot.Damage(this.Power, transform.position);
            }
        }
        else if (collision.TryGetComponent<SpriteRenderer>(out SpriteRenderer spriteRenderer) && spriteRenderer.color != this.TeamColor && collision.TryGetComponent<HealthSystemComponent>(out HealthSystemComponent healthSystemComponent))
        {
            // Attack
            this.Animator.Play("Attack");
            healthSystemComponent.HealthSystem.Damage(this.Power);
            //this.DamageParticleSystem.Play();
        }

        this.DelaySinceLastCollision = 0;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        // Nothing to do if invincible
        this.DelaySinceLastCollision += Time.deltaTime;
        if (this.DelaySinceLastCollision > 3)
        {
            this.OnTriggerEnter2D(collision);
        }
    }

    public void Heal(int amount)
    {
        if (this.HealthSystem.IsDamaged())
        {
            this.HealthSystem.Heal(amount);
            this.HealParticleSystem.Play();
        }
    }

    public void Damage(int amount, Vector3 sourcePosition)
    {
        // Nothing to do if invincible
        if(this.InvincibleUntil > Time.time)
        {
            return;
        }

        this.HealthSystem.Damage(amount);
        this.DamageParticleSystem.Play();

        // Knockback
        Vector3 moveDir = (transform.position - sourcePosition).normalized;
        transform.position += moveDir * .3f;

        //DamagePopup.Create(transform.position, amount);
        if(UnityEngine.Random.Range(0, 2) < 1)
            this.RandoMove.TurnBack();

        this.InvincibleUntil = Time.time + .2f;
    }

    public bool IsDead()
    {
        return this.HealthSystem.IsDead();
    }

    private void HealthSystem_OnDead(object sender, EventArgs e)
    {
        this.Rigidbody.simulated = false;
        this.SpriteRenderer.sortingOrder = -1;
        this.RandoMove.Disable();

        this.Animator.SetBool("isDead", true);
        GameManager.RegisterRobotDead(this.TeamColor);
        GameManager.AddGearsToOtherTeam(this.TeamColor, GameManager.PRICE_PER_ROBOT_KILLED);

        Destroy(this.gameObject, 20);
    }
}
