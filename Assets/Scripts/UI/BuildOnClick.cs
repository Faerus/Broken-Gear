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
        if (GameManager.SelectedBuilding != BuildingTypes.None && Input.GetMouseButtonDown(0))
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            this.Build(GameManager.SelectedBuilding, mousePos);
            //GameManager.SelectedBuilding = Building.None;
        }
    }

    private void Build(BuildingTypes building, Vector3 position)
    {
        // We need cell coordinates and do not allow building on middle column
        int x, y;
        if(!GameManager.Grid.TryGetXY(position, out x, out y) || x == GameManager.GRID_NEUTRAL_COLUMN)
        {
            return;
        }

        this.Build(building, x, y, true, position);
    }

    public void Build(BuildingTypes building, int x, int y, bool payGears = true, Vector3? precisePosition = null)
    {
        // We need a cell that has no building already
        Cell cell = GameManager.Grid[x, y];
        if (cell.Building != null)
        {
            return;
        }

        Color teamColor = cell.X > GameManager.GRID_NEUTRAL_COLUMN ? GameManager.ColorTeam2 : GameManager.ColorTeam1;
        Vector3 originPosition = precisePosition.GetValueOrDefault(cell.GetCenterWorldPosition());
        originPosition.z = 0;
        GameObject prefab = null;
        switch (building)
        {
            case BuildingTypes.Drill:
                // Spend gears
                if (payGears && !GameManager.RemoveGears(teamColor, GameManager.PRICE_DRILL))
                {
                    return;
                }

                prefab = this.DrillPrefab;
                originPosition += this.DrillOriginPosition;
                break;

            case BuildingTypes.Factory:
                // Spend gears
                if (payGears && !GameManager.RemoveGears(teamColor, GameManager.PRICE_FACTORY))
                {
                    return;
                }
                prefab = this.FactoryPrefab;
                originPosition += this.FactoryOriginPosition;
                break;

            case BuildingTypes.Turret:
                // Spend gears
                if (payGears && !GameManager.RemoveGears(teamColor, GameManager.PRICE_TURRET))
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
        if (cell.X > GameManager.GRID_NEUTRAL_COLUMN)
        {
            spriteRenderer.flipX = false;
        }
    }
}
