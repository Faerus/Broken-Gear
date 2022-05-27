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
                    Color teamColor = cell.X > 3 ? GameManager.ColorTeam2 : GameManager.ColorTeam1;
                    GameObject prefab = null;
                    Vector3 originPosition = mousePos;
                    originPosition.z = 0;
                    switch (GameManager.SelectedBuilding)
                    {
                        case Building.Drill:
                            // Spend gears
                            if (!GameManager.RemoveGears(teamColor, GameManager.PRICE_DRILL))
                            {
                                return;
                            }

                            prefab = this.DrillPrefab;
                            originPosition += this.DrillOriginPosition;
                            break;

                        case Building.Factory:
                            // Spend gears
                            if (!GameManager.RemoveGears(teamColor, GameManager.PRICE_FACTORY))
                            {
                                return;
                            }
                            prefab = this.FactoryPrefab;
                            originPosition += this.FactoryOriginPosition;
                            break;

                        case Building.Turret:
                            // Spend gears
                            if (!GameManager.RemoveGears(teamColor, GameManager.PRICE_TURRET))
                            {
                                return;
                            }
                            prefab = this.TurretPrefab;
                            originPosition += this.TurretOriginPosition;
                            break;
                    }

                    cell.Building = Instantiate(prefab, originPosition, Quaternion.identity);
                    SpriteRenderer spriteRenderer = cell.Building.GetComponent<SpriteRenderer>();
                    spriteRenderer.color = teamColor;
                    if (cell.X > 3)
                    {
                        spriteRenderer.flipX = false;
                    }

                    //GameManager.SelectedBuilding = Building.None;
                }
            }
        }
    }
}
