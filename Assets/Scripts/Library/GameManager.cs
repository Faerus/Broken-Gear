using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class GameManager
{
    public const int MAX_ROBOT_PER_TEAM = 20;
    public const int MAX_GEARS_PER_TEAM = 99;

    public const int PRICE_ROBOT = 5;
    public const int PRICE_PER_ROBOT_KILLED = 5;
    public const int PRICE_PER_BUILDING_KILLED = 20;
    public const int PRICE_DRILL = 40;
    public const int PRICE_FACTORY = 50;
    public const int PRICE_TURRET = 20;

    public static event EventHandler OnScoreChanged;

    public static Grid<Cell> Grid { get; set; }
    public static Dictionary<Color, int> RobotCreated { get; private set; }
    public static Dictionary<Color, int> RobotDead { get; private set; }
    public static Dictionary<Color, int> Gears { get; private set; }

    public static Building SelectedBuilding { get; set; }
    public static List<Robot> Robots { get; set; }

    public static Color ColorTeam1 { get; set; }
    public static Color ColorTeam2 { get; set; }

    static GameManager()
    {
        Robots = new List<Robot>();
        RobotCreated = new Dictionary<Color, int>();
        RobotDead = new Dictionary<Color, int>();
        Gears = new Dictionary<Color, int>();
        SelectedBuilding = Building.None;
    }

    public static void Initialize(Color team1, Color team2)
    {
        ColorTeam1 = team1;
        ColorTeam2 = team2;

        Grid = new Grid<Cell>(7, 5, 2.4f, 2.4f, new Vector3(-7.8f, -6f, 0), (grid, x, y) => new Cell(grid, x, y));

        Grid[0, 3].Building = GameObject.Find("Factory 1");
        Grid[6, 0].Building = GameObject.Find("Factory 2");

        AddGears(Utils.GetColorFromString("FFFFFF"), 20);
        AddGears(new Color(1, 0.35f, 0.3f, 1), 20);
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

    public static void AddGears(Color team, int amount = 10)
    {
        if (!Gears.ContainsKey(team))
        {
            Gears.Add(team, 0);
        }

        int gears = Gears[team] + amount;
        if (gears >= MAX_GEARS_PER_TEAM)
        { 
            gears = MAX_GEARS_PER_TEAM;
        }
        Gears[team] = gears;

        OnScoreChanged?.Invoke(null, EventArgs.Empty);
    }

    public static void AddGearsToOtherTeam(Color team, int amount = 10)
    {
        Color otherTeam = GetOtherTeamColor(team);
        AddGears(otherTeam, amount);
    }

    public static bool RemoveGears(Color team, int amount = 10)
    {
        if (!Gears.ContainsKey(team))
        {
            Gears.Add(team, 0);
        }

        int gears = Gears[team];
        if(gears < amount)
        {
            return false;
        }

        Gears[team] -= amount;
        OnScoreChanged?.Invoke(null, EventArgs.Empty);
        return true;
    }

    public static int GetRobotCount(Color team)
    {
        int created = RobotCreated.ContainsKey(team) ? RobotCreated[team] : 0;
        int dead = RobotDead.ContainsKey(team) ? RobotDead[team] : 0;

        return created - dead;
    }

    public static Color GetOtherTeamColor(Color myTeam)
    {
        return ColorTeam1 == myTeam ? ColorTeam2 : ColorTeam1;
    }
}

public enum Building
{
    None,
    Drill,
    Factory,
    Turret
}
