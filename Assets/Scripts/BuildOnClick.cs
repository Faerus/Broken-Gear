using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildOnClick : MonoBehaviour
{
    [field: SerializeField]
    public GameObject DrillPrefab { get; set; }
    [field: SerializeField]
    public Vector3 DrillOriginPosition { get; set; }

    [field: SerializeField]
    public GameObject FactoryPrefab { get; set; }
    [field: SerializeField]
    public Vector3 FactoryOriginPosition { get; set; }

    [field: SerializeField]
    public GameObject TurretPrefab { get; set; }
    [field: SerializeField]
    public Vector3 TurretOriginPosition { get; set; }

    [field: SerializeField]
    public Color Team2Color { get; set; }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.SelectedBuilding != Building.None && Input.GetMouseButtonDown(0))
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            if (GameManager.Grid.TryGetXY(mousePos, out int x, out int y) && x != 3)
            {
                Cell cell = GameManager.Grid[x, y];
                if (cell.Building == null )
                {
                    GameObject prefab = null;
                    Vector3 originPosition = mousePos;
                    originPosition.z = 0;
                    switch (GameManager.SelectedBuilding)
                    {
                        case Building.Drill:
                            prefab = this.DrillPrefab;
                            originPosition += this.DrillOriginPosition;
                            break;

                        case Building.Factory:
                            prefab = this.FactoryPrefab;
                            originPosition += this.FactoryOriginPosition;
                            break;

                        case Building.Turret:
                            prefab = this.TurretPrefab;
                            originPosition += this.TurretOriginPosition;
                            break;
                    }

                    cell.Building = Instantiate(prefab, originPosition, Quaternion.identity);
                    if (cell.X > 3)
                    {
                        SpriteRenderer spriteRenderer = cell.Building.GetComponent<SpriteRenderer>();
                        spriteRenderer.color = this.Team2Color;
                        spriteRenderer.flipX = false;
                    }

                    //GameManager.SelectedBuilding = Building.None;
                }
            }
        }
    }
}
