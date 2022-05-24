using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DamagePopup : MonoBehaviour
{
    // Create a Damage Popup
    public static DamagePopup Create(Vector3 position, int damageAmount, bool isCriticalHit = false)
    {
        var o = Resources.Load<Transform>("Prefabs/pfDamagePopup");
        Transform damagePopupTransform = Instantiate(o, position, Quaternion.identity);

        DamagePopup damagePopup = damagePopupTransform.GetComponentInChildren<DamagePopup>();
        damagePopup.Setup(damageAmount, isCriticalHit);
        return damagePopup;
    }

    private const float DISAPPEAR_TIMER_MAX = 1f;

    private static int SortingOrder { get; set; }

    private TextMeshPro TextMesh { get; set; }
    private float DisappearTimer { get; set; }
    private Color TextColor;
    private Vector3 MoveVector { get; set; }

    private void Awake()
    {
        this.TextMesh = transform.GetComponent<TextMeshPro>();
    }

    public void Setup(int damageAmount, bool isCriticalHit)
    {
        this.TextMesh.SetText(damageAmount.ToString());
        if (!isCriticalHit)
        {
            // Normal hit
            this.TextMesh.fontSize = 36;
            this.TextColor = Utils.GetColorFromString("505050"); // FFC500
        }
        else
        {
            // Critical hit
            this.TextMesh.fontSize = 45;
            this.TextColor = Utils.GetColorFromString("FF2B00");
        }

        this.TextColor.g += Random.Range(-.1f, +.1f);
        this.TextMesh.color = this.TextColor;
        this.DisappearTimer = DISAPPEAR_TIMER_MAX;

        SortingOrder++;
        this.TextMesh.sortingOrder = SortingOrder;

        this.MoveVector = new Vector3(.7f, 1) * 2f;
    }

    private void Update()
    {
        transform.position += this.MoveVector * Time.deltaTime;
        this.MoveVector -= this.MoveVector * 8f * Time.deltaTime;

        if (this.DisappearTimer > DISAPPEAR_TIMER_MAX * .5f)
        {
            // First half of the popup lifetime
            float increaseScaleAmount = 1f;
            transform.localScale += Vector3.one * increaseScaleAmount * Time.deltaTime;
        }
        else
        {
            // Second half of the popup lifetime
            float decreaseScaleAmount = 1f;
            transform.localScale -= Vector3.one * decreaseScaleAmount * Time.deltaTime;
        }

        this.DisappearTimer -= Time.deltaTime;
        if (this.DisappearTimer < 0)
        {
            // Start disappearing
            float disappearSpeed = 3f;
            this.TextColor.a -= disappearSpeed * Time.deltaTime;
            this.TextMesh.color = TextColor;
            if (this.TextColor.a < 0)
            {
                Destroy(gameObject.transform.parent.gameObject);
            }
        }
    }

}
