using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectBuildingOnClick : MonoBehaviour
{
    [field: SerializeField]
    public BuildingTypes BuildingToSelect { get; set; }

    // Start is called before the first frame update
    void Start()
    {
        this.GetComponent<Button>().onClick.AddListener(this.SelectBuilding);
    }

    private void SelectBuilding()
    {
        GameManager.SelectedBuilding = this.BuildingToSelect;
    }
}
