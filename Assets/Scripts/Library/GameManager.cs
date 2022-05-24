using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class GameManager
{
    public static event EventHandler OnScoreChanged;

    public static Grid<Cell> Grid { get; set; }
    public static Dictionary<Color, int> RobotCreated { get; private set; }
    public static Dictionary<Color, int> RobotDead { get; private set; }
    public static Dictionary<Color, int> Scraps { get; private set; }

    public static Building SelectedBuilding { get; set; }
    public static List<Robot> Robots { get; set; }

    static GameManager()
    {
        Robots = new List<Robot>();
        RobotCreated = new Dictionary<Color, int>();
        RobotDead = new Dictionary<Color, int>();
        Scraps = new Dictionary<Color, int>();
        SelectedBuilding = Building.None;
    }

    public static void Initialize()
    {
        Grid = new Grid<Cell>(7, 5, 2.4f, 2.4f, new Vector3(-7.8f, -6f, 0), (grid, x, y) => new Cell(grid, x, y));

        Grid[0, 3].Building = GameObject.Find("Factory 1");
        Grid[6, 0].Building = GameObject.Find("Factory 2");
    }

    public static Robot GetClosestEnemy(Vector3 position, float maxRange, Color myTeam)
    {
        var enemies = Robots.Where(r => !r.IsDead() && r.TeamColor != myTeam).ToList();
        Robot closest = null;
        foreach (Robot enemy in enemies)
        {
            if (Vector3.Distance(position, enemy.transform.position) <= maxRange)
            {
                if (closest == null)
                {
                    closest = enemy;
                }
                else if (Vector3.Distance(position, enemy.transform.position) < Vector3.Distance(position, closest.transform.position))
                {
                    closest = enemy;
                }
            }
        }

        return closest;
    }

    public static void RegisterRobotCreated(Color team, int amount = 1)
    {
        if (!RobotCreated.ContainsKey(team))
        {
            RobotCreated.Add(team, 0);
        }

        RobotCreated[team] += amount;
        OnScoreChanged?.Invoke(null, EventArgs.Empty);
    }

    public static void RegisterRobotDead(Color team, int amount = 1)
    {
        if (!RobotDead.ContainsKey(team))
        {
            RobotDead.Add(team, 0);
        }

        RobotDead[team] += amount;
        OnScoreChanged?.Invoke(null, EventArgs.Empty);
    }

    public static void AddScraps(Color team, int amount = 10)
    {
        if (!Scraps.ContainsKey(team))
        {
            Scraps.Add(team, 0);
        }

        Scraps[team] += amount;
        OnScoreChanged?.Invoke(null, EventArgs.Empty);
    }

    public static int GetRobotCount(Color team)
    {
        int created = RobotCreated.ContainsKey(team) ? RobotCreated[team] : 0;
        int dead = RobotDead.ContainsKey(team) ? RobotDead[team] : 0;

        return created - dead;
    }
}

public enum Building
{
    None,
    Drill,
    Factory,
    Turret
}
