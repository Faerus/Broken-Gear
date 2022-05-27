using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Robot : MonoBehaviour
{
    [field: SerializeField]
    public int Power { get; set; }

    [field: SerializeField]
    public ParticleSystem DamageParticleSystem { get; set; }

    [field: SerializeField]
    public ParticleSystem HealParticleSystem { get; set; }

    public HealthSystem HealthSystem { get; private set; }

    public SpriteRenderer SpriteRenderer { get; set; }
    public Rigidbody2D Rigidbody { get; set; }
    public Animator Animator { get; set; }
    public RandomoveOnGrid RandoMove { get; set; }
    private IMovePosition Move { get; set; }

    private float InvincibleUntil { get; set; }

    public Color TeamColor { get; private set; }

    // Start is called before the first frame update
    void Start()
    {
        this.SpriteRenderer = this.GetComponentInChildren<SpriteRenderer>();
        this.TeamColor = this.SpriteRenderer.color;
        this.Rigidbody = this.GetComponentInChildren<Rigidbody2D>();
        this.RandoMove = this.GetComponent<RandomoveOnGrid>();
        this.Move = this.GetComponent<IMovePosition>();
        this.Animator = this.GetComponentInChildren<Animator>();
        this.Animator.Play("Walk");

        if (HealthSystem.TryGetHealthSystem(this.gameObject, out HealthSystem healthSystem))
        {
            this.HealthSystem = healthSystem;
            this.HealthSystem.OnDead += this.HealthSystem_OnDead;
        }

        GameManager.Robots.Add(this);
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (this.SpriteRenderer == null) // Sometimes occurs during gameobject destroy
            return;

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
        this.Move.TargetPosition = transform.position;
        this.RandoMove.enabled = false;
        this.SpriteRenderer.sortingOrder = -1;

        this.Animator.SetBool("isDead", true);
        GameManager.RegisterRobotDead(this.TeamColor);
        GameManager.AddGearsToOtherTeam(this.TeamColor, GameManager.PRICE_PER_ROBOT_KILLED);

        Destroy(this.gameObject, 20);
    }

    private void OnDestroy()
    {
        GameManager.Robots.Remove(this);
    }
}
