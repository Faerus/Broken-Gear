using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthSystem
{
    public event EventHandler OnHealthChanged;
    public event EventHandler OnMaxHealthChanged;
    public event EventHandler<float> OnDamaged;
    public event EventHandler OnHealed;
    public event EventHandler OnDead;

    private float _health;
    public float Health
    {
        get { return _health; }
        set
        {
            // Check bounds
            if (value > this.MaxHealth)
            {
                value = this.MaxHealth;
            }
            else if (value < 0)
            {
                value = 0;
            }
            
            bool wasAlreadyDead = false;
            if(_health == 0)
            {
                wasAlreadyDead = true;
            }

            // Change health
            float previousValue = _health;
            _health = value;

            // Trigger events
            this.OnHealthChanged?.Invoke(this, EventArgs.Empty);
            if (previousValue < _health)
            {
                this.OnHealed?.Invoke(this, EventArgs.Empty);
            }
            else if (previousValue > _health)
            {
                this.OnDamaged?.Invoke(this, previousValue - _health);
            }
            if (_health == 0 && !wasAlreadyDead)
            {
                this.OnDead?.Invoke(this, EventArgs.Empty);
            }
        }
    }

    private float _maxHealth;
    public float MaxHealth
    {
        get { return _maxHealth; }
        set
        {
            _maxHealth = value;

            this.OnMaxHealthChanged?.Invoke(this, EventArgs.Empty);
            this.OnHealthChanged?.Invoke(this, EventArgs.Empty);
        }
    }

    public HealthSystem(float maxHealth)
    {
        _maxHealth = maxHealth;
        _health = maxHealth;
    }

    public float GetHealthPercent()
    {
        return this.Health / this.MaxHealth;
    }

    public bool IsDamaged()
    {
        return this.Health > 0 && this.Health < this.MaxHealth;
    }

    public bool IsDead()
    {
        return this.Health <= 0;
    }

    public void Damage(float amount)
    {
        this.Health -= amount;
    }

    public void Kill()
    {
        this.Damage(this.Health);
    }

    public void Heal(float amount)
    {
        this.Health += amount;
    }

    public void HealFull()
    {
        this.Health = this.MaxHealth;
    }

    /// <summary>
    /// Tries to get a HealthSystem from the GameObject
    /// The GameObject can have either the built in HealthSystemComponent script or any other script that creates
    /// the HealthSystem and implements the IGetHealthSystem interface
    /// </summary>
    /// <param name="getHealthSystemGameObject">GameObject to get the HealthSystem from</param>
    /// <param name="healthSystem">output HealthSystem reference</param>
    /// <param name="logErrors">Trigger a Debug.LogError or not</param>
    /// <returns></returns>
    public static bool TryGetHealthSystem(GameObject getHealthSystemGameObject, out HealthSystem healthSystem, bool logErrors = false)
    {
        healthSystem = null;

        // Check gameobject existance
        if (getHealthSystemGameObject == null)
        {
            if (logErrors)
            {
                Debug.LogError($"You need to assign the field 'getHealthSystemGameObject'!");
            }
            return false;
        }

        // Check getHealthSystem existance
        IGetHealthSystem getHealthSystem;
        if (!getHealthSystemGameObject.TryGetComponent(out getHealthSystem))
        {
            if (logErrors)
            {
                Debug.LogError($"Referenced Game Object '{getHealthSystemGameObject}' does not have a script that implements IGetHealthSystem!");
            }
            return false;
        }

        // Check HealthSystem existance
        healthSystem = getHealthSystem.HealthSystem;
        if (healthSystem == null)
        {
            if (logErrors)
            {
                Debug.LogError($"Got HealthSystem from object but healthSystem is null! Should it have been created? Maybe you have an issue with the order of operations.");
            }
            return false;
        }

        return true;
    }
}