using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretOverlay : MonoBehaviour
{
    private static TurretOverlay Instance { get; set; }
    private Turret Turret { get; set; }

    private void Awake()
    {
        Instance = this;
        this.Hide();
    }

    public static void ShowStatic(Turret turret)
    {
        Instance.Show(turret);
    }

    public static void HideStatic()
    {
        Instance.Hide();
    }

    private void Show(Turret turret)
    {
        gameObject.SetActive(true);

        transform.position = turret.transform.position;
        transform.Find("Range").localScale = Vector3.one * turret.Range * 2f;
        this.Turret = turret;
    }

    private void Hide()
    {
        gameObject.SetActive(false);
    }
}
