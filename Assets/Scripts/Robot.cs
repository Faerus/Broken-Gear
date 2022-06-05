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

    public AudioSource AudioAttack { get; set; }
    public AudioSource AudioHeal { get; set; }
    public AudioSource AudioDie { get; set; }
    public AudioSource AudioWalk { get; set; }
    public AudioSource AudioDestroy { get; set; }

    private float InvincibleUntil { get; set; }
    private float DelaySinceLastCollision { get; set; }

    protected override void Awake()
    {
        base.Awake();
        this.SpriteRenderer = this.GetComponentInChildren<SpriteRenderer>();
        this.Rigidbody = this.GetComponentInChildren<Rigidbody2D>();
        this.RandoMove = this.GetComponent<RandomoveOnGrid>();
        this.Animator = this.GetComponentInChildren<Animator>();
        this.Animator.Play("Walk");

        var audio = transform.Find("Audio");
        this.AudioAttack = audio.Find("Attack").GetComponent<AudioSource>();
        this.AudioHeal = audio.Find("Heal").GetComponent<AudioSource>();
        this.AudioDie = audio.Find("Die").GetComponent<AudioSource>();
        this.AudioWalk = audio.Find("Walk").GetComponent<AudioSource>();
        this.AudioDestroy = audio.Find("Destroy").GetComponent<AudioSource>();
    }

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        this.HealthSystem.OnDead += this.HealthSystem_OnDead;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Manage collision with actors only
        Actor actor;
        if (!collision.TryGetComponent<Actor>(out actor))
        {
            return;
        }

        // Attack ennemy
        if(this.TeamColor != actor.TeamColor)
        {
            this.AudioAttack.Play();
            this.Animator.Play("Attack");
            actor.Damage(this.Power, transform.position);

            if(actor is Building && actor.HealthSystem.IsDead())
            {
                this.AudioDestroy.Play();
            }
        }
        else if (actor is Robot)
        {
            // Heal friend robots
            actor.Heal(this.Power);
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

    public override void Heal(int amount)
    {
        if (this.HealthSystem.IsDamaged())
        {
            this.AudioHeal.Play();
            this.HealthSystem.Heal(amount);
            this.HealParticleSystem.Play();
        }
    }

    public override void Damage(int amount, Vector3 sourcePosition)
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
        this.AudioDie.Play();
        this.AudioWalk.Stop();
        this.Rigidbody.simulated = false;
        this.SpriteRenderer.sortingOrder = -1;
        this.RandoMove.Disable();

        this.Animator.SetBool("isDead", true);
        GameManager.RegisterRobotDead(this.TeamColor);
        GameManager.AddGearsToOtherTeam(this.TeamColor, GameManager.PRICE_PER_ROBOT_KILLED);

        Destroy(this.gameObject, 20);
    }
}
