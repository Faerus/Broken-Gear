using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DisplayScores : MonoBehaviour
{
    [field: SerializeField]
    public Color Team1Color { get; set; }
    [field: SerializeField]
    public TextMeshProUGUI Team1ScoreText { get; set; }

    [field: SerializeField]
    public Color Team2Color { get; set; }
    [field: SerializeField]
    public TextMeshProUGUI Team2ScoreText { get; set; }

    // Start is called before the first frame update
    void Start()
    {
        GameManager.OnScoreChanged += this.GameManager_OnScoreChanged;
    }

    private void GameManager_OnScoreChanged(object sender, System.EventArgs e)
    {
        this.UpdateScore(this.Team1Color, this.Team1ScoreText);
        this.UpdateScore(this.Team2Color, this.Team2ScoreText);
    }

    private void UpdateScore(Color teamColor, TextMeshProUGUI textMesh)
    {
        int created = GameManager.RobotCreated.ContainsKey(teamColor) ? GameManager.RobotCreated[teamColor] : 0;
        int dead = GameManager.RobotDead.ContainsKey(teamColor) ? GameManager.RobotDead[teamColor] : 0;
        textMesh.text = $"{created}\r\n{dead}\r\n{created - dead}";
    }
}
