using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitializeGame : MonoBehaviour
{
    [field: SerializeField]
    public Color ColorTeam1 { get; set; }

    [field: SerializeField]
    public Color ColorTeam2 { get; set; }

    // Start is called before the first frame update
    void Start()
    {
        GameManager.Initialize(this.ColorTeam1, this.ColorTeam2);
    }
}
