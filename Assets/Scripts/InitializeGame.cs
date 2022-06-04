using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InitializeGame : MonoBehaviour
{
    [field: SerializeField]
    public Color ColorTeam1 { get; set; }

    [field: SerializeField]
    public Color ColorTeam2 { get; set; }

    public GameObject VictoryUI { get; set; }
    public TextMeshProUGUI VictoryText { get; set; }

    public Image RobotWinner { get; set; }
    public Image RobotLoser { get; set; }

    private void Awake()
    {
        this.VictoryUI = GameObject.Find("Victory UI");
        this.VictoryText = this.VictoryUI.transform.Find("Winners").GetComponent<TextMeshProUGUI>();
        this.VictoryUI.SetActive(false);

        var robot = this.VictoryUI.transform.Find("Robots");
        this.RobotWinner = robot.Find("Winner").GetComponent<Image>();
        this.RobotLoser = robot.Find("Loser").GetComponent<Image>();

        // Start game
        GameManager.Initialize(this.ColorTeam1, this.ColorTeam2);

        // Initial gears
        GameManager.AddGears(GameManager.ColorTeam1, 15);
        GameManager.AddGears(GameManager.ColorTeam2, 15);

        // Initial buildings
        BuildOnClick builder = this.GetComponent<BuildOnClick>();
        // White
        builder.Build(BuildingTypes.Drill, 0, 4, false);
        builder.Build(BuildingTypes.Factory, 0, 3, false);

        // Red
        builder.Build(BuildingTypes.Drill, 6, 0, false);
        builder.Build(BuildingTypes.Factory, 5, 0, false);
    }

    private void Update()
    {
        if(GameManager.HasGameEnded(out Color winnerTeam))
        {
            string teamName = winnerTeam == GameManager.ColorTeam1 ? "Blancs" : "Rouges";
            this.VictoryText.text = $"Les {teamName} remportent la partie";
            this.VictoryUI.SetActive(true);

            this.RobotWinner.color = winnerTeam;
            this.RobotLoser.color = winnerTeam == GameManager.ColorTeam1 ? GameManager.ColorTeam2 : GameManager.ColorTeam1;
        }
    }
}
