using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarUI : MonoBehaviour
{

    [field: Tooltip("Optional; Either assign a reference in the Editor (that implements IGetHealthSystem) or manually call SetHealthSystem()")]
    [field: SerializeField]
    private GameObject GetHealthSystemGameObject { get; set; }

    [field: Tooltip("Image to show the Health Bar, should be set as Fill, the script modifies fillAmount")]
    [field: SerializeField]
    private Image Image { get; set; }

    private HealthSystem _healthSystem;
    public HealthSystem HealthSystem
    {
        get { return _healthSystem; }
        set
        {
            if (_healthSystem != null)
            {
                _healthSystem.OnHealthChanged -= this.HealthSystem_OnHealthChanged;
            }

            _healthSystem = value;
            this.UpdateHealthBar();

            _healthSystem.OnHealthChanged += this.HealthSystem_OnHealthChanged;
            _healthSystem.OnDamaged += this.HealthSystem_OnDamaged;
        }
    }

    private void HealthSystem_OnDamaged(object sender, float amount)
    {
        DamagePopup.Create(transform.position, (int)amount);
    }

    private void Start()
    {
        if (HealthSystem.TryGetHealthSystem(this.GetHealthSystemGameObject, out HealthSystem healthSystem, true))
        {
            this.HealthSystem = healthSystem;
        }
    }

    private void HealthSystem_OnHealthChanged(object sender, EventArgs e)
    {
        this.UpdateHealthBar();
    }

    private void UpdateHealthBar()
    {
        this.transform.parent.gameObject.SetActive(this.HealthSystem.IsDamaged());
        this.Image.fillAmount = this.HealthSystem.GetHealthPercent(); // Mathf.Lerp(this.Image.fillAmount, this.HealthSystem.GetHealthPercent(), Time.deltaTime);
    }

    private void OnDestroy()
    {
        if(this.HealthSystem != null)
        { 
            this.HealthSystem.OnHealthChanged -= this.HealthSystem_OnHealthChanged;
            this.HealthSystem.OnDamaged -= this.HealthSystem_OnDamaged;
        }
    }
}
