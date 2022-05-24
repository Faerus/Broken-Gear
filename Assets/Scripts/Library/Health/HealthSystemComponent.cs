using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthSystemComponent : MonoBehaviour, IGetHealthSystem
{
    [SerializeField]
    [Tooltip("Maximum Health amount")]
    private float _healthAmountMax = 100f;
    public float HealthAmountMax
    {
        get { return _healthAmountMax; }
        set { _healthAmountMax = value; }
    }

    [field: SerializeField]
    [field: Tooltip("Starting Health amount, leave at 0 to start at full health.")]
    private float StartingHealthAmount { get; set; }

    public HealthSystem HealthSystem { get; private set; }

    private void Awake()
    {
        // Create Health System
        this.HealthSystem = new HealthSystem(this.HealthAmountMax);

        if (this.StartingHealthAmount > 0 && this.StartingHealthAmount < this.HealthAmountMax)
        {
            this.HealthSystem.Health = this.StartingHealthAmount;
        }
    }
}