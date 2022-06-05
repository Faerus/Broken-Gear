using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InitializeGame : MonoBehaviour
{
    [field: SerializeField]
    public Color ColorTeam1 { get; set; }

    [field: SerializeField]
    public Color ColorTeam2 { get; set; }

    public bool GameAlreadyEnded { get; set; }
    public GameObject VictoryUI { get; set; }
    public CanvasGroup CanvasGroup { get; set; }
    public TextMeshProUGUI VictoryText { get; set; }

    [field: SerializeField]
    public AudioSource AudioVictory { get; set; }

    public Image RobotWinner { get; set; }
    public Image RobotLoser { get; set; }

    private void Awake()
    {
        this.VictoryUI = GameObject.Find("Victory UI");
        this.CanvasGroup = this.VictoryUI.GetComponent<CanvasGroup>();
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
        int x, y;
        this.GetRandomStartCoordinate(out x, out y);
        builder.Build(BuildingTypes.Drill, x, y, false);

        this.GetRandomStartCoordinate(out x, out y, x, y);
        builder.Build(BuildingTypes.Factory, x, y, false);

        // Red
        this.GetRandomStartCoordinate(out x, out y);
        builder.Build(BuildingTypes.Drill, x + 5, y, false);

        this.GetRandomStartCoordinate(out x, out y, x, y);
        builder.Build(BuildingTypes.Factory, x + 5, y, false);
    }

    private void GetRandomStartCoordinate(out int x, out int y, int excludeX = -1, int excludeY = -1)
    {
        x = Enumerable.Range(0, 2)
            .Except(new[] { excludeX })
            .OrderBy(i => Guid.NewGuid())
            .First();
        y = Enumerable.Range(0, 5)
            .Except(new[] { excludeY })
            .OrderBy(i => Guid.NewGuid())
            .First();
    }

    private void Update()
    {
        if(this.GameAlreadyEnded)
        { 
            this.CanvasGroup.alpha = Mathf.Lerp(this.CanvasGroup.alpha, 1, Time.deltaTime);
        }
        else if (GameManager.HasGameEnded(out Color winnerTeam))
        {
            string teamName = winnerTeam == GameManager.ColorTeam1 ? "Blancs" : "Rouges";
            this.VictoryText.text = $"Les {teamName} remportent la partie";
            this.VictoryUI.SetActive(true);

            this.RobotWinner.color = winnerTeam;
            this.RobotLoser.color = winnerTeam == GameManager.ColorTeam1 ? GameManager.ColorTeam2 : GameManager.ColorTeam1;
            
            this.GameAlreadyEnded = true;
            this.AudioVictory.Play();
        }
    }
}
